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

        public List<Post> posts;

        public ProfileModel(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("mongodb"));
            var database = client.GetDatabase("mongodb");
            _users = database.GetCollection<User>("Users");
            _posts = database.GetCollection<Post>("Posts");
        }

        [BindProperty]
        public Post post { get; set; }

        public IActionResult OnPostPostComment()
        {
            _posts.InsertOne(post);
            return Page();
        }

        public void OnGet()
        {
            posts = _posts.Find(post => true).ToList();
        }
    }
}