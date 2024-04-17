using ConsoleSolution.Interfaces;
using ConsoleSolution.Models.Json;

namespace ConsoleSolution.Objects.AnswerProviders
{
    /// <summary>
    /// Answers the count of individuals over 50.
    /// </summary>
    public class Over50AnswerProvider : IAnswerProvider<RegisteredPerson>
    {
        // This follows the Liskov substitution principle of SOLID by allowing this provider to be used wherever an answer provider is desired.

        /// <inheritdoc/>
        public string Question { get; } = "What is the count of individuals over the age of 50?";

        /// <summary>
        /// Gets the count of individuals in the collection over 50 as a JSON.
        /// </summary>
        /// <param name="data">
        /// The data to analyze.
        /// </param>
        /// <returns>
        /// The number of individuals in the data set over 50 as a JSON.
        /// </returns>
        public string ProvideAnswer(IEnumerable<RegisteredPerson> data)
        {
            return data.Count(x => x.Age != null && // Ignore missing age.
                                   x.Age > 50) // Ensure age is over 50.
                       .ToString();
        }
    }
}
