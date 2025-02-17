using PMS.Common.Views;

namespace PMS.Common;

public class UserInfoProvider
{
    public UserInfo UserInfo { get; set; }

    public UserInfoProvider()
    {
        UserInfo = new UserInfo(); // Ensure it's initialized by default
    }
}