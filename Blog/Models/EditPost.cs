using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Models
{
    /* View: Post/EditPost */
    public class EditPost
    {
        public string Id { get; set; }

        public string CategoryId { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }

        [Required(ErrorMessage = "Please enter title of the post")]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter some content of the post")]
        public string Content { get; set; }

        public IFormFile Image { get; set; }

        public List<IFormFile> Files { get; set; }

        public DateTime? Date { get; set; }

        [Required(ErrorMessage = "Please enter short description of the post")]
        [StringLength(800, MinimumLength = 10)]
        public string Description { get; set; }

        public List<string> FileNames { get; set; }
    }
}
