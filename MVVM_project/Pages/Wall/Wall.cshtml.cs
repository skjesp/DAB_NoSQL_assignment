using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAB_NoSQL_assignment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;


namespace DAB_NoSQL_assignment
{
    public class WallModel : PageModel
    {
        private readonly IMongoCollection<Post> _posts;
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Circle> _circles;
        private User user;
        private User guest;
        public List<Post> _postlist = new List<Post>();


        public WallModel(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("mongodb"));
            var database = client.GetDatabase("mongodb");
            _users = database.GetCollection<User>("Users");
            _posts = database.GetCollection<Post>("Posts");
            _circles = database.GetCollection<Circle>("Circles");
        }

        public void OnGet()
        {

        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string searchUser { get; set; }
            public string searchGuest { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!string.IsNullOrEmpty(Input.searchUser) || !string.IsNullOrEmpty(Input.searchGuest))
            {
                user = _users.Find(ur => ur.Name == Input.searchUser).FirstOrDefault();
                guest = _users.Find(ur => ur.Name == Input.searchGuest).FirstOrDefault();

                //Check if we found anyting
                if (user != null || guest !=null)
                {

                }
                else
                {
                    return RedirectToPage();
                }

            }
            else
            {
                return RedirectToPage();
            }

            _postlist = _posts.Find(post => post.PostOwner == user.Id).ToList();

            foreach (var posts in _postlist)
            {
                _postlist.AddRange(_posts.Find(post => post.PostOwner == user.Id).ToList());
            }

            
            

            return Page();
        }
    }
}