

namespace PMS.Common.Data.Enums
{
    public enum Feature
    {
        // user 0 - 100
        RegisterUser = 0,
        GetUserByID = 1,
        GetUserByEmail = 2,
        GetUserByName = 3,
        UpdateUserByID = 4,
        UpdateUserByEmail = 5,
        UpdateUserByName = 6,
        DeleteUserByID = 7,
        DeleteUserByEmail = 8,
        DeleteUserByName = 9,
        ActivateUser2FA = 10,
        AddUser2FA = 11,
        UpdateUser2FA = 12,
        DeleteUser2FA = 13,
        AddUserFeature = 14,
        UpdateUserFeature = 15,
        DeleteUserFeature = 16,
        GetUser2FALink = 17,
        
        
        // Project 100 - 200
        GetAllProjects = 101,
        GetProjectByID = 102,
        GetProjectByName = 103,
        AddProject = 104,
        UpdateProject = 105,
        DeleteProject = 106,

        
    }
}
