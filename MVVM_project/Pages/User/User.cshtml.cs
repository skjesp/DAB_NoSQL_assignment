using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAB_NoSQL_assignment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;


namespace DAB_NoSQL_assignment
{
    public class AddUserModel : PageModel
    {
        private readonly IMongoCollection<User> _users;

        public AddUserModel(IConfiguration config)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("DAB_AFL3_Db");
            _users = database.GetCollection<User>("Users");
        }

        [BindProperty]
        public User user { get; set; }

        public List<User> Get()
        {
            return _users.Find(user => true).ToList();
        }

        public IActionResult OnPost()
        {
            //Validedata ModelState is valid.
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Add object to database & save changes.
            user.CircleIDs=new List<string>();
            user.BlackList = new List<string>();
            user.FollowedUserIds = new List<string>();
            _users.InsertOne(user);

            return RedirectToPage();
        }
    }
}
