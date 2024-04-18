using Backend.Interfaces;
using Backend.Models.Json;
using System.Text;

namespace Backend.Objects.AnswerProviders
{
    /// <summary>
    /// Provider of the answer to the question of "What is the most common eye color?"
    /// </summary>
    public class MostCommenEyeColorAnswerProvider : IAnswerProvider<RegisteredPerson>
    {
        // This follows the Liskov substitution principle of SOLID by allowing this provider to be used wherever an answer provider is desired.

        /// <inheritdoc/>
        public string Question => "What is the most common eye color?";

        /// <summary>
        /// Gets the answer to the most common eye color as a JSON.
        /// </summary>
        /// <param name="data">
        /// The set of data to analyze.
        /// </param>
        /// <returns>
        /// A JSON containing a field indicating the most common eye color.
        /// </returns>
        public string ProvideAnswer(IEnumerable<RegisteredPerson> data)
        {
            IEnumerable<(string EyeColor, int Count)> eyeColorCounts =
                                                       data.Where(x => x.EyeColor != null) // Filter out any with no eye color.
                                                           .GroupBy(x => x.EyeColor)
                                                           .OrderByDescending(x => x.Count()) // Get the largest group first.
                                                           .Select(x => (x.Key ?? "", x.Count())); // Only care about eye color and count for now.


            if (eyeColorCounts == null ||
               !eyeColorCounts.Any())
            {
                // Return empty JSON if null.
                return "NULL";
            }
            else
            {
                int maxCount = eyeColorCounts.Max(x => x.Count);
                IEnumerable<string> mostCommonEyeColors = eyeColorCounts.Where(x => x.Count == maxCount)
                                                                        .Select(x => x.EyeColor);
                if (mostCommonEyeColors == null ||
                   !mostCommonEyeColors.Any())
                {
                    // Return empty JSON if null.
                    return "NULL";
                }
                else if (mostCommonEyeColors.Count() > 1)
                {
                    // Build a list of answers.
                    StringBuilder matchingPeople = new StringBuilder();
                    foreach (string eyeColor in mostCommonEyeColors.OrderBy(x => x))
                    {
                        matchingPeople.Append($"{eyeColor},\n");
                    }
                    matchingPeople.Remove(matchingPeople.Length - 2, 2);
                    return matchingPeople.ToString();
                }
                else
                {
                    // Return the only value.
                    string eyeColor = mostCommonEyeColors.First();
                    return eyeColor;
                }
            }
        }
    }
}
