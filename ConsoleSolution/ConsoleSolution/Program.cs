// Credit newtonsoft.
using Newtonsoft.Json;

using ConsoleSolution.Models.Json;
using ConsoleSolution.Interfaces;
using ConsoleSolution.Objects;

namespace ConsoleSolution
{
    internal class Program
    {
        /// <summary>
        /// The providers of answers.
        /// </summary>
        static List<IAnswerProvider<RegisteredPerson>> _answerProviders = new List<IAnswerProvider<RegisteredPerson>>() 
        { 
            new Over50AnswerProvider(),
            new MostRecentStillActiveAnswerProvider(),
            new FavoriteFruitsCountsAnswerProvider(),
            new MostCommenEyeColorAnswerProvider(),
            new TotalBalanceAnswerProvider(),
            new FullNameAnswerProvider()
        };

        /// <summary>
        /// Runs the program.
        /// </summary>
        static void Main()
        {
            // The collection of people.
            List<RegisteredPerson> people;

            // Deserialize the entire json.
            using(JsonReader reader = new JsonTextReader(File.OpenText("data.json")))
            {
                JsonSerializer deserializer = new JsonSerializer();
                people = deserializer.Deserialize<List<RegisteredPerson>>(reader) ?? new List<RegisteredPerson>();
            }

            // Get the answer for each question.
            int questionNumber = 1;
            foreach(IAnswerProvider<RegisteredPerson> answerProvider in Program._answerProviders) 
            {
                Console.Write(questionNumber + ". " + answerProvider.Question + "\n" + (answerProvider.ProvideAnswer(people) ?? "null") + "\n\n");
                questionNumber++;
            }
        }
    }
}