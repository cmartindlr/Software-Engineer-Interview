

namespace ConsoleSolution.Interfaces
{
    /// <summary>
    /// Represents an aggregator of answers.
    /// </summary>
    /// <typeparam name="T">
    /// The type of object about which answers are sought.
    /// </typeparam>
    public interface IAnswerAggregator<T>
    {
        // Simple interface satisfies interface segregation principle in SOLID.

        /// <summary>
        /// The set of answer providers injected.
        /// </summary>
        IEnumerable<IAnswerProvider<T>> AnswerProviders { get; }

        /// <summary>
        /// Aggregates the answers provided by the answer providers.
        /// </summary>
        /// <param name="data">
        /// The data set to provide answers over.
        /// </param>
        /// <returns>
        /// A JSON containing the set of answers in a list.
        /// </returns>
        IEnumerable<string> AggregateAnswers(IEnumerable<T> data);
    }
}
