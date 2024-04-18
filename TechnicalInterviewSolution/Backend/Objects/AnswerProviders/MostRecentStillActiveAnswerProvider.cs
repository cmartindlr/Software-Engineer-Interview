using Backend.Interfaces;
using Backend.Models.Json;

// Credit Newtonsoft. https://www.newtonsoft.com/json
using Newtonsoft.Json;
using System.Text;

namespace Backend.Objects.AnswerProviders
{
    /// <summary>
    /// Determines the most recently active person registered.
    /// </summary>
    public class MostRecentStillActiveAnswerProvider : IAnswerProvider<RegisteredPerson>
    {
        // This follows the Liskov substitution principle of SOLID by allowing this provider to be used wherever an answer provider is desired.

        /// <summary>
        /// The question this class answers.
        /// </summary>
        private static string _question = "Who is last individual that registered who is still active?";

        /// <summary>
        /// Gets the most recently active person registered as a JSON.
        /// </summary>
        /// <param name="data">
        /// The data to analyze.
        /// </param>
        /// <returns>
        /// The JSON representing the most recently registered active person.
        /// </returns>
        public (string Question, string Answer) ProvideAnswer(IEnumerable<RegisteredPerson> data)
        {
            // Get the person.
            IEnumerable<IGrouping<DateTime, RegisteredPerson>> dateGroups =
                                                   data.Where(x => x.IsActive == true &&
                                                                   x.RegistrationDate != null) // Ignore if not active or no date
                                                       .GroupBy(x => x.RegistrationDate.Value); // Get all with same date.


            if (dateGroups == null ||
               !dateGroups.Any())
            {
                // If no answer, provide empty JSON.
                return (MostRecentStillActiveAnswerProvider._question, "{}");
            }
            else
            {
                IEnumerable<RegisteredPerson>? people = dateGroups.OrderByDescending(x => x.Key)
                                                                  .FirstOrDefault(); // Get most recent date.
                if (people == null ||
                   !people.Any())
                {
                    // If no answer, provide empty JSON.
                    return (MostRecentStillActiveAnswerProvider._question, "{}");
                }
                else if (people.Count() > 1)
                {
                    // Build a list of answers.
                    StringBuilder matchingPeople = new StringBuilder();
                    matchingPeople.Append("[");
                    foreach (RegisteredPerson person in people.OrderBy(x => x.Name?.FormattedName ?? ""))
                    {
                        matchingPeople.Append($"\n  {JsonConvert.SerializeObject(person, Formatting.Indented).Replace("\n", "\n  ")},");
                    }
                    matchingPeople.Remove(matchingPeople.Length - 1, 1);
                    matchingPeople.Append("\n]");
                    return (MostRecentStillActiveAnswerProvider._question, matchingPeople.ToString());
                }
                else
                {
                    RegisteredPerson person = people.First();

                    // No specific ask for a certain field, just the person, so return whole person.
                    return (MostRecentStillActiveAnswerProvider._question, JsonConvert.SerializeObject(person, Formatting.Indented));
                }
            }
        }
    }
}
