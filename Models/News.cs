using System.ComponentModel.DataAnnotations;

namespace ParseRSS.Models
{
    public class News
    {
        public int Id { get; set; }
   
        public string? Title { get; set; }

        public string? Link { get; set; }

        public string? Description { get; set; }

        public DateTimeOffset PubDate { get; set; }

        [Required]
        public string? Publisher { get; set; }

        [Required]
        public string? NewsGroup { get; set; }
    }
}
