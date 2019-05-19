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
    public class AddtocircleModel : PageModel
    {
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Circle> _circles;

        public AddtocircleModel(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("mongodb"));
            var database = client.GetDatabase("mongodb");
            _users = database.GetCollection<User>("Users");
            _circles = database.GetCollection<Circle>("Circles");
        }
       
        [BindProperty]
        public string membername { get; set; }

        [BindProperty]
        public string activecircle { get; set; }

        public List<Circle> circleList;

        public void OnGet()
        {
            circleList = _circles.Find(circle => true).ToList();
        }

        public IActionResult OnPostAddMember()
        {
            User membertoadd = _users.Find(u => u.Name == membername).FirstOrDefault();
            Circle circletoaddmember = _circles.Find(c => c.Id == activecircle).FirstOrDefault();
            User user = _users.Find(usr => usr.Name == circletoaddmember.ForUser).FirstOrDefault();

            user.Circles.Remove(circletoaddmember);
           
            circletoaddmember.Members.Add(membertoadd);

            _circles.FindOneAndReplace(c => c.Id == circletoaddmember.Id,circletoaddmember);
            
            membertoadd.Circles.Add(circletoaddmember);

            _users.FindOneAndReplace(u => u.Name == membertoadd.Name,membertoadd);

            user.Circles.Add(circletoaddmember);
            
            _users.FindOneAndReplace(u => u.Name == user.Name, user);


            return RedirectToPage();
        }
    }
}