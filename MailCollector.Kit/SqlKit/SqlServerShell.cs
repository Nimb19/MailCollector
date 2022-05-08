using FastMember;
using MailCollector.Kit.SqlKit.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MailCollector.Kit.SqlKit
{
    public class SqlServerShell : IDisposable
    {
        private const int CommandTimeout = 30;
        private const string DbName = "MailCollectorStorage";

        private readonly SqlConnection _sqlConn;
        private readonly SqlServerSettings _sqlServerSettings;

        public SqlServerShell(SqlServerSettings sqlServerSettings)
        {
            _sqlServerSettings = sqlServerSettings;

            var sqlStringBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = _sqlServerSettings.ServerName,
                InitialCatalog = "master",
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
            var cmdtxt = $"SELECT * FROM master.dbo.sysdatabases WHERE name = '{DbName}'";
            if (ReadArrayOfStrings(cmdtxt).Length == 0)
            {
                Dispose();
                throw new DbDoesNotExistException($"Базы данных '{DbName}' не существует. Создайте её дистрибутивом.");
            }
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
            var cmd = _sqlConn.CreateCommand();
            cmd.CommandTimeout = CommandTimeout;
            cmd.CommandType = commandType;
            cmd.CommandText = command;

            return cmd;
        }

        public SqlDataReader ExecuteReader(string command, CommandType commandType = CommandType.Text)
        {
            using (var cmd = CreateCommand(command, commandType))
                return cmd.ExecuteReader();
        }

        public object ExecuteScalar(string command, CommandType commandType = CommandType.Text)
        {
            using (var cmd = CreateCommand(command, commandType))
                return cmd.ExecuteScalar();
        }

        public int ExecuteNonQuery(string command, CommandType commandType = CommandType.Text)
        {
            using (var cmd = CreateCommand(command, commandType))
                return cmd.ExecuteNonQuery();
        }

        public void Dispose()
        {
            _sqlConn?.Dispose();
        }
    }
}
