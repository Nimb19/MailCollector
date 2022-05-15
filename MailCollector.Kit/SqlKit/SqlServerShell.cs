using FastMember;
using MailCollector.Kit.Logger;
using MailCollector.Kit.SqlKit.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace MailCollector.Kit.SqlKit
{
    /// <summary>
    ///     Потокобезопасная оболочка для работы с СУБД и выбранной БД. 
    ///     Не содержит бизнес логики, вся она находится в адаптерах рядом с этим классом.
    /// </summary>
    public class SqlServerShell : IDisposable
    {
        private readonly ILogger _logger;
        private readonly string _moduleInfo = "SqlServerShell";
        
        private object _lock = new object();
        
        public const int ConnectionTimeoutInSeconds = 3;

        public string DbName { get; set; }
        public readonly int CommandTimeoutInSeconds = 30;
        public readonly string InitialCatalog = "master";
        public readonly SqlServerSettings SqlServerSettings;

        private DbmsType DbType { get; set; } = DbmsType.Mssql; // Пока не придираемся к типу, потом доделаю
        public SqlConnection SqlCon { get; private set; }
        
        public SqlServerShell(SqlServerSettings sqlServerSettings, ILogger logger
            , string moduleInfo, string dbName)
        {
            _logger = logger;
            DbName = string.IsNullOrWhiteSpace(dbName) ? null : dbName.Trim();
            SqlServerSettings = sqlServerSettings;
            
            if (!string.IsNullOrWhiteSpace(moduleInfo))
                _moduleInfo = moduleInfo;

            if (sqlServerSettings.CommandTimeoutInSeconds.HasValue)
                CommandTimeoutInSeconds = sqlServerSettings.CommandTimeoutInSeconds.Value;

            GetConnection();

            if (DbName != null)
                ThrowDBIfItDoesNotExist();
        }

        private void ThrowDBIfItDoesNotExist()
        {
            //var cmdtxt = $"SELECT * FROM {InitialCatalog}.dbo.sysdatabases WHERE name = '{DbName}'";
            if (!IsDbExist(DbName))
            {
                Dispose();
                throw new DbDoesNotExistException($"Базы данных '{DbName}' не существует");
            }
        }

        private void GetConnection()
        {
            var scsb = new SqlConnectionStringBuilder()
            {
                DataSource = SqlServerSettings.ServerName,
                InitialCatalog = InitialCatalog,
                ApplicationName = _moduleInfo,
                ConnectTimeout = ConnectionTimeoutInSeconds,
            };

            if (SqlServerSettings.IntegratedSecurity.HasValue
                && SqlServerSettings.IntegratedSecurity.Value == true)
            {
                scsb.IntegratedSecurity = true;
            }
            else
            {
                scsb.IntegratedSecurity = false;
                scsb.UserID = SqlServerSettings.Login;
                scsb.Password = SqlServerSettings.Password;
            }

            SqlCon = new SqlConnection(scsb.ToString());
            SqlCon.Open();

            //var splitedSqlString = SqlServerSettings.ServerName.Split('@');
            //if (splitedSqlString.Length > 1)
            //{
            //    DbType = splitedSqlString.Last().Contains("mssql") ? DbmsType.Mssql : DbmsType.Unknown;
            //}
            //else
            //{
            //    DbType = DbmsType.Mssql;
            //}

            //if (DbType == DbmsType.Mssql)
            //{
            //    var scsb = new SqlConnectionStringBuilder()
            //    {
            //        DataSource = SqlServerSettings.ServerName,
            //        InitialCatalog = InitialCatalog,
            //        ApplicationName = _moduleInfo,
            //        ConnectTimeout = CommandTimeoutInSeconds,
            //    };

            //    if ((SqlServerSettings.IntegratedSecurity.HasValue 
            //        && SqlServerSettings.IntegratedSecurity.Value == true))
            //    {
            //        scsb.IntegratedSecurity = true;
            //    }
            //    else
            //    {
            //        scsb.IntegratedSecurity = false;
            //        scsb.UserID = SqlServerSettings.Login;
            //        scsb.Password = SqlServerSettings.Password;
            //    }

            //    SqlCon = new SqlConnection(scsb.ToString());
            //    SqlCon.Open();
            //}
            //else if (DBType == DbmsType.Pgsql) // когда понадобится доделаю
            //{
            //    var pscsb = new NpgsqlConnectionStringBuilder()
            //    {
            //        Host = ServerName,
            //        Database = "postgres",
            //        InternalCommandTimeout = _commandTimeoutInSeconds,
            //        Timeout = _commandTimeoutInSeconds,
            //    };

            //    if (Password == null)
            //    {
            //        pscsb.IntegratedSecurity = true;
            //        pscsb.Username = Login.IsNullOrWhiteSpace() ? "" : Login;
            //    }
            //    else
            //    {
            //        pscsb.IntegratedSecurity = false;
            //        pscsb.Username = Login;
            //        pscsb.Password = Password;
            //    }

            //    SqlCon = new NpgsqlConnection(pscsb.ToString());
            //    SqlCon.Open();
            //}
            //else
            //{
            //    throw new NotImplementedException($"Тип базы {DbType} не поддерживается");
            //}
        }

        //public void Update<T>(T entity, T etalonEntity, string tableName) where T : class, new() // TODO: переделаю потом
        //{
        //    GetSerializedType(entity, tableName, out var columnNamesResult, out var columnsValuesResult);

        //    GetSerializedType(etalonEntity, tableName, out var columnNamesResultEtalon, out var columnsValuesResultEtalon);

        //    var columnNamesArray = columnNamesResult.Split(',').Select(x => x.Trim())
        //        .Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        //    var columnsValuesArray = columnsValuesResult.Split(',').Select(x => x.Trim())
        //        .Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        //    var columnsValuesArrayEtalon = columnsValuesResultEtalon.Split(',').Select(x => x.Trim())
        //        .Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

        //    var start = false;
        //    var setBuilder = new StringBuilder();
        //    for (int i = 0; i < columnNamesArray.Length; i++)
        //    {
        //        if (columnsValuesArray[i] != columnsValuesArrayEtalon[i])
        //        {
        //            if (start)
        //                setBuilder.Append(", ");
        //            setBuilder.Append($"{columnNamesArray[i]} = {columnsValuesArray[i]}");
        //            start = true;
        //        }
        //    }

        //    var cmdTxt = $"UPDATE [{DbName}].dbo.{tableName} SET {setBuilder} WHERE Id = {entity.Id}";

        //    if (ExecuteNonQuery(cmdTxt) == 0)
        //        throw new Affected0RowsException();
        //}

        public void UpdateCell(string columnName, string cellValue, string where, string tableName)
        {
            var cmdTxt = $"UPDATE [{DbName}].dbo.{tableName} SET {columnName} = {cellValue} {where}";

            if (ExecuteNonQuery(cmdTxt) == 0)
                throw new Affected0RowsException();
        }

        public void RemoveWhere<T>(string whereCondition, string tableName) where T : class, new()
        {
            var cmdTxt = $"DELETE FROM [{DbName}].dbo.{tableName} WHERE {whereCondition}";

            if (ExecuteNonQuery(cmdTxt) == 0)
                throw new Affected0RowsException();
        }

        public T[] GetWhere<T>(string whereCondition, string tableName) where T : class, new()
        {
            var cmdTxt = $"SELECT * FROM [{DbName}].[dbo].[{tableName}] WHERE {whereCondition}";
            return ReadArrayOf<T>(cmdTxt);
        }

        public T GetById<T>(int id, string tableName) where T : class, new()
        {
            var cmdTxt = $"SELECT * FROM [{DbName}].[dbo].[{tableName}] WHERE Id = {id}";
            return ReadAs<T>(cmdTxt);
        }

        public T GetByUid<T>(int uid, string tableName) where T : class, new()
        {
            var cmdTxt = $"SELECT * FROM [{DbName}].[dbo].[{tableName}] WHERE Uid = {uid}";
            return ReadAs<T>(cmdTxt);
        }

        public T[] GetArrayOf<T>(string tableName, string where = null, string orderBy = null) where T : class, new()
        {
            var getTableValuesCmd = $"select * from [{DbName}].[dbo].[{tableName}] ";

            if (where != null)
                getTableValuesCmd += where;

            if (orderBy != null)
                getTableValuesCmd += orderBy;

            return ReadArrayOf<T>(getTableValuesCmd);
        }

        public void Insert<T>(T obj, string tableName)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            var insertCmd = ConvertTypeInstanceToInsertCmd(obj, tableName);

            if (ExecuteNonQuery(insertCmd) == 0)
                throw new Affected0RowsException();
        }

        public T ReadAs<T>(string getCmd) where T : class, new()
        {
            return ReadArrayOf<T>(getCmd).FirstOrDefault();
        }

        public string ConvertTypeInstanceToInsertCmd<T>(T obj, string tableName)
        {
            const string ColumnNamesConst = "@@TYPENAMES@@";
            const string ColumnValuesConst = "@@TYPEVALUE@@";

            GetSerializedType(obj, tableName, out var columnNamesResult, out var columnsValuesResult);

            var etaloncmd = $"INSERT INTO [{DbName}].dbo.{tableName} ({ColumnNamesConst}) " +
                $"VALUES ({ColumnValuesConst})";

            var result = etaloncmd.Replace(ColumnNamesConst, columnNamesResult);
            result = result.Replace(ColumnValuesConst, columnsValuesResult);

            return result;
        }

        public void GetSerializedType<T>(T obj, string tableName, out string columnNamesResult, out string columnsValuesResult)
        {
            var type = obj.GetType();
            var columnNames = ReadColumnNames(tableName);

            var list = new List<(string ColumnName, object ColumnValue)>();

            var properties = type.GetProperties();
            foreach (var columnName in columnNames)
            {
                if (columnName.ToLower() == "id")
                    continue;

                var matchedProp = properties
                    .FirstOrDefault(x => x.Name.ToLower() == columnName.ToLower());
                if (matchedProp != null)
                {
                    var value = matchedProp.GetValue(obj);
                    if (value != null)
                        list.Add((columnName, value));
                }
            }

            columnNamesResult = string.Join(","
                , list.Select(x => x.ColumnName).ToArray());
            columnsValuesResult = string.Join(", "
                , list.Select(x => ObjectToStringValue(x.ColumnValue)).ToArray());
        }

        public string[] ReadColumnNames(string tableName)
        {
            var cmdtxt = $"SELECT COLUMN_NAME " +
                $"FROM[{DbName}].INFORMATION_SCHEMA.COLUMNS " +
                $"WHERE TABLE_NAME = N'{tableName}'";
            return ReadArrayOfStrings(cmdtxt);
        }

        public string ObjectToStringValue(object obj)
        {
            var type = obj?.GetType();
            if (type == null)
                return "NULL";
            else if (type.Name == nameof(String))
                return $"N'{(string)obj}'";
            else if (type.IsEnum)
                return Convert.ToString((int)obj);
            else if (DateTime.TryParse(obj.ToString(), out var dt))
                return $"'{dt.FormatToDate()}'";
            else
                return obj.ToString();
        }

        public T[] ReadArrayOf<T>(string getCmd) where T : class, new()
        {
            var list = new List<T>();
            using (var reader = ExecuteReader(getCmd))
            {
                while (reader.Read())
                {
                    var typeimplementation = ConvertRowToType<T>(reader);
                    list.Add(typeimplementation);
                }
            }
            return list.ToArray();
        }

        public static T ConvertRowToType<T>(SqlDataReader rd) where T : class, new()
        {
            Type type = typeof(T);
            var accessor = TypeAccessor.Create(type);
            var members = accessor.GetMembers();
            var t = new T();

            for (int i = 0; i < rd.FieldCount; i++)
            {
                if (!rd.IsDBNull(i))
                {
                    string fieldName = rd.GetName(i);

                    if (members.Any(m => string.Equals(m.Name, fieldName
                        , StringComparison.InvariantCultureIgnoreCase)))
                    {
                        var value = rd.GetValue(i);
                        if (value != null)
                            accessor[t, fieldName] = value;
                    }
                }
            }

            return t;
        }

        public string[] ReadArrayOfStrings(string cmdText)
        {
            var list = new List<string>();
            using (var reader = ExecuteReader(cmdText))
            {
                while (reader.Read())
                {
                    var value = reader.GetString(0);
                    list.Add(value);
                }
            }
            return list.ToArray();
        }

        #region CommonExtensions

        /// <summary> Возвращает версию SQL Server </summary>
        public Version GetSqlServerVersion()
        {
            Version version = null;
            try
            {
                if (SqlCon.State != ConnectionState.Open)
                    SqlCon.Open();
                version = new Version(SqlCon.ServerVersion.ToString());
            }
            catch (Exception ex)
            {
                _logger.Error($"Не удалось получить версию SQL Server {SqlCon.DataSource}. {ex}");
            }
            return version;
        }

        /// <summary> Возращает расположение файла БД </summary>
        public string GetPhysicalDbLocation(string dbName)
        {
            var scriptGetLocation = $@"SELECT mdf.physical_name 
                FROM (SELECT * FROM sys.master_files WHERE type_desc = 'ROWS' ) mdf
                WHERE mdf.name = '{dbName}'";

            return ExecuteScalar(scriptGetLocation) as string;
        }

        /// <summary> Восстановить бэкап </summary>
        public void RestoreBackup(string dbName, string pathToBakFolder, bool withNoRecovery = true)
        {
            var createBakScript = CreateScript_RestoreDB(dbName, pathToBakFolder, withNoRecovery);
            ExecuteNonQuery(createBakScript);
        }

        /// <summary> Сделать бэкап </summary>
        public string CreateBackup(string dbName, string pathToBakFolder)
        {
            var createBakScript = CreateScript_BackupDB(dbName, pathToBakFolder, out var backupDirectory, out var bakFullPath);
            if (!Directory.Exists(backupDirectory))
                Directory.CreateDirectory(backupDirectory);

            ExecuteNonQuery(createBakScript);

            return bakFullPath;
        }

        /// <summary> Выставить режим восстановления </summary>
        public void SetRecoveryMode(string dbName, RecoveryMode recoveryMode)
        {
            var scriptSetMode = CreateScript_SetRecoveryMode(dbName, recoveryMode);
            ExecuteNonQuery(scriptSetMode);
        }

        /// <summary> Существует ли указанная БД </summary>
        public bool IsDbExist(string dbName)
        {
            var script = CreateScript_IsDbExist(dbName);
            var objResult = ExecuteScalar(script);
            var result = Convert.ToString(objResult);
            return objResult != null && !string.IsNullOrWhiteSpace(result);
        }

        /// <summary> Получить список имён планов обслуживания </summary>
        public List<string> GetSubPlansNames()
        {
            var script = CreateScript_GetSubPlansNames();
            var names = GetList(script);
            return names;
        }

        #region CommonCreationScripts

        private string CreateScript_IsDbExist(string dbName)
        {
            if (DbType == DbmsType.Mssql)
                return $"SELECT DB_ID('{dbName}')";
            else
                return $@"SELECT 1 FROM pg_database WHERE datname='{dbName}'";
        }

        private string CreateScript_GetSubPlansNames()
        {
            return "use msdb select name from sysmaintplan_plans";
        }

        private string CreateScript_BackupDB(string dbName, string pathToBakFolder, out string backupDirectory, out string bakFullPath)
        {
            backupDirectory = Path.Combine(pathToBakFolder, SqlServerSettings.ServerName);
            if (backupDirectory.Last() != Path.DirectorySeparatorChar)
                backupDirectory += Path.DirectorySeparatorChar;

            var bakName = $"{dbName}_{DateTime.UtcNow:dd.MM.yyyy_(HH.mm)}.bak";
            bakFullPath = Path.Combine(backupDirectory, bakName);

            return $@"BACKUP DATABASE {dbName} TO DISK = '{bakFullPath}'";
        }

        private string CreateScript_SetRecoveryMode(string dbName, RecoveryMode recovereMode)
        {
            return $@"ALTER DATABASE [{dbName}] SET RECOVERY {recovereMode.ToString().ToUpper()}";
        }

        private string CreateScript_RestoreDB(string dbName, string pathToBakFolder, bool withNoRecovery)
        {
            var res = $@"RESTORE DATABASE [{dbName}] FROM DISK = '{pathToBakFolder}'";
            if (withNoRecovery)
                res += $@" WITH NORECOVERY";

            return res;
        }

        #endregion CommonCreationScripts

        #endregion CommonExtensions

        #region AgExtensions

        /// <summary> Взять имена AG групп </summary>
        public List<string> GetAgGroupsNames()
        {
            var scriptGetGroups = CreateScript_GetAgGroups();
            return GetList(scriptGetGroups);
        }

        public void SetHADRAGroup(string dbname, string agName)
        {
            var scriptSetHADR = $"ALTER DATABASE [{dbname}] SET HADR AVAILABILITY GROUP = [{agName}]";
            ExecuteNonQuery(scriptSetHADR);
        }

        public void AddDbToAg(string groupName, string dbName)
        {
            var addToAgScript = $"USE MASTER ALTER AVAILABILITY GROUP [{groupName}] ADD DATABASE [{dbName}]";
            ExecuteNonQuery(addToAgScript);
        }

        public string TakeAgFromSpecifiedDb(string dbName)
        {
            var takeScript = CreateScript_TakeAgFromSpecifiedDb(dbName);
            var agName = ExecuteScalar(takeScript);

            return Convert.ToString(agName);
        }

        #region AgCreationScripts

        private string CreateScript_GetAgGroups()
        {
            return $@"SELECT Groups.[Name] AS AGname
                FROM sys.dm_hadr_availability_group_states States
                INNER JOIN master.sys.availability_groups Groups ON States.group_id = Groups.group_id";
        }

        private string CreateScript_TakeAgFromSpecifiedDb(string dbName)
        {
            return $@"SELECT
                AG.name AS [AvailabilityGroupName],
                dbcs.database_name AS [DatabaseName]
                FROM master.sys.availability_groups AS AG
                INNER JOIN master.sys.availability_replicas AS AR
                   ON AG.group_id = AR.group_id
                INNER JOIN master.sys.dm_hadr_availability_replica_states AS arstates
                   ON AR.replica_id = arstates.replica_id AND arstates.is_local = 1
                INNER JOIN master.sys.dm_hadr_database_replica_cluster_states AS dbcs
                   ON arstates.replica_id = dbcs.replica_id
                WHERE dbcs.database_name = '{dbName}'";
        }

        #endregion AgCreationScripts

        #endregion AgExtensions

        #region PrincipalsExtensions

        /// <summary> Создаёт пользователей по подобию из sourceDB. Выдаёт те же права. </summary>
        public void CreateUsersBySourceDb(string sourceDb, string dbName)
        {
            var sourceDBUsers = GetUsers(sourceDb);

            var sourceUsers = new List<User>();
            foreach (var sourceUser in sourceDBUsers)
            {
                var roles = GetUserRoles(sourceDb, sourceUser);
                sourceUsers.Add(new User(sourceUser, roles));
            }

            CreateUsers(dbName, sourceUsers);
        }

        public List<string> GetUserRoles(string dbName, string userName)
        {
            var getRolesScript = CreateScript_GetUserRoles(dbName, userName);
            var roles = GetList(getRolesScript);
            return roles;
        }

        public List<string> GetUsers(string dbName)
        {
            var getSourceUsersScript = CreateScript_GetUsers(dbName);
            var sourceDBUsers = GetList(getSourceUsersScript);
            return sourceDBUsers;
        }

        public void CreateUsers(string dbName, IEnumerable<User> users)
        {
            var existingUsersName = GetUsers(dbName);

            foreach (var user in users)
            {
                // Если пользователь уже имеется, то убираем из списка добавляемых роли, которые у него уже есть
                if (existingUsersName.Any(x => x.Trim().ToLower() == user.Name.Trim().ToLower()))
                {
                    var getSlaveRolesScript = CreateScript_GetUserRoles(dbName, user.Name);
                    var existingRoles = GetList(getSlaveRolesScript);

                    foreach (var existingRole in existingRoles)
                    {
                        if (user.Roles.Contains(existingRole))
                            user.Roles.Remove(existingRole);
                    }
                }
                else
                {
                    var createUserScript = CreateScript_CreateUser(dbName, user.Name);
                    _logger.Trace($"createUserScript=>{createUserScript}");
                    ExecuteNonQuery(createUserScript);
                }

                foreach (var role in user.Roles)
                {
                    var addRoleScript = CreateScript_AddRoleToUser(dbName, role, user.Name);
                    _logger.Trace($"addRoleScript=>{addRoleScript}");
                    ExecuteNonQuery(addRoleScript);
                }
            }
        }

        /// <summary> Выставляет owner как и у БД источника. </summary>
        public void SetOwnerBySourceDb(string sourceDb, string db)
        {
            var owner = GetOwner(sourceDb);
            SetOwner(db, owner);
        }

        public string GetOwner(string db)
        {
            var scriptGetOwner = CreateScript_GetOwner(db);
            var owner = Convert.ToString(ExecuteScalar(scriptGetOwner));
            return owner;
        }

        public void SetOwner(string db, string owner)
        {
            var scriptSetOwner = CreateScript_SetOwner(db, owner);
            ExecuteNonQuery(scriptSetOwner);
        }

        #region PrincipalsCreationScripts

        private string CreateScript_SetOwner(string db, string owner)
        {
            if (DbType == DbmsType.Mssql)
                return $"use [{db}]; exec sp_changedbowner [{owner}]";
            else
                return $@"ALTER DATABASE {db} OWNER TO {owner}";
        }

        private static string CreateScript_AddRoleToUser(string db, string role, string user)
        {
            return $@"use [{db}]
                EXEC sp_AddRoleMember '{role}', '{user}'";
        }

        private static string CreateScript_CreateUser(string db, string userName)
        {
            return $@"use [{db}]
                CREATE USER [{userName}] FOR LOGIN [{userName}];";
        }

        private static string CreateScript_GetUsers(string db)
        {
            return $@"use [{db}]
                SELECT NAME
                FROM sys.database_principals
                WHERE Type IN (
                		'U'
                		,'S'
                		)
                	AND NAME NOT IN (
                		'dbo'
                		,'guest'
                		,'sys'
                		,'INFORMATION_SCHEMA'
                		)";
        }

        private static string CreateScript_GetUserRoles(string dbname, string user)
        {
            return $@"use [{dbname}]
                SELECT DBRole.name
                FROM sys.database_principals DBUser
                INNER JOIN sys.database_role_members DBM ON DBM.member_principal_id = DBUser.principal_id
                INNER JOIN sys.database_principals DBRole ON DBRole.principal_id = DBM.role_principal_id
                WHERE DBUser.name = '{user}'";
        }

        private string CreateScript_GetOwner(string dbname)
        {
            if (DbType == DbmsType.Mssql)
                return $@"use [{dbname}]
                    select sp.name [SQL Login Name] from sys.databases db left join sys.server_principals sp on db.owner_sid=sp.sid
                    where db.name='{dbname}'";
            else
                return $@"SELECT pg_catalog.pg_get_userbyid(d.datdba) FROM pg_catalog.pg_database d WHERE d.datname = '{dbname}'";
        }

        #endregion PrincipalsCreationScripts

        #endregion PrincipalsExtensions

        #region ReadExtensions

        private List<string> GetList(string script)
        {
            var list = new List<string>();
            using (var reader = ExecuteReader(script))
            {
                while (reader.Read())
                {
                    var user = reader.GetValue(0);
                    list.Add(Convert.ToString(user));
                }
            }
            return list;
        }

        private List<T> GetList<T>(string script)
        {
            var list = new List<T>();
            using (var reader = ExecuteReader(script))
            {
                while (reader.Read())
                {
                    var user = reader.GetValue(0);
                    list.Add((T)user);
                }
            }
            return list;
        }

        #endregion ReadExtensions

        #region ExecuteCommands

        public SqlCommand CreateCommand(string command, CommandType commandType = CommandType.Text)
        {
            SqlCommand cmd;
            lock (_lock)
            {
                cmd = SqlCon.CreateCommand();
            }

            cmd.CommandTimeout = CommandTimeoutInSeconds;
            cmd.CommandType = commandType;
            cmd.CommandText = command;

            return cmd;
        }

        public SqlDataReader ExecuteReader(string command, CommandType commandType = CommandType.Text)
        {
            using (var cmd = CreateCommand(command, commandType))
            {
                lock (_lock)
                {
                    return cmd.ExecuteReader();
                }
            }
        }

        public object ExecuteScalar(string command, CommandType commandType = CommandType.Text)
        {
            using (var cmd = CreateCommand(command, commandType))
            {
                lock (_lock)
                {
                    return cmd.ExecuteScalar();
                }
            }
        }

        public int ExecuteNonQuery(string command, CommandType commandType = CommandType.Text)
        {
            using (var cmd = CreateCommand(command, commandType))
            {
                lock (_lock)
                {
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion ExecuteCommands

        public void Dispose()
        {
            SqlCon?.Dispose();
        }
    }

    public enum DbmsType : short
    {
        Unknown,
        Mssql,
        Pgsql
    }

    public enum RecoveryMode
    {
        SIMPLE,
        FULL
    }

    public class User
    {
        public string Name { get; set; }
        public List<string> Roles { get; set; }

        public User(string name, List<string> roles)
        {
            Name = name;
            Roles = roles;
        }
    }
}
