using System;

namespace Logica.Utils
{
    public static class EnumExtensions
    {
        public static TEnum Parse<TEnum>(string value) where TEnum : struct
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }
    }
}
