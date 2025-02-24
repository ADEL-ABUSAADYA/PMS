namespace PMS.Features.AuthManagement.SendFrogetPasswordResetEmail;

public record FrogetPasswordInfoDTO(Guid UserID, bool IsEmailConfirmed);