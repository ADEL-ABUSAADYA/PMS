namespace PMS.Features.UserManagement.GetAllUsers;

public record UserDTO
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNo { get; set; }
    public bool IsActive { get; set; }
}
