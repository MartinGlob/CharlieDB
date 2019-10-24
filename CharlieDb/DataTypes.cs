using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieDb
{
    public static class DataTypes
    {
        public class TypeMap
        {
            public string SysName { get; set; }
            public ElementTypes CharlieType { get; set; }
            public string SqlType { get; set; }

            public TypeMap(string SystemName, ElementTypes charlieType, string sqlType)
            {
                SysName = SystemName;
                CharlieType = charlieType;
                SqlType = sqlType;
            }
        }

        public static List<TypeMap> DataTypeMap = new List<TypeMap>()
        {
            new TypeMap("System.String",ElementTypes.String,"NVARCHAR(#)"),
            new TypeMap("System.Char",ElementTypes.Char,"NVARCHAR(1)"),
            new TypeMap("System.Boolean",ElementTypes.Boolean,"BIT"),
            new TypeMap("System.Byte",ElementTypes.Byte,"TINYINT"),
            new TypeMap("System.SByte",ElementTypes.SByte,"SMALLINT"),
            new TypeMap("System.Int16",ElementTypes.Int16,"SMALLINT"),
            new TypeMap("System.UInt16",ElementTypes.UInt16,"INT"),
            new TypeMap("System.Int32",ElementTypes.Int32,"INT"),
            new TypeMap("System.UInt32",ElementTypes.UInt32,"BIGINT"),
            new TypeMap("System.Int64",ElementTypes.Int64,"BIGINT"),
            new TypeMap("System.UInt64", ElementTypes.UInt64,"DECIMAL"),
            new TypeMap("System.Single",ElementTypes.Single,"REAL"),
            new TypeMap("System.Double",ElementTypes.Double,"FLOAT"),
            new TypeMap("System.Decimal",ElementTypes.Decimal,"DECIMAL"),
            new TypeMap("System.Guid"   ,ElementTypes.Guid,"UNIQUEIDENTIFIER"),
            new TypeMap("System.DateTime"   ,ElementTypes.DateTime,"DATETIME2"),
            new TypeMap("System.Enum"   ,ElementTypes.Enum,"NVARCHAR(100)"),
        };

        private static readonly Dictionary<string, ElementTypes> _dataTypeMap = new Dictionary<string, ElementTypes>()
        {
            {"System.Boolean"   , ElementTypes.Boolean},
            {"System.Byte",   ElementTypes.Byte   },
            {"System.SByte",  ElementTypes.SByte  },

            {"System.Int16",  ElementTypes.Int16  },
            {"System.UInt16", ElementTypes.UInt16 },
            {"System.Int32",  ElementTypes.Int32    },
            {"System.UInt32", ElementTypes.UInt32   },
            {"System.Int64",  ElementTypes.Int64   },
            {"System.UInt64", ElementTypes.UInt64  },

            {"System.Single", ElementTypes.Single },
            {"System.Float",  ElementTypes.Single },
            {"System.Double", ElementTypes.Double },
            {"System.Decimal",ElementTypes.Decimal },

            {"System.DateTime", ElementTypes.DateTime},

            {"System.Char"    , ElementTypes.Char},
            {"System.String"  ,ElementTypes.String },
            {"System.Guid"    ,ElementTypes.Guid },

        };

        public static ElementTypes DecodeType(Type type)
        {
            if (_dataTypeMap.ContainsKey(type.FullName))
            {
                return _dataTypeMap[type.FullName];
            }
            else
            {
                throw new Exception($"Charlie doesn't know data type {type.FullName}");
            }
        }

    }

    public enum SqlType
    {
        Bit,
        TinyInt,
        SmallInt,
        Int,
        BigInt,
        Char,
        VarChar,
        NVarChar,
        NChar,
        Text,
        Decimal,
        Real,
        Float,
        UniqueIdentifier
    }
}
