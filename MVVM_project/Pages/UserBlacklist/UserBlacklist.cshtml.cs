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
            var client = new MongoClient(config.GetConnectionString("mongodb"));
            var database = client.GetDatabase("mongodb");
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

        [BindProperty]
        public User user { get; set; }

        public List<SelectListItem> listUsers { get; set; }

        public List<User> CreateListAllUsers()
        {
            return _users.Find(user => true).ToList();
        }

        public User FindUserByName(string userName)
        {
            return _users.Find(user => user.Name == userName).FirstOrDefault();
        }

        public List<Blacklist> Get()
        {
            return _blacklist.Find(user => true).ToList();
        }


        //Parameter 1(userName): The userName which to check if on 2nd parameters(userNamesBlacklist) blacklist.
        public bool IsUserNameOnUsersBlacklist(string userName, string userNamesBlacklist)
        {
            //Find both users by their names
            var userToCheck = FindUserByName(userName);

            var usersBlackList = FindUserByName(userNamesBlacklist);

            var result = _blacklist.Find(x => x.ForUser.Equals(usersBlackList.Id) && x.listUsers.Contains(ObjectId.Parse(userToCheck.Id))).FirstOrDefault();

            if(result != null)
            {
                return true;

            } else {

                return false;
            }
        }

        public void OnGet()
        {
            List<SelectListItem> listUser = new List<SelectListItem>();  
            foreach (var user in CreateListAllUsers())
            {
                listUser.Add(new SelectListItem() { Value = user.Id, Text = user.Name });
            }
            listUsers = listUser;
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
