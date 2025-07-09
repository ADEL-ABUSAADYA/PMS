

namespace PMS.Features.ProjectManagement.GetAllProject
{
    public class ProjectResponseViewModel
    {
        public List<ProjectViewModel> Projects { get; set; }

        public int totalNumber { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }


    }
}