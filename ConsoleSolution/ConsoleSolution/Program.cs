// Credit newtonsoft. https://www.newtonsoft.com/json
using Newtonsoft.Json;

using ConsoleSolution.Models.Json;
using ConsoleSolution.Interfaces;
using ConsoleSolution.Objects.AnswerProviders;
using ConsoleSolution.Objects;
using ConsoleSolution.Models.Sql;

namespace ConsoleSolution
{
    internal class Program
    {
        /// <summary>
        /// THe name of the file to read.
        /// </summary>
        static string _fileName = "data.json";

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
            using(JsonReader reader = new JsonTextReader(File.OpenText(Program._fileName)))
            {
                JsonSerializer deserializer = new JsonSerializer();
                people = deserializer.Deserialize<List<RegisteredPerson>>(reader) ?? new List<RegisteredPerson>();
            }

            // Get the answer for each question.
            IAnswerAggregator<RegisteredPerson> answerAggregator = new MultithreadedAnswerAggregator<RegisteredPerson>(Program._answerProviders);
            List<string> results = answerAggregator.AggregateAnswers(people).ToList();

            // Print off each answer. Create a record of the answer for later.
            int questionNumber = 1;
            foreach(string result in results) 
            {
                // Print out the result.
                Console.Write(questionNumber + ". " + Program._answerProviders[questionNumber - 1].Question + "\n" + result + "\n\n");
                questionNumber++;
            }

            // Save the answers.
            IAnswerRecordDataManager answerRecordDataManager = new AnswerRecordDataManager(new AnswerContextFactory());
            answerRecordDataManager.Save(results.Select(x => new AnswerRecord()
            {
                AnswerDate = DateTime.Now,
                FileName = "data.json",
                Question = Program._answerProviders[results.IndexOf(x)].Question,
                Answer = x
            }));
        }
    }
}