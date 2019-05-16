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

        public UserFeedModel(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("mongodb"));
            var database = client.GetDatabase("mongodb");
            _users = database.GetCollection<User>("Users");
            _posts = database.GetCollection<Post>("Posts");
            _circles = database.GetCollection<Circle>("Circles");
            _comments = database.GetCollection<Comment>("Comments");
        }

        public void OnGet()
        {

        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string searchString { get; set; }
        }

        //Search students by AU-id and get Courses with status and grade.
        public async Task<IActionResult> OnPostAsync()
        {
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

            _postlist=_posts.Find(post => post.PostOwner == user.Id).ToList();

            foreach (var circle in user.Circles)
            {
                var crcl = _circles.Find(circ => circ.Id == circle).FirstOrDefault();
                var leader = _users.Find(user => user.Id == crcl.ForUser).FirstOrDefault();
                if (!leader.BlackList.Contains(user.Id))
                {
                    _postlist.AddRange(_posts.Find(post => post.PostOwner == leader.Id).ToList());
                }

                foreach (var member in crcl.Members)
                {
                    var usr = _users.Find(user => user.Id == member).FirstOrDefault();
                    if (!usr.BlackList.Contains(user.Id))
                    {
                        _postlist.AddRange(_posts.Find(post => post.PostOwner == usr.Id).ToList());
                    }
                }
            }

            foreach (var followed in user.FollowedUserIds)
            {
                var usr = _users.Find(ur => ur.Id == followed).FirstOrDefault();
                if (!usr.BlackList.Contains(user.Id))
                {
                    _postlist.AddRange(_posts.Find(post => post.PostOwner == usr.Id).ToList());
                }
            }
            //Update current page.
            return Page();
        }


    }
}