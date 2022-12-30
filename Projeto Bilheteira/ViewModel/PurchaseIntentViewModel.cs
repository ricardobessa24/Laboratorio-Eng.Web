namespace Utad_Proj_.ViewModel
{
    using System;
    using System.ComponentModel;

    public class PurchaseIntentViewModel
    {
        public string ApplicationUserId { get; set; }

        [DisplayName("Date")]
        public DateTime Date { get; set; }

        public int MovieSessionId { get; set; }

        [DisplayName("Movie Title")]
        public string MovieTitle { get; set; }

        [DisplayName("Number of Tickets")]
        public int NumberOfTickets { get; set; }

        [DisplayName("Price")]
        public float Price { get; set; }

        [DisplayName("Room")]
        public string RoomName { get; set; }
    }
}