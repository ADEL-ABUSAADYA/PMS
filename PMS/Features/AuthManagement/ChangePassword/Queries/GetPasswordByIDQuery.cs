using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Common.BaseHandlers;
using PMS.Common.Data.Enums;
using PMS.Common.Views;
using PMS.Data.Repositories;
using PMS.Features.Common.Users.DTOs;
using PMS.Features.AuthManagement.LogInUser;
using PMS.Models;

namespace PMS.Features.AuthManagement.ChangePassword.Queries;

public record GetPasswordByIDQuery () : IRequest<RequestResult<string>>;

public class GetPasswordByIDQueryHandler : BaseRequestHandler<GetPasswordByIDQuery, RequestResult<string>>
{
    private readonly IRepository<User> _repository;
    public GetPasswordByIDQueryHandler(BaseRequestHandlerParameters parameters, IRepository<User> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<RequestResult<string>> Handle(GetPasswordByIDQuery request, CancellationToken cancellationToken)
    {
        var password = await _repository.Get(u => u.ID == _userInfo.ID).Select(u => u.Password).FirstOrDefaultAsync();
        
        if (string.IsNullOrWhiteSpace(password))
            return RequestResult<string>.Failure(ErrorCode.PasswordTokenNotMatch, "Password Token Not Match");

        return RequestResult<string>.Success(password);


    }
}

