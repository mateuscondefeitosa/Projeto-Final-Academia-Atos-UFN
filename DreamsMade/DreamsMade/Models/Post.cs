using System.ComponentModel.DataAnnotations;

namespace DreamsMade.Models
{
    public class Post
    {
        [Key]
        public int id { get; set; }


        [StringLength(20)]
        public string? title { get; set; }


        [Required]
        public string? image { get; set; }


        public string? text { get; set; }


        public virtual User user { get; set; }

    }
}
