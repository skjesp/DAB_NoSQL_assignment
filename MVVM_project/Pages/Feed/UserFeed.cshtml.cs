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
            _circles = database.GetCollection<Circle>("Circles");
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
            //Load list of User
            UserList = _users.Find(user => true).ToList();


            if (!string.IsNullOrEmpty(Input.searchString))
            {
                user = _users.Find(ur => ur.Name == Input.searchString).FirstOrDefault();

                //Check if we found anyting
                if (user!=null)
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

            _postlist= _posts.Find(post => post.PostOwner == user.Id).ToList();

            foreach (var crcl in user.Circles)
            {
                var circle = _circles.Find(c => c.Id == crcl).FirstOrDefault();
                var leader = _users.Find(user => user.Name == circle.ForUser).FirstOrDefault();
                if (!leader.BlackList.Contains(user.Id))
                {
                    _postlist.AddRange(_posts.Find(post => post.PostOwner == leader.Id && post.Circle.Id==circle.Id).ToList());
                }

                foreach (var mbr in circle.Members)
                {
                    var member = _users.Find(m => m.Id == mbr).FirstOrDefault();
                    if (!member.BlackList.Contains(user.Id) && member.Id != user.Id)
                    {
                        _postlist.AddRange(_posts.Find(post => post.PostOwner == member.Id && post.Circle.Id == circle.Id).ToList());
                    }
                }
            }

            if (user.BlackList.Count > 0)
            {
                foreach (var followed in user.FollowedUserIds)
                {
                    var usr = _users.Find(ur => ur.Id == followed).FirstOrDefault();
                    if (!usr.BlackList.Contains(user.Id))
                    {
                        _postlist.AddRange(_posts.Find(post => post.PostOwner == usr.Id && post.Circle == null).ToList());
                    }
                }
            }
            
            //Update current page.
            return Page();
        }


    }
}