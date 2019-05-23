using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAB_NoSQL_assignment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;


namespace DAB_NoSQL_assignment
{
    public class UserBlacklistModel : PageModel
    {
        private readonly IMongoCollection<Blacklist> _blacklist;
        private readonly IMongoCollection<User> _users;

        public UserBlacklistModel(IConfiguration config)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("DAB_AFL3_Db");
            _blacklist = database.GetCollection<Blacklist>("Blacklist");
            _users = database.GetCollection<User>("Users");
        }

        [BindProperty]
        public InputModel Input{ get; set; }

        public class InputModel
        {
            public string userName { get; set; }
            public string blacklistName { get; set; }
        }
     
        public IActionResult OnPost()
        {
            //Validedata ModelState is valid.
            var user = _users.Find(u => u.Name == Input.userName).FirstOrDefault();
            var blacklistee = _users.Find(u => u.Name == Input.blacklistName).FirstOrDefault();

            if (user.BlackList == null)
            {
                user.BlackList = new List<string>();
            }
            user.BlackList.Add(blacklistee.Id);

            _users.FindOneAndReplace(c => c.Id == user.Id, user);

            return RedirectToPage();
        }
    }
}
