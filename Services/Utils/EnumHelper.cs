

using Microsoft.AspNetCore.Mvc.Rendering;

namespace Services.Utils
{
    public static class EnumHelper
    {
        public static List<SelectListItem> ToSelectList<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum))
                       .Cast<TEnum>()
                       .Select(e => new SelectListItem
                       {
                           Value = Convert.ToInt32(e).ToString(),
                           Text = e.ToString()
                       }).ToList();
        }
        public static TEnum? ToEnum<TEnum>(this int value) where TEnum : struct, Enum
        {
            if (Enum.IsDefined(typeof(TEnum), value))
                return (TEnum)(object)value;

            return null;
        }
    }
}
