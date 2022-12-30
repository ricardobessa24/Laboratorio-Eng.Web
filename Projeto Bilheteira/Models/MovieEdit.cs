namespace Utad_Proj_.Models
{
    public class MovieEdit
    {
        public string cast { get; set; }
        public Category Category { get; set; }
        public int CategoryID { get; set; }
        public string description { get; set; }
        public int Id { get; set; }
        public byte[] photo { get; set; }
        public string Title { get; set; }
        public string trailer { get; set; }
        public int year { get; set; }
    }
}