using AutoMapper;
using PMS.Models;

namespace PMS.Features.ProjectManagement.GetAllProject;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<Project, ProjectDTO>()
            .ForMember(dst => dst.CreatorName, opt => opt.MapFrom(prj => prj.Creator.Name));
    }
}