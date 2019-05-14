using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAB_NoSQL_assignment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DAB_NoSQL_assignment
{
    public class WallModel : PageModel
    {
        public List<Post> WallPosts;



        public void OnGet()
        {

        }
    }
}