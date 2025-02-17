using FluentValidation;
using PMS.Features.AuthManagement.RegisterUser;
using PMS.Features.AuthManagement.RegisterUser.Commands;

namespace PMS.Features.ProjectManage.AddProject
{
    public class RequestAddProjectModel
    {
        public string Title { get;  set; }
        public string Descrbition { get;  set; }
        
        public DateTime EndDate { get;  set; }
    }

    public class RequestAddProjectModelValidator : AbstractValidator<RequestAddProjectModel>
    {
        public RequestAddProjectModelValidator()
        {
  
        }
    }
}