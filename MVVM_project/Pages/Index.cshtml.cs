using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAB_NoSQL_assignment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace DAB_NoSQL_assignment.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AddUserModel _addUser;

        public List<User> users { get; set; }

        public IndexModel(AddUserModel addUser)
        {
            _addUser = addUser;
        }

        public void OnGet()
        {
            //Load list of User
            users =  _addUser.Get();
        }

    }
}
