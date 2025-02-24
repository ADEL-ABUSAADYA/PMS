namespace PMS.Features.AuthManagement.RegisterUser;

public record UserRegisteredEvent(Guid UserID, string Email, string Name, string ActivationLink, DateTime CreatedAt);
