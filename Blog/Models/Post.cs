using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    /* DBContext */
    public class Post
    {
        public string Id { get; set; }

        [Required]
        public string UserId { get; set; } // Foreign key UserId + User
        public ApplicationUser User { get; set; }

        [Required]
        public string CategoryId { get; set; } // Foreign key CategoryId + Category
        public Category Category { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime? Date { get; set; }

        [Required]
        public string ImageName { get; set; }
        [Required]
        public string ImageType { get; set; }
        [Required]
        public byte[] ImageData { get; set; }
    }
}
