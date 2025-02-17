namespace PMS.Models;

public class UserSprintItem :BaseModel
{
    public int UserID { get; set; }
    public User User { get; set; }
    
    public int SprintItemID { get; set; }
    public SprintItem SprintItem { get; set; }
}