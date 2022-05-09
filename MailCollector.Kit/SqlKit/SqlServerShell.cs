using FastMember;
using MailCollector.Kit.SqlKit.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MailCollector.Kit.SqlKit
{
    /// <summary>
    ///     Потокобезопасная оболочка для работы с БД. 
    ///     Не содержит бизнес логики, вся она находится в SqlServerShellAdapter.
    /// </summary>
    public class SqlServerShell : IDisposable
    {
        internal const int CommandTimeout = 30;
        internal const string InitialCatalog = "master";
        internal const string DbName = "MailCollectorStorage";

        private readonly SqlConnection _sqlConn;
        private readonly SqlServerSettings _sqlServerSettings;

        private object _lock = new object();

        public SqlServerShell(SqlServerSettings sqlServerSettings)
        {
            _sqlServerSettings = sqlServerSettings;

            var sqlStringBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = _sqlServerSettings.ServerName,
                InitialCatalog = InitialCatalog,
                IntegratedSecurity = _sqlServerSettings.IntegratedSecurity ?? true,
                ConnectTimeout = CommandTimeout,
            };

            if (!sqlStringBuilder.IntegratedSecurity)
            {
                sqlStringBuilder.UserID = _sqlServerSettings.Login;
                sqlStringBuilder.Password = _sqlServerSettings.Password;
            }

            _sqlConn = new SqlConnection(sqlStringBuilder.ConnectionString);
            _sqlConn.Open();

            ThrowDBIfItDoesNotExist();
        }

        private void ThrowDBIfItDoesNotExist()
        {
            var cmdtxt = $"SELECT * FROM {InitialCatalog}.dbo.sysdatabases WHERE name = '{DbName}'";
            if (ReadArrayOfStrings(cmdtxt).Length == 0)
            {
                Dispose();
                throw new DbDoesNotExistException($"Базы данных '{DbName}' не существует. Создайте её дистрибутивом.");
            }
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

        public T[] GetArrayOf<T>(string tableName, string where = null) where T : class, new()
        {
            var getTableValuesCmd = $"select * from [{DbName}].[dbo].[{tableName}] ";

            if (where != null)
                getTableValuesCmd += where;

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

        public SqlCommand CreateCommand(string command, CommandType commandType = CommandType.Text)
        {
            SqlCommand cmd;
            lock (_lock)
            {
                cmd = _sqlConn.CreateCommand();
            }

            cmd.CommandTimeout = CommandTimeout;
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

        public void Dispose()
        {
            _sqlConn?.Dispose();
        }
    }
}
