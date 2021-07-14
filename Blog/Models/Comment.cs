using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    /* DBContext */
    public class Comment
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public string PostId { get; set; }

        public string UserId { get; set; } // Foreign key UserId + User
        public ApplicationUser User { get; set; }
    }
}
