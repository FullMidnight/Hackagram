using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hackagram.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Hackagram.DAL
{
    public class AdminContext : DbContext
    {
        public AdminContext() : base("AdminContext")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<AdminContext>(null);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<AnsweredQuestion> AnsweredQuestions { get; set; }
    }
}