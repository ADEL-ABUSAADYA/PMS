namespace PMS.Features.Common.Users.DTOs;

public record User2FAInfoDTO(bool Is2FAEnabled, string TwoFactorAuthSecretKey);