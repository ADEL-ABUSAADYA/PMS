using System.ComponentModel.DataAnnotations.Schema;

namespace PMS.Models;

public class UserAssignedProject : BaseModel
{
    public int UserID { get; set; }
    public User User { get; set; }
    
    public int ProjectID { get; set; }
    public Project Project { get; set; }
    
    public DateTime AssignDate { get; set; }
    public DateTime EndDate { get; set; }
}