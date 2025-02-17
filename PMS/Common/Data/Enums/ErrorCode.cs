
namespace PMS.Common.Data.Enums;

public enum ErrorCode
{
    None = 0,
    UnKnownError,
    FieldIsEmpty,
    InvalidInput,
    ItemAlreadyExists,
    UserNotFound,
    UserAlreadyExist,
    EmailNotSent,
    UserAlreadyRegistered,
    Uasr2FAIsNotEnabled,
    Invalid2FA,
    AccountNotVerified,
    PasswordTokenNotMatch,
    NoUsersFound,
    UserIsDeActivated , 


    ProjectAlreadyExists=24,
    ProjectNotFound,
    NoProjectAdd,
    NoSprintItems
}
