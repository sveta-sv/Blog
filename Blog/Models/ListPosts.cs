using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    /*View: Home/Index, PersonalArea/Index */
    public class ListPosts
    {
        public PaginatedList<Post> Posts { get; set; }

        public PaginatedList<Post> UserPosts { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
