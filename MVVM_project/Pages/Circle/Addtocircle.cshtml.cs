using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;
using DAB_NoSQL_assignment.Models;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.Configuration;

namespace DAB_NoSQL_assignment
{
    public class AddtocircleModel : PageModel
    {
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Circle> _circles;

        public AddtocircleModel(IConfiguration config)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("DAB_AFL3_Db");
            _users = database.GetCollection<User>("Users");
            _circles = database.GetCollection<Circle>("Circles");
        }
       
        [BindProperty]
        public string MbrName { get; set; }

        [BindProperty]
        public string activecircle { get; set; }

        public List<Circle> circleList;

        public void OnGet()
        {
            circleList = _circles.Find(circle => true).ToList();
        }

        public string getmbrname(string id)
        {
            User mbr = _users.Find(u => u.Id == id).FirstOrDefault();
            if (mbr == null)
            {
                return "placeholder";
            }
            return mbr.Name;
        }

        public IActionResult OnPostAddMember(string id)
        {
            User membertoadd = _users.Find(u => u.Name == MbrName).FirstOrDefault();
            Circle circletoaddmember = _circles.Find(c => c.Id == id).FirstOrDefault();

            if (circletoaddmember.Members == null)
            {
                circletoaddmember.Members = new List<string>();
            }
            circletoaddmember.Members.Add(membertoadd.Id);

            _circles.FindOneAndReplace(c => c.Id == circletoaddmember.Id,circletoaddmember);

            if (membertoadd.Circles == null)
            {
                membertoadd.Circles = new List<string>();
            }
            membertoadd.Circles.Add(circletoaddmember.Id);

            _users.FindOneAndReplace(user => user.Name == MbrName, membertoadd);

            return RedirectToPage();
        }
    }
}