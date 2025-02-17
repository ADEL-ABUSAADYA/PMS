using System.ComponentModel.DataAnnotations.Schema;

namespace PMS.Models;
public class BaseModel
{
    public int ID { get; set; }
    public bool Deleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
