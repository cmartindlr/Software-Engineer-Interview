namespace ConsoleSolution.Interfaces
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
        /// The question that this answer provider can answer.
        /// </summary>
        string Question { get; }

        /// <summary>
        /// Provides the answer about the data.
        /// </summary>
        /// <param name="data">
        /// The data to get answers out of.
        /// </param>
        /// <returns>
        /// The answer formatted as a JSON.
        /// </returns>
        string ProvideAnswer(IEnumerable<T> data);
    }
}
