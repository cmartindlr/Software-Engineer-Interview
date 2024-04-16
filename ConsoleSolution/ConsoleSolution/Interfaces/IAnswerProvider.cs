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
        /// Provides the answer about the data.
        /// </summary>
        /// <param name="data">
        /// The data to get answers out of.
        /// </param>
        /// <returns>
        /// The answer as a string suitable for display.
        /// </returns>
        string ProvideAnswer(IEnumerable<T> data);
    }
}
