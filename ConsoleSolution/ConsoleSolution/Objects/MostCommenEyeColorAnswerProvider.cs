using ConsoleSolution.Interfaces;
using ConsoleSolution.Models.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSolution.Objects
{
    /// <summary>
    /// Provider of the answer to the question of "What is the most common eye color?"
    /// </summary>
    public class MostCommenEyeColorAnswerProvider : IAnswerProvider<RegisteredPerson>
    {
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
            string? mostCommmonEyeColor = data.Where(x => x.EyeColor != null) // Filter out any with no eye color.
                                              .GroupBy(x => x.EyeColor)
                                              .OrderByDescending(x => x.Count()) // Get the largest group first.
                                              .Select(x => x.Key ?? "") // Only care about eye color.
                                              .FirstOrDefault();

            if(mostCommmonEyeColor == null)
            {
                // If no result, return an empty JSON.
                return "{}";
            }
            else 
            {
                return "{\n  \"eyeColor\": \"" + mostCommmonEyeColor + "\"\n}";
            }
        }
    }
}
