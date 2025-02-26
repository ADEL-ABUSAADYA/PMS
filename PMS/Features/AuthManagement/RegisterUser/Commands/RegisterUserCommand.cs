using System.Text.Json;
using DotNetCore.CAP;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PMS.Common.BaseHandlers;
using PMS.Common.Data.Enums;
using PMS.Common.Views;
using PMS.Data.Repositories;
using PMS.Features.AuthManagement.RegisterUser.Queries;
using PMS.Models;

namespace PMS.Features.AuthManagement.RegisterUser.Commands;

public record RegisterUserCommand(string email, string password, string name, string phoneNo, string country) : IRequest<RequestResult<bool>>;

public class RegisterUserCommandHandler : BaseRequestHandler<RegisterUserCommand, RequestResult<bool>>
{
    private readonly IRepository<User> _repository;
    private readonly ICapPublisher _capPublisher;

    public RegisterUserCommandHandler(BaseRequestHandlerParameters parameters, IRepository<User> repository,
        ICapPublisher capPublisher) : base(parameters)
    {
        _repository = repository;
        _capPublisher = capPublisher;
    }

    public async override Task<RequestResult<bool>> Handle(RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        var reponse = await _mediator.Send(new IsUserExistQuery(request.email));
        if (reponse.isSuccess)
            return RequestResult<bool>.Failure(ErrorCode.UserAlreadyExist);

        PasswordHasher<string> passwordHasher = new PasswordHasher<string>();
        var password = passwordHasher.HashPassword(null, request.password);

        var user = new User
        {
            Email = request.email,
            Password = password,
            Name = request.name,
            PhoneNo = request.phoneNo,
            Country = request.country,
            RoleID = new Guid("80146a4c-2dbe-4eb7-b4dd-ba1d3e8eeb93"),
            IsActive = true,
            ConfirmationToken = Guid.NewGuid().ToString()
        };


        var userID = await _repository.AddAsync(user, _cancellationToken);
        await _repository.SaveChangesAsync(_cancellationToken);

        if (userID == Guid.Empty)
            return RequestResult<bool>.Failure(ErrorCode.UnKnownError);

        var message = new UserRegisteredEvent(user.Email, user.Name, $"{user.ConfirmationToken}", DateTime.UtcNow);
        var messageJson = JsonSerializer.Serialize(message);
        await _capPublisher.PublishAsync("user.registered", messageJson);

        
        return RequestResult<bool>.Success(true);
    }
}