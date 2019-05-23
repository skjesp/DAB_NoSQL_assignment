using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using DAB_NoSQL_assignment.Models;
using MongoDB.Bson;

namespace DAB_NoSQL_assignment
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            SeedDatabase();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddScoped<AddUserModel>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();
        }

        public void SeedDatabase()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            client.DropDatabase("DAB_AFL3_Db");
            var database = client.GetDatabase("DAB_AFL3_Db");

            var UserCollection = database.GetCollection<User>("Users");
            var CircleCollection = database.GetCollection<Circle>("CircleIDs");
            var CommentCollection = database.GetCollection<Comment>("Comments");
            var PostCollection = database.GetCollection<Post>("Posts");
            var BlacklistCollection = database.GetCollection<Blacklist>("Blacklists");


            #region UserSeedCreation and databaseinsertion
            // UserCreation
            User User1 = new User()
            {
                Name = "Peter",
                Gender = "Mand",
                Age = "20",
                CircleIDs = new List<string>(),
                BlackList = new List<string>(),
                FollowedUserIds = new List<string>(),
            };
            User User2 = new User()
            {
                Name = "Signe",
                Gender = "Kvinde",
                Age = "25",
                CircleIDs = new List<string>(),
                BlackList = new List<string>(),
                FollowedUserIds = new List<string>(),
            };
            User User3 = new User()
            {
                Name = "Rune",
                Gender = "Mand",
                Age = "19",
                CircleIDs = new List<string>(),
                BlackList = new List<string>(),
                FollowedUserIds = new List<string>(),
            };
            User User4 = new User()
            {
                Name = "Kamilla",
                Gender = "Kvinde",
                Age = "23",
                CircleIDs = new List<string>(),
                BlackList = new List<string>(),
                FollowedUserIds = new List<string>(),
            };
            User User5 = new User()
            {
                Name = "Pernille",
                Gender = "Kvinde",
                Age = "28",
                CircleIDs = new List<string>(),
                BlackList = new List<string>(),
                FollowedUserIds = new List<string>(),
            };

            // UserInsertion - Make MongoDB create ID for them
            UserCollection.InsertOne(User1);
            UserCollection.InsertOne(User2);
            UserCollection.InsertOne(User3);
            UserCollection.InsertOne(User4);
            UserCollection.InsertOne(User5);

            
            #endregion

            #region CircleSeedCreation

            Circle Circle1 = new Circle()
            {
                ForUser = User1.Id,
                Members = new List<string>{User1.Id, User2.Id},
            };

            Circle Circle2 = new Circle()
            {
                ForUser = User4.Id,
                Members = new List<string>{ User1.Id, User5.Id, User4.Id },
            };

            CircleCollection.InsertOne(Circle1);
            CircleCollection.InsertOne(Circle2);
            #endregion

            #region PostSeedCreation
            Post Post1 = new Post()
            {
                PostOwnerID = User1.Id,
                Text = "First post from Peter! Only circle 1 should see this",
                Comments = null,
                Circle = Circle1,
            };
            
            Post Post2 = new Post()
            {
                PostOwnerID = User4.Id,
                Text = "First post from Kamilla! Only circle 2 should see this",
                Comments = null,
                Circle = Circle2,
            };

            Post Post3 = new Post()
            {
                PostOwnerID = User5.Id,
                Text = "Public post from Pernille. Only Peter should be able to see this. All other users are blacklisted."
            };

            Post Post4 = new Post()
            {
                PostOwnerID = User1.Id,
                Text = "Second post from Peter! This post is public.",
                Comments = null,
            };
            
            PostCollection.InsertOne(Post1);
            PostCollection.InsertOne(Post2);
            PostCollection.InsertOne(Post3);
            #endregion

            #region CommentSeedCreation

            Comment Comment1 = new Comment()
            {
                Text = "First comment on post 1.",
                OwnerPostID = Post1.Id,
                Writer_userID = User2.Id,
                Writer_userName = User2.Name,
            };
            Comment Comment2 = new Comment()
            {
                Text = "Second comment on post 1.",
                OwnerPostID = Post1.Id,
                Writer_userID = User4.Id,
                Writer_userName = User4.Name,
            };
            Comment Comment3 = new Comment()
            {
                Text = "First Comment on post 2.",
                OwnerPostID = Post2.Id,
                Writer_userID = User3.Id,
                Writer_userName = User3.Name,
            };
            Comment Comment4 = new Comment()
            {
                Text = "Second Comment on Post 2",
                OwnerPostID = Post2.Id,
                Writer_userID = User1.Id,
                Writer_userName = User1.Name,
            };

            CommentCollection.InsertOne(Comment1);
            CommentCollection.InsertOne(Comment2);
            CommentCollection.InsertOne(Comment3);
            CommentCollection.InsertOne(Comment4);
            #endregion

            #region BlackListCreation

            Blacklist Blacklist1 = new Blacklist()
            {
                ForUser = User5.Id,
                listUsers = new List<string>{ User2.Id, User3.Id, User4.Id},
            };
            
            // Insert blacklist for User5
            User5.BlackList.Add(Blacklist1.Id);
            BlacklistCollection.InsertOne(Blacklist1);
            #endregion

            #region RelationCreation

            // Followers Configuration
            User1.FollowedUserIds.Add(User5.Id);
            User2.FollowedUserIds.Add(User1.Id);
            User2.FollowedUserIds.Add(User5.Id);
            User3.FollowedUserIds.Add(User1.Id);
            User3.FollowedUserIds.Add(User5.Id);
            User4.FollowedUserIds.Add(User5.Id);
            User5.FollowedUserIds.Add(User4.Id);
            User5.FollowedUserIds.Add(User1.Id);

            // CircleIDs Configuration
            User1.CircleIDs.Add(Circle1.Id);
            User2.CircleIDs.Add(Circle1.Id);
            User3.CircleIDs.Add(Circle1.Id);
            User1.CircleIDs.Add(Circle2.Id);
            User5.CircleIDs.Add(Circle2.Id);

            // Blacklisting configuration
            User1.BlackList.Add(User3.Id);
            User1.BlackList.Add(User5.Id);
            User5.BlackList.Add(User2.Id);
            User5.BlackList.Add(User3.Id);
            User5.BlackList.Add(User4.Id);

            // Update the database
            UserCollection.ReplaceOne(user => user.Name == "Peter", User1);
            UserCollection.ReplaceOne(user => user.Name == "Signe", User2);
            UserCollection.ReplaceOne(user => user.Name == "Rune", User3);
            UserCollection.ReplaceOne(user => user.Name == "Kamilla", User4);
            UserCollection.ReplaceOne(user => user.Name == "Pernille", User5);

            #endregion

        }





    }
}
