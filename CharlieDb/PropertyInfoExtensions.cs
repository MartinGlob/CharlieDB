using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CharlieDb
{
    public static class PropertyInfoExtensions
    {


        public static bool IsGenericList(this PropertyInfo o)
        {
            var oType = o.PropertyType;
            return (oType.IsGenericType && (oType.GetGenericTypeDefinition() == typeof(List<>)));
        }

        public static bool HasAttribute<T>(this PropertyInfo pi)
        {
            return (Attribute.IsDefined(pi, typeof(T)));
        }
    }

    public static class TypeExtensions
    {
        public static bool IsNullable(this Type type)
        {
            return (Nullable.GetUnderlyingType(type) != null);
        }

        public static bool HasAttribute<T>(this Type t)
        {
            return (Attribute.IsDefined(t, typeof(T)));
        }

        public static T GetAttribute<T>(this Type t) where T : class
        {
            var ca = t.GetCustomAttributes(typeof(T)).FirstOrDefault();
            return ca as T;
        }

        //var customAttributes = (MyCustomAttribute[])typeof(Foo).GetCustomAttributes(typeof(MyCustomAttribute), true);
        //    if (customAttributes.Length > 0)
        //{
        //    var myAttribute = customAttributes[0];
        //    string value = myAttribute.SomeProperty;
        //    // TODO: Do something with the value
        //}
}

}
