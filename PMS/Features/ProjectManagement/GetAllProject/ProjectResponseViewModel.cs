

namespace PMS.Features.ProjectManagement.GetAllProject
{
    public class ProjectResponseViewModel
    {
        public List<ProjectDTO> Projects { get; set; }

        public int totalNumber { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }


    }
}