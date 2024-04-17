// Credit newtonsoft. https://www.newtonsoft.com/json
using Newtonsoft.Json;

using ConsoleSolution.Models.Json;
using ConsoleSolution.Interfaces;
using ConsoleSolution.Objects.AnswerProviders;
using ConsoleSolution.Objects;
using ConsoleSolution.Models.Sql;
using Microsoft.EntityFrameworkCore;

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
            IAnswerAggregator<RegisteredPerson> answerAggregator = new MultithreadedAnswerAggregator<RegisteredPerson>(Program._answerProviders);
            int questionNumber = 1;
            List<string> results = answerAggregator.AggregateAnswers(people).ToList();
            using(AnswerContext database = new AnswerContext())
            {
                foreach(string result in results) 
                {
                    // Add the result to save.
                    database.AnswerRecords.Add(new AnswerRecord() 
                    { 
                        AnswerDate = DateTime.Now,
                        FileName = "data.json",
                        Question = Program._answerProviders[questionNumber - 1].Question,
                        Answer = result
                    });

                    // Print out the result.
                    Console.Write(questionNumber + ". " + Program._answerProviders[questionNumber - 1].Question + "\n" + result + "\n\n");
                    questionNumber++;
                }

                // Save results.
                database.SaveChanges();
            }
        }
    }
}