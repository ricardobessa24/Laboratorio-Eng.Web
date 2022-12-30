namespace Utad_Proj_.Models
{
    public class SendEmailArgs
    {
        public string HtmlContent { get; set; }
        public string ReceiverEmail { get; set; }
        public string ReceiverName { get; set; }
        public string SenderEmail { get; set; }

        public string SenderName { get; set; }
        public string Subject { get; set; }
    }
}