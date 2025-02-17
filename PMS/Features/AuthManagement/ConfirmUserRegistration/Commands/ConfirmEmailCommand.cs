using MediatR;
using PMS.Common.BaseHandlers;
using PMS.Common.Views;
using PMS.Data.Repositories;
using PMS.Features.AuthManagement.ConfirmUserRegistration.Queries;
using PMS.Models;

namespace PMS.Features.AuthManagement.ConfirmUserRegistration.Commands;

public record ConfirmEmailCommand(string email, string token) : IRequest<RequestResult<bool>>;

public class ConfirmEmailHandler : BaseRequestHandler<ConfirmEmailCommand, RequestResult<bool>>
{
    private readonly IRepository<User> _repository;
    public ConfirmEmailHandler(BaseRequestHandlerParameters parameters, IRepository<User> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<RequestResult<bool>> Handle(ConfirmEmailCommand request,
        CancellationToken cancellationToken)
    {
        var isRegistered = await _mediator.Send(new IsUserRegisteredQuery(request.email, request.token));

        User user = new User();
        if (isRegistered.isSuccess)
        {
            user.ID = isRegistered.data;
            user.IsEmailConfirmed = true;
            user.ConfirmationToken = null;

            var result = await _repository.SaveIncludeAsync(user, nameof(User.IsEmailConfirmed),
                nameof(User.ConfirmationToken));

            await _repository.SaveChangesAsync();
            return RequestResult<bool>.Success(result);
        }

        return RequestResult<bool>.Failure(isRegistered.errorCode, isRegistered.message);
    }

    
}



