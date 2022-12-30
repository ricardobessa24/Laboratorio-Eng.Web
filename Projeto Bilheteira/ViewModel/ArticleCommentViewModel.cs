namespace Utad_Proj_.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Utad_Proj_.Models;

    public class ArticleCommentViewModel
    {
        public int ArticlesId { get; set; }
        public string Comment { get; set; }
        public List<ArticleComment> ListOfComments { get; set; }
        public int Rating { get; set; }
        public string Title { get; set; }
    }
}