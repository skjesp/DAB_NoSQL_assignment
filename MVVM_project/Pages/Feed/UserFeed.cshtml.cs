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
    public class UserFeedModel : PageModel
    {
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Post> _posts;
        private readonly IMongoCollection<Circle> _circles;
        private readonly IMongoCollection<Comment> _comments;
        private User user;
        public List<Post> _postlist=new List<Post>();
        public List<User> UserList { get; set; }

        public UserFeedModel(IConfiguration config)
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
            //Load list of User
            UserList = _users.Find(user => true).ToList();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string searchString { get; set; }
        }

        public IActionResult OnPost()
        {
            // Load list of Users
            UserList = _users.Find(user => true).ToList();

            // Search for user specifiec from HTML-input
            if (!string.IsNullOrEmpty(Input.searchString))
            {
                // Find the first user with the name specified from HTML-input
                user = _users.Find(ur => ur.Name == Input.searchString).FirstOrDefault();

                //Check if we found anyting
                if (user!=null)
                {
                    // The function will continue
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

            //Find all the posts that the user owns.
            _postlist = _posts.Find(post => post.PostOwnerID == user.Id).ToList();

            // Show all messages from users which the user follows
            if (user.FollowedUserIds.Count > 0)
            {
                foreach (var followed in user.FollowedUserIds)
                {
                    var usr = _users.Find(ur => ur.Id == followed).FirstOrDefault();
                    // Don't display if user is blacklisted.
                    if (!usr.BlackList.Contains(user.Id))
                    {
                        _postlist.AddRange(_posts.Find(post => post.PostOwnerID == usr.Id && post.Circle == null).ToList());
                    }
                }
            }

            // Check all the circles that user is part of.
            foreach (var CircleID in user.CircleIDs)
            {
                // Find the circle in database
                var circle = _circles.Find(c => c.Id == CircleID).FirstOrDefault();

                // Find the leader of the circle
                var leader = _users.Find(user => user.Id == circle.ForUser).FirstOrDefault();

                // If other users in the circle has blocked the user, then the user cannot see their posts.
                foreach (var mbr in circle.Members)
                {
                    var member = _users.Find(m => m.Id == mbr).FirstOrDefault();
                    if (!member.BlackList.Contains(user.Id) &&
                        member.Id != user.Id &&
                        user.FollowedUserIds.Contains(mbr))
                    {
                        _postlist.AddRange(_posts.Find(post => post.PostOwnerID == member.Id && post.Circle.Id == circle.Id).ToList());
                    }
                }
            }
            //Update current page.
            return Page();
        }


    }
}