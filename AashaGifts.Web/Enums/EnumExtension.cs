using System.ComponentModel;

namespace AashaGifts.Web.Enums
{
    public static class EnumExtension
    {
        public static string GetEnumDescription(this System.Enum value)
        {
            DescriptionAttribute[] array = (DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);
            if (array != null && array.Length != 0)
            {
                return array[0].Description;
            }

            return value.ToString();
        }
    }
}
