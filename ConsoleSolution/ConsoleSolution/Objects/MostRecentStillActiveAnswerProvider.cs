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
        /// Gets the most recently active person registered as a JSON.
        /// </summary>
        /// <param name="data">
        /// The data to analyze.
        /// </param>
        /// <returns>
        /// The JSON representing the most recently registered active person.
        /// </returns>
        public string ProvideAnswer(IEnumerable<RegisteredPerson> data)
        {
            // Get the person.
            RegisteredPerson? person = data.Where(x => x.IsActive == true &&
                                                       x.RegistrationDate != null) // Ignore if not active or no date
                                           .OrderByDescending(x => x.RegistrationDate) // Get most recent date.
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
