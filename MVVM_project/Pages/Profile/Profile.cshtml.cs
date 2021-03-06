﻿using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly IMongoCollection<Circle> _circles;

        // Lists that page reads from 
        public List<Post> postList;
        public User user;

        public ProfileModel(IConfiguration config)
        {
            //string connectionstring = "mongodb://localhost:27017/DAB_AFL3_Db";

            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("DAB_AFL3_Db");

            // var database = client.GetDatabase("mongodb://localhost:27017/DAB_AFL3_Db");
            _users = database.GetCollection<User>("Users");
            _posts = database.GetCollection<Post>("Posts");
            _comments = database.GetCollection<Comment>("Comments");
            _circles = database.GetCollection<Circle>("CircleIDs");
        }

        [BindProperty]
        public Post postBoundProperty { get; set; }

        [BindProperty]
        public String postcircleBoundProperty { get; set; }

        [BindProperty]
        public User userBoundProperty { get; set; }

        [BindProperty]
        public Comment commentBoundProperty { get; set; }

        public IActionResult OnPostPost()
        {
            //See if user exists
            user = _users.Find(user => user.Name == postBoundProperty.PostOwnerID).FirstOrDefault(); // TODO: Try using .single()

            if (user==null)
            {
                return Page();
            }
            
            // Add userid to postBoundProperty (object)
            postBoundProperty.PostOwnerID = user.Id;

            var crcl = _circles.Find(c => c.Id == postcircleBoundProperty).FirstOrDefault();
            postBoundProperty.Circle = crcl;

            // Set TimeStamp on post
            var culture = new CultureInfo("en-GB");
            postBoundProperty.TimeStamp = DateTime.Now.ToString(culture);
            
            // Upload post in database
            _posts.InsertOne(postBoundProperty);

            // Add selected posts to list.
            postList = _posts.Find(post => post.PostOwnerID == user.Id).ToList();
            return Page();
        }

        public IActionResult OnPostShowPosts()
        {
            user = _users.Find(user => user.Name == postBoundProperty.PostOwnerID).FirstOrDefault();
            postList = _posts.Find(post => post.PostOwnerID == user.Id).ToList();
            return Page();
        }

        public IActionResult OnPostAddComment(string id)
        {
            // Add writer-information to the comment which is to be added to a post.
            User CommentWriter = _users.Find(user => user.Name == commentBoundProperty.Writer_userName).Single();
            commentBoundProperty.Writer_userID = CommentWriter.Id;
            commentBoundProperty.PostID = id;

            // Find relevant post for the comment
            Post PostToAddComment = _posts.Find(post => post.Id == commentBoundProperty.PostID).Single();

            // Add the comment to the post
            if (PostToAddComment.Comments == null)
            {
                PostToAddComment.Comments = new List<Comment>();
            }

            var culture = new CultureInfo("en-GB");
            commentBoundProperty.TimeStamp = DateTime.Now.ToString(culture);
            PostToAddComment.Comments.Add(commentBoundProperty);

            // Update the post
            _posts.FindOneAndReplace(post => post.Id == PostToAddComment.Id, PostToAddComment);

            // Insert comment in database
            _comments.InsertOne(commentBoundProperty);
            return Page();
        }
 
    }
}