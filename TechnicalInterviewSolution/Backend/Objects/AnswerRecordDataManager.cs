using Backend.Interfaces;
using Backend.Models.Sql;
using Microsoft.EntityFrameworkCore;

namespace Backend.Objects
{
    /// <inheritdoc/>
    public class AnswerRecordDataManager : IAnswerRecordDataManager
    {
        // This follow the dependency inversion principle of SOLID by relying on abstractions that can be injected.

        /// <summary>
        /// The factory for producing database contexts.
        /// </summary>
        private IDbContextFactory<AnswerContext> _databaseFactory;

        /// <summary>
        /// Creates a new answer record data manager.
        /// </summary>
        /// <param name="databaseFactory">
        /// The factory for producing database contexts.
        /// </param>
        public AnswerRecordDataManager(IDbContextFactory<AnswerContext> databaseFactory)
        {
            this._databaseFactory = databaseFactory;
        }

        /// <inheritdoc/>
        public void Save(IEnumerable<AnswerRecord> answers)
        {
            using(AnswerContext database = this._databaseFactory.CreateDbContext()) 
            {
                // Add each answer and save.
                foreach(AnswerRecord answer in answers) 
                {
                    database.Add(answer);
                }
                database.SaveChanges();
            }
        }
    }
}
