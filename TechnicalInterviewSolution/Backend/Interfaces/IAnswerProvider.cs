namespace Backend.Interfaces
{
    /// <summary>
    /// Provides answers to questions about the data.
    /// </summary>
    /// <typeparam name="T">
    /// The type of data to analyze.
    /// </typeparam>
    public interface IAnswerProvider<T>
    {
        // Simple interface satisfies interface segregation principle in SOLID.

        /// <summary>
        /// Provides the answer about the data.
        /// </summary>
        /// <param name="data">
        /// The data to get answers out of.
        /// </param>
        /// <returns>
        /// The answer, formatted as JSON if complex, along with the question it answers.
        /// </returns>
        (string Question, string Answer) ProvideAnswer(IEnumerable<T> data); // Uses dependency inversion by depending on an abstraction (IEnumberable).
    }
}
