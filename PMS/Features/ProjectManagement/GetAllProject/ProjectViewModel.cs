using PMS.Models.Enums;

namespace PMS.Features.ProjectManagement.GetAllProject
{
    public class ProjectViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public ProjectStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } 
    
        public string CreatorName { get; set; }
    }
}
