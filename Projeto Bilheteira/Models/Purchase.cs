namespace Utad_Proj_.Models
{
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.ComponentModel.DataAnnotations;
    using Utad_Proj_.Models;

    public class Purchase
    {
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserID { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime Date_ { get; set; }

        public int Id { get; set; }
        public string Movie_SessionID { get; set; }
        public Movie_Session MovieSession { get; set; }

        [Required]
        [Display(Name = "Preço")]
        public float price { get; set; }
    }
}