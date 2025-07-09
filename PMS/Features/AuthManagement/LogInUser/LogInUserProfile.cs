using AutoMapper;
using PMS.Features.AuthManagement.LogInUser.Commands;
using PMS.Models;

namespace PMS.Features.AuthManagement.LogInUser;

public class LogInUserProfile : Profile
{
    public LogInUserProfile()
    {
        CreateMap<LogInUserRequestViewModel, LogInUserCommand>().ReverseMap();
    }
}