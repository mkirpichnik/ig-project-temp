using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WeddingForward.ApplicationServices.Extensions
{
    internal static class EnumExtensions
    {
        public static string ToEnumString<T>(T type)
        {
            Type enumType = typeof(T);
            
            string name = Enum.GetName(enumType, type);

            EnumMemberAttribute enumMemberAttribute =
                ((EnumMemberAttribute[]) enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true))
                .Single();
            
            return enumMemberAttribute.Value;
        }

        public static T ToEnum<T>(string str)
        {
            Type enumType = typeof(T);

            foreach (var name in Enum.GetNames(enumType))
            {
                EnumMemberAttribute enumMemberAttribute =
                    ((EnumMemberAttribute[]) enumType.GetField(name)
                        .GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
            
                if (enumMemberAttribute.Value == str)
                {
                    return (T)Enum.Parse(enumType, name);
                }
            }
            //throw exception or whatever handling you want or
            return default(T);
        }
    }
}
