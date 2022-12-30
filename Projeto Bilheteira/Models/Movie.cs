namespace Utad_Proj_.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Utad_Proj_.Models;

    public class Movie
    {
        [Required]
        [Display(Name = "Cast")]
        public string cast { get; set; }

        public Category Category { get; set; }
        public int CategoryID { get; set; }

        [Required]
        [Display(Name = "Synopsis")]
        public string description { get; set; }

        public int Id { get; set; }

        [Display(Name = "Picture")]
        public byte[] photo { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Trailer")]
        public string trailer { get; set; }

        [Required]
        [Display(Name = "Year")]
        public int year { get; set; }
    }
}