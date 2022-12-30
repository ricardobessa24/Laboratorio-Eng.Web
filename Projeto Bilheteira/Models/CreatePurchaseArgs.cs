namespace Utad_Proj_.Models
{
    using System;

    public struct CreatePurchaseArgs
    {
        public string ApplicationUserId { get; set; }

        public DateTime Date { get; set; }

        public int MovieSessionId { get; set; }

        public float Price { get; set; }
    }
}