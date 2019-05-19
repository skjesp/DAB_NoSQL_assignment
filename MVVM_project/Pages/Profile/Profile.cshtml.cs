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
        // Collections from database
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Post> _posts;
        private readonly IMongoCollection<Comment> _comments;

        // Lists that page reads from 
        public List<Post> postList;
        public List<User> userList;

        public ProfileModel(IConfiguration config)
        {
            //string connectionstring = "mongodb://localhost:27017/DAB_AFL3_Db";

            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("DAB_AFL3_Db");
            
            // var database = client.GetDatabase("mongodb://localhost:27017/DAB_AFL3_Db");
            _users = database.GetCollection<User>("Users");
            _posts = database.GetCollection<Post>("Posts");
            _comments = database.GetCollection<Comment>("Comments");
        }

        [BindProperty]
        public Post postBoundProperty { get; set; }

        [BindProperty]
        public User userBoundProperty { get; set; }

        [BindProperty]
        public Comment commentBoundProperty { get; set; }

        public IActionResult OnPostPost()
        {
            //See if user exists
            userList = _users.Find(user => user.Name == postBoundProperty.PostOwner).ToList(); // TODO: Try using .single()

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

        public IActionResult OnPostShowPosts()
        {
            userList = _users.Find(user => user.Name == postBoundProperty.PostOwner).ToList();
            postList = _posts.Find(post => post.PostOwner == userList[0].Id).ToList();
            return Page();
        }

        public IActionResult OnPostAddComment()
        {
            // Add writer-information to the comment which is to be added to a post.
            User CommentWriter = _users.Find(user => user.Name == commentBoundProperty.Writer_userName).Single();
            commentBoundProperty.Writer_userID = CommentWriter.Id;

            // Find relevant post for the comment
            Post PostToAddComment = _posts.Find(post => post.Id == commentBoundProperty.OwnerPostID).Single();

            // Add the comment to the post
            if (PostToAddComment.Comments == null)
            {
                PostToAddComment.Comments = new List<Comment>();
            }

            PostToAddComment.Comments.Add(commentBoundProperty);

            // Update the post
            _posts.FindOneAndReplace(post => post.PostOwner == PostToAddComment.PostOwner, PostToAddComment);

            // Insert comment in database
            _comments.InsertOne(commentBoundProperty);
            return Page();
        }
 
    }
}