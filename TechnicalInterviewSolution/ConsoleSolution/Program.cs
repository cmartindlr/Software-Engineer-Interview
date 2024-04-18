// Credit newtonsoft. https://www.newtonsoft.com/json
using Newtonsoft.Json;

using Backend.Models.Json;
using Backend.Interfaces;
using Backend.Objects.AnswerProviders;
using Backend.Objects;
using Backend.Models.Sql;

namespace ConsoleSolution
{
    internal class Program
    {
        /// <summary>
        /// The name of the file to read.
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
            List<(string Question, string Answer)> results = answerAggregator.AggregateAnswers(people).ToList();

            // Print off each answer. Create a record of the answer for later.
            int questionNumber = 1;
            foreach((string Question, string Answer) result in results)
            {
                // Print out the result.
                Console.Write(questionNumber + ". " + result.Question + "\n" + result.Answer + "\n\n");
                questionNumber++;
            }

            // Save the answers.
            IAnswerRecordDataManager answerRecordDataManager = new AnswerRecordDataManager(new AnswerContextFactory());
            answerRecordDataManager.Save(results.Select(x => new AnswerRecord()
            {
                AnswerDate = DateTime.Now,
                FileName = "data.json",
                Question = x.Question,
                Answer = x.Answer
            }));
        }
    }
}