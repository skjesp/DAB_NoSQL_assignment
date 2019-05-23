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
    public class createcircleModel : PageModel
    {
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Circle> _circles;
        private User user;

        public createcircleModel(IConfiguration config)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("DAB_AFL3_Db");
            _users = database.GetCollection<User>("Users");
            _circles = database.GetCollection<Circle>("CircleIDs");
        }

        [BindProperty]
        public Circle circlebindproperty { get; set; }

        [BindProperty]
        public String owner { get; set; }

        public IActionResult OnPost()
        {
            user = _users.Find(user => user.Name == owner).FirstOrDefault();

            if (user == null)
            {
                return Page();
            }
            circlebindproperty.ForUser = user.Name;
            circlebindproperty.Members = new List<string>();
            _circles.InsertOne(circlebindproperty);

            if (user.CircleIDs == null)
            {
                user.CircleIDs = new List<string>();
            }

            user.CircleIDs.Add(circlebindproperty.Id);
            _users.FindOneAndReplace(u => u.Name == user.Name, user);

            return Page();
        }


    }
}