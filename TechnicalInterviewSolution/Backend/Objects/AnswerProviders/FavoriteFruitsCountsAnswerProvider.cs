﻿using Backend.Interfaces;
using Backend.Models.Json;
using System.Text;

namespace Backend.Objects.AnswerProviders
{
    /// <summary>
    /// Provides answers to the question of the number of people choosing each favorite fruit.
    /// </summary>
    public class FavoriteFruitsCountsAnswerProvider : IAnswerProvider<RegisteredPerson>
    {
        // This follows the Liskov substitution principle of SOLID by allowing this provider to be used wherever an answer provider is desired.

        /// <summary>
        /// The question this class answers.
        /// </summary>
        private static string _question { get; } = "What are the counts of each favorite fruit?";

        /// <summary>
        /// Gets every combination of fruits and counts.
        /// </summary>
        /// <param name="data">
        /// The data to analyze.
        /// </param>
        /// <returns>
        /// Every combination of fruits and counts in alphabetical order as a JSON.
        /// </returns>
        public (string Question, string Answer) ProvideAnswer(IEnumerable<RegisteredPerson> data)
        {
            // Get the fruit pair.
            IEnumerable<(string Fruit, int Count)> fruitCounts = data.Where(x => x.FavoriteFruit != null) // Ignore unspecified favorites.
                                                                     .GroupBy(x => x.FavoriteFruit)
                                                                     .Select(x => (x.Key ?? "", x.Count())); // Get pair of fruit and number of occurrences.

            if (fruitCounts == null ||
               !fruitCounts.Any())
            {
                // If no answer, provide empty JSON.
                return (FavoriteFruitsCountsAnswerProvider._question, "NULL");
            }
            else
            {
                // Return a json where each fruit is a number.
                StringBuilder result = new StringBuilder();
                foreach ((string Fruit, int Count) fruitCount in fruitCounts.OrderBy(x => x.Fruit))
                {
                    result.Append(fruitCount.Fruit + ": " + fruitCount.Count + ",\n");
                }
                result.Remove(result.Length - 2, 2); // Remove last comma and newline.

                return (FavoriteFruitsCountsAnswerProvider._question, result.ToString());
            }
        }
    }
}
