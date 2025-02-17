using PMS.Common.Views;

namespace PMS.Helpers;

public static class EnumHelper
{
    public static string GetDescription(this Enum value)
    {
        return DescriptionAttribute.GetDescription(value, false);
    }

    public static IEnumerable<ItemListViewModel> ToItemList<TEnum>() where TEnum : struct, Enum
    {
        return Enum.GetValues<TEnum>().Select(e => new ItemListViewModel
        {
            ID = Convert.ToInt32(e),
            Name = e.GetDescription()
        }).ToList();
    }
} 