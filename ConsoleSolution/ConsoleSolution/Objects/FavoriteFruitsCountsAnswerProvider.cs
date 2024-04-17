using ConsoleSolution.Interfaces;
using ConsoleSolution.Models.Json;
using System.Text;

namespace ConsoleSolution.Objects
{
    /// <summary>
    /// Provides answers to the question of the number of people choosing each favorite fruit.
    /// </summary>
    public class FavoriteFruitsCountsAnswerProvider : IAnswerProvider<RegisteredPerson>
    {
        // This follows the Liskov substitution principle of SOLID by allowing this provider to be used wherever an answer provider is desired.

        /// <inheritdoc/>
        public string Question { get; } = "What are the counts of each favorite fruit?";

        /// <summary>
        /// Gets every combination of fruits and counts.
        /// </summary>
        /// <param name="data">
        /// The data to analyze.
        /// </param>
        /// <returns>
        /// Every combination of fruits and counts in alphabetical order as a JSON.
        /// </returns>
        public string ProvideAnswer(IEnumerable<RegisteredPerson> data)
        {
            // Get the person.
            IEnumerable<(string Fruit, int Count)> fruitCounts = data.Where(x => x.FavoriteFruit != null)
                                                                     .GroupBy(x => x.FavoriteFruit)
                                                                     .Select(x => (x.Key ?? "", x.Count()));

            if(fruitCounts == null || 
               !fruitCounts.Any())
            {
                // If no answer, provide empty JSON.
                return "{}";
            }
            else
            {
                // Return a json where each fruit is a number.
                StringBuilder result = new StringBuilder();
                result.Append("{");
                foreach((string Fruit, int Count) fruitCount in fruitCounts.OrderBy(x => x.Fruit)) 
                {
                    result.Append("\n  \"" + fruitCount.Fruit + "\": " + fruitCount.Count + ",");
                }
                result.Remove(result.Length - 1, 1); // Remove last comma.
                result.Append("\n}");

                return result.ToString();
            }
        }
    }
}
