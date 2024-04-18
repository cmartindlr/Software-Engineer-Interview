
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Backend.Models.Sql
{
    public class AnswerContext : DbContext
    {
        private static string _defaultDataSource = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TylerCarsonInterviewSolution", "Answers.db");

        public DbSet<AnswerRecord> AnswerRecords { get; set; }

        public AnswerContext() : base()
        {
            this.Setup();
        }

        public AnswerContext(DbContextOptions options) : base(options)
        {
            this.Setup();
        }

        public void Setup()
        {
            if(this.Database.GetDbConnection().DataSource == AnswerContext._defaultDataSource)
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
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                // Define data source.
                optionsBuilder.UseSqlite($"Data Source={AnswerContext._defaultDataSource}");
            }
        }
    }
}
