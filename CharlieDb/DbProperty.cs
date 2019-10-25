using System;
using System.Reflection;

namespace CharlieDb
{
    public class DbProperty
    {
        public bool IsID;
        public string Name { get; set; }
        public PropertyHandler Handler { get; set; }
        public PropertyType PropertyType { get; set; }
        public Type T { get; set; }
        public bool IsNullable { get; set; }
        public ElementTypes ElementType { get; set; }
        public string ElementTypeName { get; set; }
        public string ColumnName { get; set; }
        public PropertyInfo Info { get; set; }
        public int DbSize { get; set; }

        public DbProperty RegisterProperty(PropertyInfo pi)
        {
            var dp = new DbProperty();
            dp.Name = dp.ColumnName = pi.Name;
            dp.Info = pi;
            dp.T = pi.PropertyType;

            if (pi.PropertyType.IsArray)
            {
                dp.PropertyType = PropertyType.IsArray;
                DecodeElementType(pi.PropertyType.GetElementType(), dp);
            }
            else if (pi.IsGenericList())
            {
                dp.PropertyType = PropertyType.IsList;
                DecodeElementType(pi.PropertyType.GenericTypeArguments[0], dp);
                dp.T = pi.PropertyType;
                dp.Handler = PropertyHandler.AsJson;
            }
            else
            {
                dp.PropertyType = PropertyType.IsBasic;
                DecodeElementType(pi.PropertyType, dp);
            }

            //todo check if underlying type full fill this
            if (dp.ElementType == ElementTypes.Class)
            {
                dp.Handler = PropertyHandler.AsJson;
                return dp;
            }

            return dp;
        }

        private void DecodeElementType(Type type, DbProperty dp)
        {
            //dp.T = type;

            if (type.IsNullable())
            {
                dp.IsNullable = true;
                dp.ElementType = DataTypes.DecodeType(Nullable.GetUnderlyingType(type));
            }
            else if (type.IsEnum)
            {
                dp.ElementType = ElementTypes.Enum;
                dp.ElementTypeName = type.FullName;
            }
            else if (type.IsClass && type.FullName != "System.String")
            {
                dp.ElementType = ElementTypes.Class;
                dp.ElementTypeName = type.FullName;
            }
            else
            {
                dp.ElementType = DataTypes.DecodeType(type);
            }
        }

    }
}