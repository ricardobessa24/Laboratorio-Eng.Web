namespace Utad_Proj_.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Article
    {
        public bool Active { get; set; }
        public ICollection<ArticleComment> ArticlesComments { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
    }
}