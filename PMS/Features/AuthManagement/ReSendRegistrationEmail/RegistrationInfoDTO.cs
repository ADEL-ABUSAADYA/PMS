namespace PMS.Features.AuthManagement.ResendRegistrationEmail;

public class RegistrationInfoDTO
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string ConfirmationToken { get; set; }
    public bool IsEmailConfirmed { get; set; }
}