using System.ComponentModel.DataAnnotations.Schema;
using PMS.Models.Enums;

namespace PMS.Models;

public class Project : BaseModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public ProjectStatus Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; } 
    
    public Guid CreatorID { get; set; }
    public User Creator { get; set; }
    
    public ICollection<SprintItem> SprintItems { get; set; }
    public ICollection<UserAssignedProject> UserAssignedProjects { get; set; } = new List<UserAssignedProject>();
}