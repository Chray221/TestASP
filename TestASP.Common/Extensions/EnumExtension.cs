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
    }
}

