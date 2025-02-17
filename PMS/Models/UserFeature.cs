using PMS.Common.Data.Enums;

namespace PMS.Models
{
    public class UserFeature : BaseModel
    { 
        public int UserID { get; set; }
        public User user { get; set; }
        public Feature Feature { get; set; }
    }
}
