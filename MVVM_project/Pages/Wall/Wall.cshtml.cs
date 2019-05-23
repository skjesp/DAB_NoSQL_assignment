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
        private readonly IMongoCollection<Comment> _comments;
        private User user;
        private User guest;
        public List<Post> _postlist = new List<Post>();
        public List<User> UserList { get; set; }

        public WallModel(IConfiguration config)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("DAB_AFL3_Db");
            _users = database.GetCollection<User>("Users");
            _posts = database.GetCollection<Post>("Posts");
            _circles = database.GetCollection<Circle>("CircleIDs");
            _comments = database.GetCollection<Comment>("Comments");
        }

        public void OnGet()
        {
            UserList = _users.Find(user => true).ToList();
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
            // Load list of Users
            UserList = _users.Find(user => true).ToList();

            // Find the users which are requested from HTML-Input.
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

            // If the guest is blacklisted, don't return any information.
            if (user.BlackList.Contains(guest.Id))
            {
                return RedirectToPage();
            }
            
            // Get all posts which isn't restricted by a circle.
            _postlist = _posts.Find(post => post.PostOwnerID == user.Id && post.Circle == null).ToList();
            

            foreach (var crcl in guest.CircleIDs)
            {
                var circle = _circles.Find(c => c.Id == crcl).FirstOrDefault();
                if (circle.Members.Contains(user.Id)||circle.ForUser==user.Name)
                {
                    _postlist.AddRange(_posts.Find(post => post.PostOwnerID == user.Id && post.Circle.Id == circle.Id).ToList());
                }
                
            }
            
            return Page();
        }
    }
}