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
    public class IndexModel : PageModel
    {
        private readonly AddUserModel _addUser;

        private readonly UserBlacklistModel _blacklist;

        public List<User> users { get; set; }

        public List<Blacklist> blacklist { get; set; }

        public IndexModel(AddUserModel addUser, UserBlacklistModel blacklist)
        {
            _addUser = addUser;
            _blacklist = blacklist;
        }

        public void OnGet()
        {
            //Load list of User
            users =  _addUser.Get();

            //Load blacklist
            blacklist =  _blacklist.Get();
        }

    }
}
