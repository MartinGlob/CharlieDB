using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CharlieDb
{
    public partial class Charlie
    {
        public Dictionary<string, DbClass> ClassesType { get; set; }

        public Charlie()
        {
            ClassesType = new Dictionary<string, DbClass>();
        }

        public DbClass Register<T>()
        {
            var dbc = new DbClass { ClassType = typeof(T) };

            dbc.SqlTableName = dbc.ClassType.Name;

            foreach (var p in dbc.ClassType.GetProperties())
            {
                var rp = new DbProperty().RegisterProperty(p);

                dbc.Properties.Add(rp);
            }
            ClassesType.Add(dbc.ClassType.FullName, dbc);

            return dbc;
        }


        //private T MapFromSql<T>(DbClass rc, SqlDataReader reader)
        //{
        //    T t = (T)Activator.CreateInstance(typeof(T));

        //    var ty = typeof(T);

        //    foreach (var p in rc.Properties)
        //    {
        //        var pidx = reader.GetOrdinal(p.ColumnName);

        //        p.Info.SetValue(t, reader[pidx]);

        //    }

        //    return t;
        //}

        private string MapToSql(DbProperty rp)
        {
            var colName = $"{rp.ColumnName}";
            var comment = "";
            var sqlType = "";
            var nullType = "";
            var identType = "";

            switch (rp.Handler)
            {
                case PropertyHandler.Default:
                    var map = DataTypes.DataTypeMap.FirstOrDefault(x => x.CharlieType == rp.ElementType);
                    if (map == null)
                    {
                        throw new Exception($"No C# to SQL mapping found for {rp.ColumnName}:{rp.ElementType}");
                    }

                    sqlType = map.SqlType;
                    if (sqlType.Contains("#"))
                    {
                        sqlType = sqlType.Replace("#", rp.DbSize == 0 ? "MAX" : rp.DbSize.ToString());
                    }

                    nullType = $"{(rp.IsNullable ? "NULL" : "NOT NULL")}";
                    break;
                case PropertyHandler.Class:
                    sqlType = $"NVARCHAR(max)";
                    nullType = "NULL";
                    comment = $" -- JSON Representation for class {rp.ElementTypeName}";
                    break;
                case PropertyHandler.AsJson:
                    sqlType = $"NVARCHAR(max)";
                    nullType = "NULL";
                    comment = $" -- JSON Representation for class {rp.ElementTypeName}";
                    break;
                default:
                    throw new Exception($"MapToSql failed for {rp.ColumnName}. No handler defined");
            }

            if (rp.IsID)
            {
                //todo check prop is int type
                comment += "ID";
                identType = " IDENTITY PRIMARY KEY";
            }

            if (!string.IsNullOrEmpty(comment))
                comment = $"-- {comment}";

            return $"    {colName} {sqlType}{identType} {nullType}, {comment}";
        }

        private DbClass GetRegisteredType<T>()
        {
            var tName = typeof(T).FullName;

            if (!ClassesType.ContainsKey(tName))
            {
                throw new Exception($"CharlieDB: Type {tName} not registered");
            }

            return ClassesType[tName];
        }

        //public T ReadSingle<T>(SqlConnection con, int id)
        //{
        //    var rc = GetRegisteredType<T>();

        //    using (SqlCommand cmd = new SqlCommand($"select * from {rc.SqlTableName}", con))
        //    {

        //        using (SqlDataReader reader = cmd.ExecuteReader())
        //        {
        //            if (reader.Read())
        //            {
        //                var result = MapFromSql<T>(rc, reader);
        //                return result;
        //            }
        //            else
        //            {
        //                return default(T);
        //            }
        //        }
        //    }
        //}

        public string GenerateCreateSqlScript()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var rc in ClassesType.Values)
            {
                sb.AppendLine($"create table {rc.SqlTableName}");
                sb.AppendLine("(");

                if (rc._IdProperty != null)
                {
                    sb.AppendLine(MapToSql(rc._IdProperty));
                }

                foreach (var col in rc.Properties)
                {
                    if (col.Handler != PropertyHandler.Ignore)
                    {
                        sb.AppendLine(MapToSql(col));
                    }
                }
                sb.AppendLine(")");
            }

            return sb.ToString();
        }
    }
}
