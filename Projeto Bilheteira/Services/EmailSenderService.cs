namespace Utad_Proj_.Services
{
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.Options;
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using System.Threading.Tasks;
    using Utad_Proj_.Models;

    internal class EmailSenderService : IEmailSenderService
    {
        private readonly SendGridEmailOptions messageSenderOptions;

        public EmailSenderService(SendGridEmailOptions emailSenderOptions)
        {
            this.messageSenderOptions = emailSenderOptions;
        }

        public async Task SendEmailAsync(SendEmailArgs sendEmailArgs)
        {
            var client = new SendGridClient(this.messageSenderOptions.SendGridApiKey);

            var emailMessage = new SendGridMessage
            {
                From = new EmailAddress(sendEmailArgs.SenderEmail, sendEmailArgs.SenderName),
                Subject = sendEmailArgs.Subject,
                PlainTextContent = sendEmailArgs.HtmlContent,
                HtmlContent = sendEmailArgs.HtmlContent,
            };

            emailMessage.AddTo(new EmailAddress(sendEmailArgs.ReceiverEmail, sendEmailArgs.ReceiverName));

            emailMessage.SetClickTracking(false, false);

            var result = await client.SendEmailAsync(emailMessage).ConfigureAwait(false);
        }
    }
}