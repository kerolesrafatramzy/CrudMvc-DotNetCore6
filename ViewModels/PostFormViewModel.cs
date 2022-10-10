using Simple_blog.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simple_blog.ViewModels
{
    public class PostFormViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(250)]
        public string Title { get; set; }

        [Required, StringLength(2500)]
        public string Content { get; set; }

     
        [Display(Name = "Image")]
        public byte[]? Image { get; set; }

        [Range(1,10)]
        public double Rate { get; set; }

        [Display(Name = "Published In")]
        public string Published_In { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public IEnumerable<Category>? Categories { get; set; }
    }
}
