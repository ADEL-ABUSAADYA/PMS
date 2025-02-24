using System.ComponentModel.DataAnnotations.Schema;

namespace PMS.Models;

public class User : BaseModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string PhoneNo { get; set; }
    public string Country { get; set; }
    public bool IsActive { get; set; } = true;
    
    public bool TwoFactorAuthEnabled { get; set; }
    public string? TwoFactorAuthsecretKey { get; set; }

    public bool IsEmailConfirmed { get; set; } = false;
    public string? ConfirmationToken { get; set; }
    
    public string? ResetPasswordToken { get; set; }
    
    public Guid RoleID { get; set; }
    public Role Role { get; set; }
    
    public ICollection<UserFeature> UserFeatures { get; set; }
    public ICollection<UserSprintItem> UserSprintItems { get; set; }
    public ICollection<Project> CreatedProjects { get; set; }
    public ICollection<UserAssignedProject> UserAssignedProjects { get; set; } = new List<UserAssignedProject>();
    
    public User()
    {
        RoleID = Guid.Empty; // Set this manually during user creation
    }
}