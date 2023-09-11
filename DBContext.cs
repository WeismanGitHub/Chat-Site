//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using Application.Models;
//using System;

//namespace Application.Database {
//    public class UserContext : DbContext {
//        public DbSet<User> Users { get; set; }

//        public string DbPath { get; }

//        public UserContext(string name) {
//            var folder = Environment.SpecialFolder.LocalApplicationData;
//            var path = Environment.GetFolderPath(folder);
//            DbPath = System.IO.Path.Join(path, name);
//        }

//        protected override void OnConfiguring(DbContextOptionsBuilder options)
//            => options.UseSqlite($"Data Source={DbPath}");
//    }
//}

//public class User {
//    public string ID { get; set; }
//    public string Name { get; set; }
//    public string Password { get; set; } // I know I need to hash it.
//}