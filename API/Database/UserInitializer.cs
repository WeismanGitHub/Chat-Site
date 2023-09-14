using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using API.Models;
using System.Diagnostics;

namespace API.DAL {
    public class SchoolInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<UserContext> {
        protected override void Seed(UserContext context) {
            var users = new List<User> {
                new User{Name="Carson", Email="name@example.com"},
                new User{Name="Meredith", Email="name@example.com"},
                new User{Name="Arturo", Email = "name@example.com"},
                new User{Name="Gytis", Email = "name@example.com"},
                new User{Name="Yan", Email = "name@example.com"},
                new User{Name="Peggy", Email = "name@example.com"},
                new User{Name="Laura", Email = "name@example.com"},
                new User{Name="Nino", Email = "name@example.com"}
            };

            users.ForEach(user=> context.Users.Add(user));
            context.SaveChanges();
        }
    }
}