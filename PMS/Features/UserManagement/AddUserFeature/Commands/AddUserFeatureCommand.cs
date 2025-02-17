using MediatR;
using PMS.Common.BaseHandlers;
using PMS.Common.Data.Enums; 
using PMS.Common.Views;
using PMS.Data.Repositories;
using PMS.Features.Common.Users.Queries;
using PMS.Models;

namespace PMS.Features.UserManagement.AddUserFeature.Commands;

public record AddUserFeatureCommand(string Email, Feature feature) : IRequest<RequestResult<bool>>;

public class AddUserFeatureCommandHandler : BaseRequestHandler<AddUserFeatureCommand, RequestResult<bool>>
{
    private readonly IRepository<UserFeature> _repository;
    public AddUserFeatureCommandHandler(BaseRequestHandlerParameters parameters, IRepository<UserFeature> repository) :
        base(parameters)
    {
        _repository = repository;
    }

    public async override Task<RequestResult<bool>> Handle(AddUserFeatureCommand request, CancellationToken cancellationToken)
    {
        
        var userID = await _mediator.Send(new GetUserIDByEmailQuery(request.Email));
        if (!userID.isSuccess)
            return RequestResult<bool>.Failure(userID.errorCode, userID.message);

        var hasAccess = await _mediator.Send(new HasAccessQuery(userID.data, request.feature));
        if (hasAccess)
            return RequestResult<bool>.Success(true);
        
        await _repository.AddAsync(new UserFeature
            {
                Feature = request.feature,
                UserID = userID.data,
            });
        await _repository.SaveChangesAsync();

        return RequestResult<bool>.Success(true);
    }
}
