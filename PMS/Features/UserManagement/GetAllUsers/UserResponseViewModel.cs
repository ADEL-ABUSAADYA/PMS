namespace PMS.Features.UserManagement.GetAllUsers;

public record UserResponseViewModel
{
    public List<UserDTO> Users { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
    