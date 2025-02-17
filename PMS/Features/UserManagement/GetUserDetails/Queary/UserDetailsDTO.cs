using System.Globalization;

namespace PMS.Features.UserManagement.GetUserDetailes.Queary
{
    public class UserDetailsDTO
    {
        public string name {  get; set; } 

        public string email { get; set; }

        public string PhoneNo { get; set; }

        public bool IsActive { get; set; }
        
        public DateTime CreatedTime { get; set; }

    }
}