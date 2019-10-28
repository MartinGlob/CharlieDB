using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace CharlieDb
{
    public partial class Charlie
    {
        //public static string GetPropValueForSql(object src, DbProperty pi)
        //{
        //    var ov = pi.Info.GetValue(src, null);

        //    if (ov == null)
        //        return "NULL";

        //    switch (pi.ElementType)
        //    {
        //        case ElementTypes.Enum:
        //        case ElementTypes.Guid:
        //        case ElementTypes.String:
        //        case ElementTypes.Char:
        //            return $"'{ov.ToString()}'";
        //        case ElementTypes.Boolean:
        //            return $"'{ov.ToString()}'";
        //        case ElementTypes.DateTime:
        //            return $"'{ov.ToString()}'";
        //        case ElementTypes.Byte:
        //        case ElementTypes.SByte:
        //        case ElementTypes.UInt32:
        //        case ElementTypes.UInt64:
        //        case ElementTypes.Int16:
        //        case ElementTypes.UInt16:
        //        case ElementTypes.Int32:
        //        case ElementTypes.Int64:
        //            return $"{ov.ToString()}";
        //        case ElementTypes.Double:
        //        case ElementTypes.Single:
        //        case ElementTypes.Decimal:
        //        case ElementTypes.Float:
        //            return $"'{Convert.ToDecimal(ov.ToString())}'";
        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }
        //}

        private void AddSqlParameters(SqlCommand sqlCmd, object par)
        {
            foreach (var p in par.GetType().GetProperties())
            {
                var v = p.GetValue(par, null);
                sqlCmd.Parameters.AddWithValue($@"{p.Name}", v);
            }
        }


        private object GetPropertyValue(DbProperty p, object data)
        {
            switch (p.Handler)
            {
                case PropertyHandler.Default:
                    return p.Info.GetValue(data, null);
                case PropertyHandler.AsJson:
                    return JsonConvert.SerializeObject(p.Info.GetValue(data, null));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public int DeleteAll<T>(SqlConnection connection)
        {
            var rc = GetRegisteredType<T>();
            using (var sqlCmd = new SqlCommand(rc.SqlInsert, connection))
            {
                SqlCommand cmd = new SqlCommand($"DELETE FROM {rc.SqlTableName}", connection);
                var n = cmd.ExecuteNonQuery();
                return n;
            }
        }

        public int Insert<T>(SqlConnection connection, T data)
        {
            var rc = GetRegisteredType<T>();

            if (rc.SqlInsert == null)
            {
                var outputOutId = rc._IdProperty != null ? "OUTPUT INSERTED.ID" : "";
                rc.SqlInsert =
                    $"INSERT INTO {rc.SqlTableName} " +
                    $"({string.Join(",", rc.Properties.Select(p => p.ColumnName))})" +
                    $" {outputOutId} " +
                    $"VALUES({string.Join(",", rc.Properties.Select(p => $"@{p.Name}"))})";
            }

            using (var sqlCmd = new SqlCommand(rc.SqlInsert, connection))
            {
                
                rc.Properties.ForEach(
                    p => sqlCmd.Parameters.AddWithValue($"@{p.Name}", GetPropertyValue(p, data)));

                if (rc._IdProperty != null)
                {
                    var id = (int)sqlCmd.ExecuteScalar();
                    rc._IdProperty.Info.SetValue(data, id);
                    return id;
                }
                else
                    return 0;
            }
        }

        private void SetPropertyValue(DbProperty p, object dst, object v)
        {
            switch (p.Handler)
            {
                case PropertyHandler.Default:
                    p.Info.SetValue(dst, v);
                    break;
                case PropertyHandler.AsJson:


                    var x = JsonConvert.DeserializeObject((string)v, p.T);
                    p.Info.SetValue(dst, x);
                    break;
                case PropertyHandler.Class:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Update<T>(SqlConnection conn, T data)
        {
            var rc = GetRegisteredType<T>();

            if (rc.SqlUpdate == null)
            {
                rc.SqlUpdate =
                    $"UPDATE {rc.SqlTableName} " +
                    $"SET {string.Join(",", rc.Properties.Select(p => p.ColumnName + "=@" + p.Name))} " +
                    $"WHERE {rc._IdProperty.ColumnName}=@{rc._IdProperty.Name}";
            }

            using (var sqlCmd = new SqlCommand(rc.SqlUpdate, conn))
            {
                rc.Properties.ForEach(p => sqlCmd.Parameters.AddWithValue($"@{p.Name}", GetPropertyValue(p, data)));
                sqlCmd.Parameters.AddWithValue($"@{rc._IdProperty.Name}", GetPropertyValue(rc._IdProperty, data));
                sqlCmd.ExecuteNonQuery();
            }
        }

        public IEnumerable<T> Get<T>(SqlConnection conn, string sqlWhereStatement, object parameters)
        {

            var rc = GetRegisteredType<T>();

            using (SqlCommand cmd = new SqlCommand($"SELECT * from {rc.SqlTableName} WHERE {sqlWhereStatement}", conn))
            {

                AddSqlParameters(cmd, parameters);


                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    //var r = Activator.CreateInstance(typeof(T));

                    //while (reader.Read())
                    //{
                    //    for (var i = 0; i < reader.FieldCount; i++)
                    //    {
                    //        var p = rc.Properties.Find(c => c.ColumnName == reader.GetName(i));
                    //        if (p == null)
                    //        {
                    //            //todo log column not found in data
                    //            continue;
                    //        }

                    //        p.Info.SetValue(r, reader[i]);
                    //    }
                    //    rc.IdProperty.Info.SetValue(r, id);
                    //    return (T)r;
                }
            }
            return null;
        }



        public T Get<T>(SqlConnection conn, int id)
        {
            var rc = GetRegisteredType<T>();

            using (SqlCommand cmd = new SqlCommand($"SELECT * from {rc.SqlTableName} WHERE {rc._IdProperty.ColumnName}=@{rc._IdProperty.Name}", conn))
            {
                cmd.Parameters.AddWithValue($"@{rc._IdProperty.Name}", id);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    var r = Activator.CreateInstance(typeof(T));

                    while (reader.Read())
                    {
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            var p = rc.Properties.Find(c => c.ColumnName == reader.GetName(i));
                            if (p == null)
                            {
                                //todo log column not found in data
                                continue;
                            }

                            switch (p.Handler)
                            {
                                case PropertyHandler.AsJson:
                                case PropertyHandler.Default:
                                    SetPropertyValue(p, r, reader[i]);
                                    break;
                                //case PropertyHandler.Ignore:
                                //    break;
                                case PropertyHandler.AsList:
                                    break;
                                case PropertyHandler.Class:
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                        rc._IdProperty.Info.SetValue(r, id);
                        return (T)r;
                    }
                }
            }
            return default(T);
        }
    }
}
