using Backend.Interfaces;
using Backend.Models.Json;

namespace Backend.Objects.AnswerProviders
{
    /// <summary>
    /// Answers the count of individuals over 50.
    /// </summary>
    public class Over50AnswerProvider : IAnswerProvider<RegisteredPerson>
    {
        // This follows the Liskov substitution principle of SOLID by allowing this provider to be used wherever an answer provider is desired.

        /// <summary>
        /// The question this class answers.
        /// </summary>
        private static string _question = "What is the count of individuals over the age of 50?";

        /// <summary>
        /// Gets the count of individuals in the collection over 50 as a JSON.
        /// </summary>
        /// <param name="data">
        /// The data to analyze.
        /// </param>
        /// <returns>
        /// The number of individuals in the data set over 50 as a JSON.
        /// </returns>
        public (string Question, string Answer) ProvideAnswer(IEnumerable<RegisteredPerson> data)
        {
            return (Over50AnswerProvider._question, 
                    data.Count(x => x.Age != null && // Ignore missing age.
                                    x.Age > 50) // Ensure age is over 50.
                        .ToString());
        }
    }
}
