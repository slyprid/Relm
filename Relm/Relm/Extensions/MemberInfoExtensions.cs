using System;
using System.Reflection;

namespace Relm.Extensions
{
    public static class MemberInfoExtensions
    {
        public static T GetAttribute<T>(this MemberInfo self) where T : Attribute
        {
            var attributes = self.GetCustomAttributes(typeof(T));
            foreach (var attribute in attributes)
            {
                if (attribute.GetType() == typeof(T))
                    return (T)attribute;
            }

            return null;
        }
	}
}