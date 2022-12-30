namespace Utad_Proj_.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ArticleComment
    {
        public Article Article { get; set; }
        public int ArticleId { get; set; }
        public string Comments { get; set; }
        public int Id { get; set; }
        public DateTime PublishDate { get; set; }
        public int Rating { get; set; }
    }
}