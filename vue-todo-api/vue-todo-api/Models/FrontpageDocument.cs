namespace VueTodoApi.Models
{
    public class FrontpageDocument
    {
        public string Id { get; set; }
        public string Heading { get; set; }
        public string Undertitle { get; set; }
        public string Author { get; set; }
        public Sidebar Sidebar { get; set; }
    }

    public class Sidebar
    {
        public string Image { get; set; }
        public string Heading { get; set; }
        public string Description { get; set; }
    }
}
