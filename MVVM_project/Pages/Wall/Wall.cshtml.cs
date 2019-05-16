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
        public List<Post> WallPosts;
        private readonly IMongoCollection<User> _users;


        public WallModel(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("mongodb"));
            var database = client.GetDatabase("mongodb");
            _users = database.GetCollection<User>("Users");
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
    }
}