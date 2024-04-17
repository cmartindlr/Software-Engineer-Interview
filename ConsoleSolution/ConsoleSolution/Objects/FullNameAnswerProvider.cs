using ConsoleSolution.Interfaces;
using ConsoleSolution.Models.Json;
using System.Text;

namespace ConsoleSolution.Objects
{
    /// <summary>
    /// A provider of answers to the question "What is the full name of the individual with the id of 5aabbca3e58dc67745d720b1 in the format of lastname, firstname?".
    /// </summary>
    public class FullNameAnswerProvider : IAnswerProvider<RegisteredPerson>
    {
        // This follows the Liskov substitution principle of SOLID by allowing this provider to be used wherever an answer provider is desired.
        
        /// <inheritdoc/>
        public string Question => "What is the full name of the individual with the id of 5aabbca3e58dc67745d720b1 in the format of lastname, firstname?";

        /// <summary>
        /// Gets the name of the person with the given ID in a JSON.
        /// </summary>
        /// <param name="data">
        /// The data set to analyze.
        /// </param>
        /// <returns>
        /// The name of the person with the given ID.
        /// </returns>
        public string ProvideAnswer(IEnumerable<RegisteredPerson> data)
        {
            // Find the person by ID.
            IEnumerable<RegisteredPerson> people = data.Where(x => x.ID == "5aabbca3e58dc67745d720b1");

            if(people == null ||
               !people.Any() ||
               people.All(x => x.Name == null))
            {
                // Return empty JSON if null.
                return "{}";
            }
            else if(people.Count() > 1)
            {
                // Build a list of answers.
                StringBuilder matchingPeople = new StringBuilder();
                matchingPeople.Append("[");
                foreach(RegisteredPerson person in people)
                {
                    matchingPeople.Append($"\n  {{\n    \"answer\": \"{person.Name.FormattedName}\"\n  }},");
                }
                matchingPeople.Remove(matchingPeople.Length - 1, 1);
                matchingPeople.Append("\n]");
                return matchingPeople.ToString();
            }
            else 
            { 
                // Return the only person.
                RegisteredPerson person = people.First();
                return $"{{\n  \"answer\": \"{person.Name.FormattedName}\"\n}}";
            }
        }
    }
}
