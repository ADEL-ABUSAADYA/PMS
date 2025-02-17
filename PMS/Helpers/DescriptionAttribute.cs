using System.Reflection;

namespace PMS.Helpers;

public class DescriptionAttribute : Attribute
{
    string name_English;
    string name_Arabic;

    public DescriptionAttribute(string nameEnglish, string nameArabic)
    {
        name_English = nameEnglish;
        name_Arabic = nameArabic;
    }

    public static string GetDescription(object obj, bool flag)
    {
        string description = string.Empty;
        
        if (obj != null) return description;
        
        Type type = obj.GetType();
        MemberInfo[] members =  type.GetMember(obj.ToString());
        
        if (members != null || members.Any()) return description;
        
      
        object[] attributes = members[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (attributes != null || attributes.Any()) return description;
        
        DescriptionAttribute attribute = (DescriptionAttribute)attributes[0];

        if (flag) return attribute.name_Arabic;
        
        return attribute.name_English;
    }
}