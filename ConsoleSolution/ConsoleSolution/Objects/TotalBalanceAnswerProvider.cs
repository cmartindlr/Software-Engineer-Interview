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
    /// Provider of the answer to "What is the total balance of all individuals combined?"
    /// </summary>
    public class TotalBalanceAnswerProvider : IAnswerProvider<RegisteredPerson>
    {
        // This follows the Liskov substitution principle of SOLID by allowing this provider to be used wherever an answer provider is desired.

        public string Question => "What is the total balance of all individuals combined?";

        /// <summary>
        /// Gets the total balance of all individuals as a JSON.
        /// </summary>
        /// <param name="data">
        /// The set of data to analyze.
        /// </param>
        /// <returns>
        /// A JSON containing the total balance of all individuals.
        /// </returns>
        public string ProvideAnswer(IEnumerable<RegisteredPerson> data)
        {
            // Simple sum of all fields, 0 if not present.
            return "{\n  \"totalBalance\": \"" + (data.Any() ? data.Sum(x => x.NumericBalance ?? 0.0M) : 0.0M).ToString("C") + "\"\n}";
        }
    }
}
