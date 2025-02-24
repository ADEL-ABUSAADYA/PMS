namespace PMS.Models;

public class UserSprintItem :BaseModel
{
    public Guid UserID { get; set; }
    public User User { get; set; }
    
    public Guid SprintItemID { get; set; }
    public SprintItem SprintItem { get; set; }
}