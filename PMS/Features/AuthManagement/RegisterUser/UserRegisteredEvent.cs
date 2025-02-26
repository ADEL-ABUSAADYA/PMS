namespace PMS.Features.AuthManagement.RegisterUser;

public record UserRegisteredEvent( string Email, string Name, string ActivationLink, DateTime CreatedAt);
