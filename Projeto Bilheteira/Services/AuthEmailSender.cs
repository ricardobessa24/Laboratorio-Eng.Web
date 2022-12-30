namespace Utad_Proj_.Services
{
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.Options;
    using System;
    using System.Threading.Tasks;
    using Utad_Proj_.Models;

    public class AuthEmailSender : IEmailSender
    {
        private readonly AuthEmailSenderOptions authEmailSenderOptions;
        private readonly IEmailSenderService emailSenderService;

        public AuthEmailSender(
            IEmailSenderService emailSenderService,
            AuthEmailSenderOptions authEmailSenderOptions)
        {
            if (authEmailSenderOptions is null)
            {
                throw new ArgumentNullException(nameof(authEmailSenderOptions));
            }

            this.emailSenderService = emailSenderService ?? throw new System.ArgumentNullException(nameof(emailSenderService));
            this.authEmailSenderOptions = authEmailSenderOptions;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return this.emailSenderService.SendEmailAsync(new Models.SendEmailArgs
            {
                HtmlContent = htmlMessage,
                ReceiverEmail = email,
                Subject = subject,
                ReceiverName = email,
                SenderEmail = this.authEmailSenderOptions.SenderEmail,
                SenderName = this.authEmailSenderOptions.SenderName
            });
        }
    }
}