namespace Utad_Proj_.Services
{
    using Microsoft.EntityFrameworkCore;
    using System.Text;
    using System.Threading.Tasks;
    using Utad_Proj_.Data;
    using Utad_Proj_.Models;

    public class PurchaseService : IPurchaseService
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IEmailSenderService emailSenderService;
        private readonly PurchaseEmailSenderOptions purchaseEmailSenderOptions;

        public PurchaseService(
            ApplicationDbContext applicationDbContext,
            IEmailSenderService emailSenderService,
            PurchaseEmailSenderOptions purchaseEmailSenderOptions)
        {
            this.applicationDbContext = applicationDbContext;
            this.emailSenderService = emailSenderService;
            this.purchaseEmailSenderOptions = purchaseEmailSenderOptions;
        }

        public async Task CreatePurchaseAsync(CreatePurchaseArgs createPurchaseArgs)
        {
            var applicationUser = await this.applicationDbContext.Users
                .FindAsync(createPurchaseArgs.ApplicationUserId)
                .ConfigureAwait(false);
            var movieSession = await this.applicationDbContext.Sessions
                .Include(x => x.Movie)
                .Include(x => x.Room)
                .FirstOrDefaultAsync(x => x.Id == createPurchaseArgs.MovieSessionId)
                .ConfigureAwait(false);

            var purchase = new Purchase
            {
                ApplicationUser = applicationUser,
                MovieSession = movieSession,
                Date_ = createPurchaseArgs.Date,
                price = createPurchaseArgs.Price
            };

            this.applicationDbContext.Purchases.Add(purchase);
            int affectedRecords = await this.applicationDbContext.SaveChangesAsync().ConfigureAwait(false);

            if (affectedRecords > 0)
            {
                var contentBuilder = new StringBuilder();
                contentBuilder.AppendLine($"Your tickets for movie '{movieSession.Movie.Title}' have been confirmed for session on room '{movieSession.Room.Name}' on the {purchase.Date_}.")
                    .AppendLine()
                    .AppendLine($"You have been charged {purchase.price} USD.");
                string content = contentBuilder.ToString();

                await this.emailSenderService.SendEmailAsync(new SendEmailArgs
                {
                    ReceiverEmail = applicationUser.Email,
                    ReceiverName = applicationUser.UserName,
                    SenderEmail = this.purchaseEmailSenderOptions.SenderEmail,
                    SenderName = this.purchaseEmailSenderOptions.SenderName,
                    Subject = "Purchase tickets confirmation",
                    HtmlContent = content
                }).ConfigureAwait(false);
            }
        }
    }
}