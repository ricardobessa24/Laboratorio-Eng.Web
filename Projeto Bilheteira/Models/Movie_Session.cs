namespace Utad_Proj_.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Utad_Proj_.Models;

    public class Movie_Session
    {
        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.DateTime)]
        public DateTime Date_ { get; set; }

        public int Id { get; set; }
        public Movie Movie { get; set; }
        public int MovieID { get; set; }
        public Room Room { get; set; }
        public int RoomID { get; set; }
    }
}