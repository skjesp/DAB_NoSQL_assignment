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
    public class FollowModel : PageModel
    {
        // Collections from database
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Post> _posts;
        private readonly IMongoCollection<Comment> _comments;
        private readonly IMongoCollection<Circle> _circles;

        // Lists that page reads from 
        public List<Post> postList;
        public User user;

        public FollowModel(IConfiguration config)
        {
            //string connectionstring = "mongodb://localhost:27017/DAB_AFL3_Db";

            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("DAB_AFL3_Db"); ;
            
            // var database = client.GetDatabase("mongodb://localhost:27017/DAB_AFL3_Db");
            _users = database.GetCollection<User>("Users");
            _posts = database.GetCollection<Post>("Posts");
            _comments = database.GetCollection<Comment>("Comments");
            _circles = database.GetCollection<Circle>("Circles");
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Follower { get; set; }
            public string Followed { get; set; }
        }

        public IActionResult OnPost()
        {
            //Validedata ModelState is valid.
            var user = _users.Find(u => u.Name == Input.Follower).FirstOrDefault();
            var Followed = _users.Find(u => u.Name == Input.Followed).FirstOrDefault();

            if (user.FollowedUserIds == null)
            {
                user.FollowedUserIds = new List<string>();
            }
            user.FollowedUserIds.Add(Followed.Id);

            _users.FindOneAndReplace(c => c.Id == user.Id, user);

            return RedirectToPage();
        }

    }
}