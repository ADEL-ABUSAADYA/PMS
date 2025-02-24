using System.ComponentModel.DataAnnotations.Schema;

namespace PMS.Models;
public class BaseModel
{
    public Guid ID { get; set; }
    public bool Deleted { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
