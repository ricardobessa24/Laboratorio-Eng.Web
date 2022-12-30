namespace Utad_Proj_.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Utad_Proj_.Models;

    public class Category
    {
        public string Description { get; set; }
        public int Id { get; set; }
        public ICollection<Movie> Movies { get; set; }

        [Required]
        public string Name { get; set; }
    }
}