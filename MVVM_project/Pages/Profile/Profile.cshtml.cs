using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using MongoDB.Driver;
using DAB_NoSQL_assignment.Models;
using Microsoft.Extensions.Configuration;

namespace DAB_NoSQL_assignment
{
    public class ProfileModel : PageModel
    {
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Post> _posts;

        public List<Post> postList;
        public List<User> userList;

        public ProfileModel(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("mongodb"));
            var database = client.GetDatabase("mongodb");
            _users = database.GetCollection<User>("Users");
            _posts = database.GetCollection<Post>("Posts");
        }

        [BindProperty]
        public Post postBoundProperty { get; set; }

        [BindProperty]
        public User userBoundProperty { get; set; }

        public IActionResult OnPostPostComment()
        {
            //See if user exists
            userList = _users.Find(user => user.Name == postBoundProperty.PostOwner).ToList();

            if (userList.Count == 0)
            {
                return Page();
            }
            
            // Add userid to postBoundProperty (object)
            postBoundProperty.PostOwner = userList[0].Id;

            // Upload post in database
            _posts.InsertOne(postBoundProperty);

            // Add selected posts to list.
            postList = _posts.Find(post => post.PostOwner == userList[0].Id).ToList();
            return Page();
        }

        public IActionResult OnPostShowComment()
        {
            userList = _users.Find(user => user.Name == postBoundProperty.PostOwner).ToList();
            postList = _posts.Find(post => post.PostOwner == userList[0].Id).ToList();
            return Page();
        }
 
    }
}