using System;
using System.Linq;
namespace TestASP.Common.Extensions
{
	public static class EnumExtension
	{
		public static Dictionary<TEnum,string> GetValues<TEnum>(this Type @enum) where TEnum: Enum
		{
			if (@enum.IsEnum)
			{
				return Enum.GetValues(@enum).Cast<TEnum>().ToDictionary(e => e, e => e.ToString());
            }
			return null;
        }

        public static List<string> GetStringValues<TEnum>(this Type @enum) where TEnum : Enum
        {
            if (@enum.IsEnum)
            {
                return Enum.GetValues(@enum).Cast<TEnum>().Select(e => e.ToString()).ToList();
            }
            return null;
        }

        public static T ToEnum<T>(this string value, T defaultValue) where T : struct
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return Enum.TryParse(value, true, out T result) ? result : defaultValue;
        }

        public static TEnum ToEnum<TEnum>(this string value) where TEnum: Enum
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value, true);            
        }
    }
}

