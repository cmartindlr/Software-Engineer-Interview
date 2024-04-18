using Backend.Interfaces;
using Backend.Models.Json;

namespace Backend.Objects.AnswerProviders
{
    /// <summary>
    /// Provider of the answer to "What is the total balance of all individuals combined?"
    /// </summary>
    public class TotalBalanceAnswerProvider : IAnswerProvider<RegisteredPerson>
    {
        // This follows the Liskov substitution principle of SOLID by allowing this provider to be used wherever an answer provider is desired.

        /// <summary>
        /// The question this class answers.
        /// </summary>
        private static string _question => "What is the total balance of all individuals combined?";

        /// <summary>
        /// Gets the total balance of all individuals as a JSON.
        /// </summary>
        /// <param name="data">
        /// The set of data to analyze.
        /// </param>
        /// <returns>
        /// A JSON containing the total balance of all individuals.
        /// </returns>
        public (string Question, string Answer) ProvideAnswer(IEnumerable<RegisteredPerson> data)
        {
            // Simple sum of all fields, 0 if not present.
            return (TotalBalanceAnswerProvider._question, (data.Any() ? data.Sum(x => x.NumericBalance ?? 0.0M) : 0.0M).ToString("C"));
        }
    }
}
