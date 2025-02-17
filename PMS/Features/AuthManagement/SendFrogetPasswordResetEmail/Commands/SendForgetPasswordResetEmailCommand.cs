using MediatR;
using MailKit.Net.Smtp;
using MimeKit;
using PMS.Common.BaseHandlers;
using PMS.Common.Data.Enums;
using PMS.Common.Views;
using PMS.Data.Repositories;
using PMS.Features.AuthManagement.ReSendRegistrationEmail.Queries;
using PMS.Features.AuthManagement.SendFrogetPasswordResetEmail.Queries;
using PMS.Models;


namespace PMS.Features.AuthManagement.SendFrogetPasswordResetEmail.Commands;

public record SendForgetPasswordResetEmailCommand(string email) : IRequest<RequestResult<bool>>;

public class SendForgetPasswordResetEmailCommandHandler : BaseRequestHandler<SendForgetPasswordResetEmailCommand, RequestResult<bool>>
{
    private readonly IRepository<User> _repository;
    public SendForgetPasswordResetEmailCommandHandler(BaseRequestHandlerParameters parameters, IRepository<User> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<RequestResult<bool>> Handle(SendForgetPasswordResetEmailCommand request,
        CancellationToken cancellationToken)
    {
        var passwordResetData = await _mediator.Send(new GetForgetPasswordInfoQuery(request.email));

        if (!passwordResetData.isSuccess)
            return RequestResult<bool>.Failure(passwordResetData.errorCode, passwordResetData.message);
        
        var passwordResetCode =Guid.NewGuid().ToString().Substring(0, 6);

        var user = new User
        {
            ID = passwordResetData.data.UserID,
            ResetPasswordToken = passwordResetCode
        };

        await _repository.SaveIncludeAsync(user, nameof(User.ResetPasswordToken)); 
             
        await _repository.SaveChangesAsync();

        var confirmationToken = $"{user.ResetPasswordToken}";
        
        var emailSent = await SendConfirmationEmail(request.email, "UpSkilling Student", confirmationToken);
        if (!emailSent.isSuccess)
            return RequestResult<bool>.Failure(ErrorCode.EmailNotSent);

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

