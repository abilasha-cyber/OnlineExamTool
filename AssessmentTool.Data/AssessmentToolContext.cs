using Microsoft.AspNet.Identity.EntityFramework;
using AssessmentTool.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentTool.Data
{
    public class AssessmentToolContext : IdentityDbContext<AssessmentToolUser>, IDisposable
    {
        public AssessmentToolContext()
            : base("AssessmentToolConnection", throwIfV1Schema: false)
        {
            //Database.SetInitializer<AssessmentToolContext>(new CreateDatabaseIfNotExists<AssessmentToolContext>());

            Database.SetInitializer<AssessmentToolContext>(new AssessmentToolDatabaseInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This needs to go before the other rules!

            modelBuilder.Entity<AssessmentToolUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
        }

        public static AssessmentToolContext Create()
        {
            return new AssessmentToolContext();
        }

        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<StudentQuiz> StudentQuizzes { get; set; }
        public DbSet<AttemptedQuestion> AttemptedQuestions { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}
