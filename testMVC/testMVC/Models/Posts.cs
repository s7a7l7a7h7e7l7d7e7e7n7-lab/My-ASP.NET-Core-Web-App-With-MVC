using System.ComponentModel.DataAnnotations;

namespace testMVC.Models
{
    public class Posts
    {
        [Key]
        public int postId { get; set; }
        [Required]
        public required string postTitle { get; set; }
        [Required]
        public required string postDescription { get; set; }
        [Required]
        public required string postAuthor { get; set; }
        [Required]
        public DateTime postDate { get; set; }= DateTime.Now;

    }
}
