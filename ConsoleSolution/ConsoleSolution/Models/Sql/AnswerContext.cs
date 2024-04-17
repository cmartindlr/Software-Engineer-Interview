
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ConsoleSolution.Models.Sql
{
    public class AnswerContext : DbContext
    {
        public DbSet<AnswerRecord> AnswerRecords { get; set; }

        public AnswerContext() : base()
        {
            // Create the storage location if it does not exist.
            string folder = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TylerCarsonInterviewSolution");
            DirectoryInfo directory = new DirectoryInfo(folder);
            if(!directory.Exists)
            {
                directory.Create();
            }

            // Apply migrations.
            this.Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Define data source.
            optionsBuilder.UseSqlite($"Data Source={Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TylerCarsonInterviewSolution", "Answers.db")}");
        }
    }
}
