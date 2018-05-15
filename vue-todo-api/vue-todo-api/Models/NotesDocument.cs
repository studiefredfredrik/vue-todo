using System;

namespace VueTodoApi.Models
{
    public class NotesDocument
    {
        public string Id { get; set; }
        public string Heading { get; set; }
        public string Body { get; set; }
        public string Footer { get; set; }
        public string Type { get; set; }
        public DateTime TimeOfEntry { get; set; }
    }
}
