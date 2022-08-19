using System;
using System.Reflection;

namespace Relm.Extensions
{
    public static class ReflectionExtensions
    {
        public static FieldInfo GetFieldInfo(this Type type, string fieldName)
        {
            FieldInfo fieldInfo;
            do
            {
                fieldInfo = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                type = type.BaseType;
            } while (fieldInfo == null && type != null);

            return fieldInfo;
        }

        public static MethodInfo GetMethodInfo(this Type type, string methodName, Type[] parameters = null)
        {
            return parameters == null 
                ? type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public) 
                : type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, Type.DefaultBinder, parameters, null);
        }
    }
}