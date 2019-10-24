using System;
using System.Collections.Generic;
using System.Linq;

namespace CharlieDb
{
    public class DbClass
    {
        public Type ClassType { get; set; }
        public string SqlTableName { get; set; }
        public List<DbProperty> Properties { get; set; }
        public DbProperty _IdProperty { get; set; }

        public string SqlInsert { get; set; }
        public string SqlUpdate { get; set; }

        public DbClass()
        {
            Properties = new List<DbProperty>();
        }

        //public static DbClass Register<T>(string idProp = null)
        //{
        //    var dbc = new DbClass { ClassType = typeof(T) };

        //    dbc.SqlTableName = dbc.ClassType.Name;

        //    foreach (var p in dbc.ClassType.GetProperties())
        //    {
        //        var rp = new DbProperty().RegisterProperty(p);

        //        if (rp.Name == idProp)
        //        {
        //            rp.IsID = true;
        //            dbc.IdProperty = rp;
        //            continue;
        //        }

        //        dbc.Properties.Add(rp);
        //    }

        //    return dbc;
        //}

        public DbClass TableName(string name)
        {
            this.SqlTableName = name;
            return this;
        }

        public DbClass Ignore(string propertyName)
        {
            var idx = Properties.FindIndex(p => p.Name == propertyName);
            
            if (idx == -1)
                throw new ArgumentException($"Property {propertyName} not found in class {ClassType.Name}");

            Properties.RemoveAt(idx);

            return this;
        }

        public DbClass IdProperty(string propertyName)
        {
            var idx = Properties.FindIndex(p => p.Name == propertyName);

            if (idx == -1)
                throw new ArgumentException($"Property {propertyName} not found in class {ClassType.Name}");
            this._IdProperty = Properties[idx];

            return this;
        }

        public DbClass AddParentId()
        {
            return this;
        }

        public DbClass Column()
        {
                //todo check which types this is valid for
                //dp.DbSize = dbc.Size;
                //dp.ColumnName = dbc.Name ?? dp.Name;
                return this;
        }

    }
}