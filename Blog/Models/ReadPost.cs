using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    /* View: Home/ReadPost */
    public class ReadPost
    {
        public string PostId { get; set; }

        public string PostTitle { get; set; }

        public string PostContent { get; set; }

        public IEnumerable<Comment> Comments { get; set; }

        public Comment Comment { get; set; }
    }
}
