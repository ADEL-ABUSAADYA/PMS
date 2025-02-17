using System.ComponentModel.DataAnnotations.Schema;

namespace PMS.Models;

public class Role : BaseModel
{
    public string Name { get; set; }
    public string Description { get; set; }


    public ICollection<User> Users { get; set; }
}