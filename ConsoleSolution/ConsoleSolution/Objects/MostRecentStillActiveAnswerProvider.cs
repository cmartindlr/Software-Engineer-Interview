using ConsoleSolution.Interfaces;
using ConsoleSolution.Models.Json;

// Credit Newtonsoft.
using Newtonsoft.Json;

namespace ConsoleSolution.Objects
{
    /// <summary>
    /// Determines the most recently active person registered.
    /// </summary>
    public class MostRecentStillActiveAnswerProvider : IAnswerProvider<RegisteredPerson>
    {
        // This follows the Liskov substitution principle of SOLID by allowing this provider to be used wherever an answer provider is desired.

        /// <inheritdoc/>
        public string Question { get; } = "Who is last individual that registered who is still active?";

        /// <summary>
        /// Gets the count of individuals in the collection over 50.
        /// </summary>
        /// <param name="data">
        /// The data to analyze.
        /// </param>
        /// <returns>
        /// The number of individuals in the data set over 50.
        /// </returns>
        public string ProvideAnswer(IEnumerable<RegisteredPerson> data)
        {
            // Get the person.
            RegisteredPerson? person = data.Where(x => x.IsActive == true &&
                                                       x.RegistrationDate != null)
                                           .OrderByDescending(x => x.RegistrationDate)
                                           .FirstOrDefault();

            if(person == null)
            {
                // If no answer, provide empty JSON.
                return "{}";
            }
            else
            {
                // No specific ask for a certain field, just the person, so return whole person.
                return JsonConvert.SerializeObject(person, Formatting.Indented);
            }
        }
    }
}
