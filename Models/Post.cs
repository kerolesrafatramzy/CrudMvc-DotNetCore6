using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simple_blog.Models;


namespace Simple_blog.Models
{
    public class Post
    {
        public int Id { get; set; } 

        [Required, MaxLength(250)]
        public string Title { get; set; }

        [Required, MaxLength(2500)]
        public string Content { get; set; }

        [Required]
        public byte[]? Image { get; set; }

        public double Rate { get; set; } 

        public string Published_In { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

    }
}
