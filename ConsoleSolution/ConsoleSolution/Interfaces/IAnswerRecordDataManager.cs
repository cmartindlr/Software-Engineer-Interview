using ConsoleSolution.Models.Sql;

namespace ConsoleSolution.Interfaces
{
    /// <summary>
    /// Manages answer record data storage.
    /// </summary>
    public interface IAnswerRecordDataManager
    {
        // Simple interface satisfies interface segregation principle in SOLID.

        /// <summary>
        /// Saves the set of answers.
        /// </summary>
        /// <param name="answers">
        /// The set of answers to be saved.
        /// </param>
        void Save(IEnumerable<AnswerRecord> answers);
    }
}
