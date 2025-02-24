using System.Text.Json;
using DotNetCore.CAP;
using MailKit.Net.Smtp;
using MimeKit;
using PMS.Common.Data.Enums;
using PMS.Common.Views;
using PMS.Models;

namespace PMS.Features.AuthManagement.RegisterUser.Consumers;

public class UserRegisteredEventConsumer : ICapSubscribe
{
    [CapSubscribe("user.registered")]
    public async Task HandleUserRegisteredAsync(string messageJson)
    {
        try
        {
            // Deserialize the incoming JSON message
            var userEvent = JsonSerializer.Deserialize<UserRegisteredEvent>(messageJson);
            if (userEvent == null)
            {
                Console.WriteLine("Failed to deserialize UserRegisteredEvent.");
                return;
            }

            // Generate the confirmation link
            var confirmationLink = $"https://yourapp.com/confirm?token={userEvent.ActivationLink}";

            // Send confirmation email
            var result = await SendConfirmationEmail(userEvent.Email, userEvent.Name, confirmationLink);

            if (result.isSuccess)
            {
                Console.WriteLine($"Confirmation email sent to {userEvent.Email}");
            }
            else
            {
                Console.WriteLine($"Failed to send confirmation email: {result.message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing user.registered event: {ex.Message}");
        }
    }

    private async Task<RequestResult<bool>> SendConfirmationEmail(string email, string name, string confirmationLink)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("UpSkilling Project", "upskillingfinalproject@gmail.com"));
        message.To.Add(new MailboxAddress(name, email));
        message.Subject = "Confirm Your Registration";

        var bodyBuilder = new BodyBuilder
        {
            TextBody = $"Please confirm your registration using this token: [{confirmationLink}]",
            HtmlBody = $"<p>Please confirm your registration using this token: <a href='{confirmationLink}'>{confirmationLink}</a></p>"
        };

        message.Body = bodyBuilder.ToMessageBody();

        try
        {
            using (var client = new SmtpClient())
            {
                client.Timeout = 10000;  
                await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("upskillingfinalproject@gmail.com", "vxfdhstkqegcfnei");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            return RequestResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
            return RequestResult<bool>.Failure(ErrorCode.UnKnownError, ex.Message);
        }
    }
}
