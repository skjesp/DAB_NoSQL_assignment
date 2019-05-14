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
    public class UserFeedModel : PageModel
    {
        private readonly IMongoCollection<User> _users;

        public UserFeedModel(IConfiguration config)
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

        //Search students by AU-id and get Courses with status and grade.
        //public async Task<IActionResult> OnPostAsync()
        //{
           // var users = from sc in 


            //var courseStudents = from sc in _db.CourseStudents
            //    select sc;

            //if (!string.IsNullOrEmpty(Input.searchString))
            //{
            //    courseStudents = courseStudents.Where(s => s.Student.AuId.Contains(Input.searchString));

            //    //Check if we found anyting
            //    if (courseStudents.AsNoTracking().ToList().Count != 0)
            //    {
            //        //Succes found a match

            //    }
            //    else
            //    {

            //        //Failed no match reload page - Show all.
            //        return RedirectToPage();
            //    }

            //}
            //else
            //{

            //    //Do nothing if no search-string id entered.
            //}
            ////Load list of StudentGroups
            //CourseStudents = await courseStudents.AsNoTracking().Include(cs => cs.Student).Include(cs => cs.Course).ToListAsync();

            ////Update current page.
        //    return Page();
        //}


    }
}