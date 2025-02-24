using System.Text;
using System.Text.Json;
using DotNetCore.CAP;
using MailKit.Net.Smtp;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MimeKit;
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
    public RegisterUserCommandHandler(BaseRequestHandlerParameters parameters, IRepository<User> repository, ICapPublisher capPublisher) : base(parameters)
    {
        _repository = repository;
        _capPublisher = capPublisher;
    }

    public async override Task<RequestResult<bool>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
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
        
       
        var userID = await _repository.AddAsync(user);
        await _repository.SaveChangesAsync();
        
        if (userID == Guid.Empty)
        return RequestResult<bool>.Failure(ErrorCode.UnKnownError);

        var message = new UserRegisteredEvent(user.ID, user.Email, user.Name, $"{user.ConfirmationToken}", DateTime.UtcNow);
        var messageJson = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(messageJson);
        await _capPublisher.PublishAsync("user.registered", body );

        return RequestResult<bool>.Success(true);
        
    }
    
    private async Task<RequestResult<bool>> SendConfirmationEmail(string email, string name, string confirmationLink)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("adel", "upskillingfinalproject@gmail.com"));
        message.To.Add(new MailboxAddress(name, email));
        message.Subject = "UpSkilling Final Project";

        // Create multipart content for both plain text and HTML
        var bodyBuilder = new BodyBuilder
        {
            TextBody = $"Please confirm your registration by token [{confirmationLink}]",
            HtmlBody = $"Please confirm your registration by token [{confirmationLink}]"
        };

        message.Body = bodyBuilder.ToMessageBody();

        try
        {
            using (var client = new SmtpClient())
            {
                // Set the timeout for connection and authentication
                client.Timeout = 10000;  // Timeout after 10 seconds
            
                // Connect using StartTLS for security
                await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

                // Authenticate with the provided credentials
                await client.AuthenticateAsync("upskillingfinalproject@gmail.com", "vxfdhstkqegcfnei");

                // Send the email
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            return RequestResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            // Log the detailed exception message for debugging
            Console.WriteLine($"Error sending email: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");

            // Return failure with error details
            return RequestResult<bool>.Failure(ErrorCode.UnKnownError, ex.Message);
        }
    }
}


