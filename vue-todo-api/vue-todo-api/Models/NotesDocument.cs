using System;
using System.Collections.Generic;

namespace VueTodoApi.Models
{
    public class NotesDocument
    {
        public string Id { get; set; }
        public string Heading { get; set; }
        public string Undertitle { get; set; }
        public string Description { get; set; }
        public string More { get; set; }
        public string Type { get; set; }
        public string Image { get; set; }
        public List<string> Images { get; set; }
        public List<string> Files { get; set; }
        public List<string> Tags { get; set; }
        public DateTime TimeOfEntry { get; set; }
    }
}
