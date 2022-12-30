namespace Utad_Proj_.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Room
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Lotação")]
        public int Num { get; set; }

        public ICollection<Movie_Session> Sessions { get; set; }
    }
}