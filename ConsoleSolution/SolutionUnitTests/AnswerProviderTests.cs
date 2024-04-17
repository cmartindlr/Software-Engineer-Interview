using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionUnitTests
{
    [TestClass]
    public class AnswerProviderTests
    {
        /// <summary>
        /// Tests if all answer providers return JSONs.
        /// </summary>
        [TestMethod]
        public void TestIfValidJson() 
        {
            // Get the types derived from IAnswerProvider.
            IEnumerable<Type> answerProviders = AppDomain.CurrentDomain.GetAssemblies()
                                                                       .SelectMany(x => x.GetTypes())
                                                                       .Where(x => typeof(IAnswerProvider<RegisteredPerson>).IsAssignableFrom(x));

            // Loop and instantiate each, ensuring that empty person and valid person both produce valid JSONs.
            RegisteredPerson emptyPerson = new RegisteredPerson(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
            RegisteredPerson validPerson = new RegisteredPerson("a", "b", 1.0, 2.0, DateTime.Now, "c", "d", "e", "f", new RegisteredName("g", "h"), "i", 1, "$1.00", false, "j");
            foreach(Type answerProviderType in answerProviders) 
            { 
                IAnswerProvider<RegisteredPerson> answerProvider = (IAnswerProvider<RegisteredPerson>)Activator.CreateInstance(answerProviderType);
                Assert.IsTrue(JsonConvert.DeserializeObject(answerProvider.ProvideAnswer(new List<RegisteredPerson>())) != null, "No answer for empty list.");
                Assert.IsTrue(JsonConvert.DeserializeObject(answerProvider.ProvideAnswer(new List<RegisteredPerson>() { emptyPerson })) != null, "No answer for empty person.");
                Assert.IsTrue(JsonConvert.DeserializeObject(answerProvider.ProvideAnswer(new List<RegisteredPerson>() { validPerson })) != null, "No answer for valid person.");
                Assert.IsTrue(JsonConvert.DeserializeObject(answerProvider.ProvideAnswer(new List<RegisteredPerson>() { emptyPerson, validPerson })) != null, "No answer for combination.");
            }
        }

        /// <summary>
        /// Tests whether the greater age answer provider gives the correct count.
        /// </summary>
        [TestMethod]
        [DataRow(1, 2, 3, 4, 5)]
        [DataRow(1, 2, 3, 4, 50)]
        [DataRow(1, 2, 3, 4, 51)]
        [DataRow(1, 2, 3, 4, 5)]
        [DataRow(1, 2, 3, 4, 5)]
        [DataRow(80, 23, 65, 45, 75)]
        [DataRow(null, null, null, null, null)]
        [DataRow(null, 1, null, null, null)]
        [DataRow(null, null, 51, null, null)]
        public void TestOver50AnswerProvider(int age1, int age2, int age3, int age4, int age5)
        {
            List<int> ages = new List<int> { age1, age2, age3, age4, age5 };

            // Find the expected number.
            int result = ages.Count(x => x > 50);

            // Produce the objects.
            List<RegisteredPerson> people = new List<RegisteredPerson>() 
            { 
                new RegisteredPerson(null, null, null, null, null, null, null, null, null, null, null, age1, null, null, null),
                new RegisteredPerson(null, null, null, null, null, null, null, null, null, null, null, age2, null, null, null),
                new RegisteredPerson(null, null, null, null, null, null, null, null, null, null, null, age3, null, null, null),
                new RegisteredPerson(null, null, null, null, null, null, null, null, null, null, null, age4, null, null, null),
                new RegisteredPerson(null, null, null, null, null, null, null, null, null, null, null, age5, null, null, null) 
            };

            // Run with the answer provider.
            Over50AnswerProvider ageAnswerProvider = new Over50AnswerProvider();
            string answer = ageAnswerProvider.ProvideAnswer(people);

            // Check if the answer matches the correct count.
            Assert.IsTrue(int.Parse(answer.Replace("{", "").Replace("\"count\":", "").Replace("}", "").Trim()) == result, "Count is not the correct count.");
        }

        /// <summary>
        /// Tests whether the most recently active user is returned as JSON.
        /// </summary>
        [TestMethod]
        [DataRow(null, null, null, null, null, null, null, null, null, null)]
        [DataRow(null, false, null, false, null, false, null, false, null, false)]
        [DataRow(null, true, null, true, null, true, null, true, null, true)]
        [DataRow(1, null, 2, null, 3, null, 4, null, 5, null)]
        [DataRow(1, true, 2, true, 3, true, 4, true, 5, true)]
        [DataRow(1, false, 2, false, 3, false, 4, false, 5, false)]
        [DataRow(5, true, 7, false, 2, true, 6, false, 1, true)]
        public void TestMostRecentStillActiveAnswerProvider(int date1, bool isActive1, 
                                                            int date2, bool isActive2, 
                                                            int date3, bool isActive3, 
                                                            int date4, bool isActive4, 
                                                            int date5, bool isActive5) 
        {
            List<List<RegisteredPerson>> peopleGroups = new List<List<RegisteredPerson>>();

            // Years
            DateTime dateTime1 = new DateTime(2000 + date1, 1, 1);
            DateTime dateTime2 = new DateTime(2000 + date2, 1, 1);
            DateTime dateTime3 = new DateTime(2000 + date3, 1, 1);
            DateTime dateTime4 = new DateTime(2000 + date4, 1, 1);
            DateTime dateTime5 = new DateTime(2000 + date5, 1, 1);

            // Create people.
            peopleGroups.Add(new List<RegisteredPerson>()
            {
                new RegisteredPerson(null, null, null, null, dateTime1, null, null, null, null, null, null, null, null, isActive1, null),
                new RegisteredPerson(null, null, null, null, dateTime2, null, null, null, null, null, null, null, null, isActive2, null),
                new RegisteredPerson(null, null, null, null, dateTime3, null, null, null, null, null, null, null, null, isActive3, null),
                new RegisteredPerson(null, null, null, null, dateTime4, null, null, null, null, null, null, null, null, isActive4, null),
                new RegisteredPerson(null, null, null, null, dateTime5, null, null, null, null, null, null, null, null, isActive5, null)
            });

            // Months
            dateTime1 = new DateTime(2000, 1 + date1, 1);
            dateTime2 = new DateTime(2000, 1 + date2, 1);
            dateTime3 = new DateTime(2000, 1 + date3, 1);
            dateTime4 = new DateTime(2000, 1 + date4, 1);
            dateTime5 = new DateTime(2000, 1 + date5, 1);

            // Create people.
            peopleGroups.Add(new List<RegisteredPerson>()
            {
                new RegisteredPerson(null, null, null, null, dateTime1, null, null, null, null, null, null, null, null, isActive1, null),
                new RegisteredPerson(null, null, null, null, dateTime2, null, null, null, null, null, null, null, null, isActive2, null),
                new RegisteredPerson(null, null, null, null, dateTime3, null, null, null, null, null, null, null, null, isActive3, null),
                new RegisteredPerson(null, null, null, null, dateTime4, null, null, null, null, null, null, null, null, isActive4, null),
                new RegisteredPerson(null, null, null, null, dateTime5, null, null, null, null, null, null, null, null, isActive5, null)
            });


            // Days
            dateTime1 = new DateTime(2000, 1, 1 + date1);
            dateTime2 = new DateTime(2000, 1, 1 + date2);
            dateTime3 = new DateTime(2000, 1, 1 + date3);
            dateTime4 = new DateTime(2000, 1, 1 + date4);
            dateTime5 = new DateTime(2000, 1, 1 + date5);

            // Create people.
            peopleGroups.Add(new List<RegisteredPerson>()
            {
                new RegisteredPerson(null, null, null, null, dateTime1, null, null, null, null, null, null, null, null, isActive1, null),
                new RegisteredPerson(null, null, null, null, dateTime2, null, null, null, null, null, null, null, null, isActive2, null),
                new RegisteredPerson(null, null, null, null, dateTime3, null, null, null, null, null, null, null, null, isActive3, null),
                new RegisteredPerson(null, null, null, null, dateTime4, null, null, null, null, null, null, null, null, isActive4, null),
                new RegisteredPerson(null, null, null, null, dateTime5, null, null, null, null, null, null, null, null, isActive5, null)
            });

            foreach(List<RegisteredPerson> people in peopleGroups)
            {
                // Get most recent person.
                RegisteredPerson? person = people.Where(x => x.IsActive == true &&
                                                             x.RegistrationDate != null)
                                                 .OrderByDescending(x => x.RegistrationDate)
                                                 .FirstOrDefault();
                string result = person == null ? "{}" : JsonConvert.SerializeObject(person, Formatting.Indented);

                // Get the provided answer.
                IAnswerProvider<RegisteredPerson> answerProvider = new MostRecentStillActiveAnswerProvider();
                string answer = answerProvider.ProvideAnswer(people);

                // Compare the results.
                Assert.IsTrue(result == answer, "Answer provided did not match.\n" + result + "\n" + answer);
            }
        }

        /// <summary>
        /// Tests whether the correct count of favorite fruits is provided.
        /// </summary>
        [TestMethod]
        public void TestFavoriteFruitCountsAnswerProvider() 
        {
            IAnswerProvider<RegisteredPerson> answerProvider = new FavoriteFruitsCountsAnswerProvider();

            // All null fruits.
            List<RegisteredPerson> people = new List<RegisteredPerson>()
            {
                new RegisteredPerson(null ,null, null, null, null, null, null, null, null, null, null, null, null, null, null),
                new RegisteredPerson(null ,null, null, null, null, null, null, null, null, null, null, null, null, null, null),
                new RegisteredPerson(null ,null, null, null, null, null, null, null, null, null, null, null, null, null, null)
            };
            string correctAnswer = "{}";
            string resultingAnswer = answerProvider.ProvideAnswer(people);
            Assert.IsTrue(resultingAnswer == correctAnswer, "Incorrect return value for all nulls.");

            // At least one null fruit.
            people = new List<RegisteredPerson>()
            {
                new RegisteredPerson("apple" ,null, null, null, null, null, null, null, null, null, null, null, null, null, null),
                new RegisteredPerson("banana" ,null, null, null, null, null, null, null, null, null, null, null, null, null, null),
                new RegisteredPerson(null ,null, null, null, null, null, null, null, null, null, null, null, null, null, null)
            };
            correctAnswer = "{\n  \"apple\": 1,\n  \"banana\": 1\n}";
            resultingAnswer = answerProvider.ProvideAnswer(people);
            Assert.IsTrue(resultingAnswer == correctAnswer, "Incorrect return value for one null.");

            // All same fruit.
            people = new List<RegisteredPerson>()
            {
                new RegisteredPerson("apple" ,null, null, null, null, null, null, null, null, null, null, null, null, null, null),
                new RegisteredPerson("apple" ,null, null, null, null, null, null, null, null, null, null, null, null, null, null),
                new RegisteredPerson("apple" ,null, null, null, null, null, null, null, null, null, null, null, null, null, null)
            };
            correctAnswer = "{\n  \"apple\": 3\n}";
            resultingAnswer = answerProvider.ProvideAnswer(people);
            Assert.IsTrue(resultingAnswer == correctAnswer, "Incorrect return value for all same fruit.");

            // All different fruits.
            people = new List<RegisteredPerson>()
            {
                new RegisteredPerson("lemon" ,null, null, null, null, null, null, null, null, null, null, null, null, null, null),
                new RegisteredPerson("lime" ,null, null, null, null, null, null, null, null, null, null, null, null, null, null),
                new RegisteredPerson("orange" ,null, null, null, null, null, null, null, null, null, null, null, null, null, null)
            };
            correctAnswer = "{\n  \"lemon\": 1,\n  \"lime\": 1,\n  \"orange\": 1\n}";
            resultingAnswer = answerProvider.ProvideAnswer(people);
            Assert.IsTrue(resultingAnswer == correctAnswer, "Incorrect return value for all different.");

            // Variation.
            people = new List<RegisteredPerson>()
            {
                new RegisteredPerson("raspberry" ,null, null, null, null, null, null, null, null, null, null, null, null, null, null),
                new RegisteredPerson("raspberry" ,null, null, null, null, null, null, null, null, null, null, null, null, null, null),
                new RegisteredPerson("strawberry" ,null, null, null, null, null, null, null, null, null, null, null, null, null, null),
                new RegisteredPerson("raspberry" ,null, null, null, null, null, null, null, null, null, null, null, null, null, null),
                new RegisteredPerson("lime" ,null, null, null, null, null, null, null, null, null, null, null, null, null, null),
                new RegisteredPerson("cherry" ,null, null, null, null, null, null, null, null, null, null, null, null, null, null),
                new RegisteredPerson("cherry" ,null, null, null, null, null, null, null, null, null, null, null, null, null, null),
                new RegisteredPerson("apple" ,null, null, null, null, null, null, null, null, null, null, null, null, null, null),
                new RegisteredPerson("apple" ,null, null, null, null, null, null, null, null, null, null, null, null, null, null),
                new RegisteredPerson("apple" ,null, null, null, null, null, null, null, null, null, null, null, null, null, null)
            };
            correctAnswer = "{\n  \"apple\": 3,\n  \"cherry\": 2,\n  \"lime\": 1,\n  \"raspberry\": 3,\n  \"strawberry\": 1\n}";
            resultingAnswer = answerProvider.ProvideAnswer(people);
            Assert.IsTrue(resultingAnswer == correctAnswer, "Incorrect return value for variation.");
        }

        /// <summary>
        /// Tests the most common eye color answer provider.
        /// </summary>
        [TestMethod]
        [DataRow(null, null, null, null, null)]
        [DataRow(null, null, null, null, "blue")]
        [DataRow(null, null, null, "blue", "blue")]
        [DataRow(null, null, "brown", "blue", "blue")]
        [DataRow(null, null, "brown", "brown", "blue")]
        [DataRow(null, null, "blue", "brown", "blue")]
        [DataRow("blue", "green", "blue", "brown", "blue")]
        public void TestMostCommonEyeColorAnswerProvider(string? eyeColor1, string? eyeColor2, string eyeColor3, string eyeColor4, string eyeColor5)
        {
            // Make the people.
            List<RegisteredPerson> people = new List<RegisteredPerson>()
            {
                new RegisteredPerson(null ,null, null, null, null, null, null, null, null, null, eyeColor1, null, null, null, null),
                new RegisteredPerson(null ,null, null, null, null, null, null, null, null, null, eyeColor2, null, null, null, null),
                new RegisteredPerson(null ,null, null, null, null, null, null, null, null, null, eyeColor3, null, null, null, null),
                new RegisteredPerson(null ,null, null, null, null, null, null, null, null, null, eyeColor4, null, null, null, null),
                new RegisteredPerson(null ,null, null, null, null, null, null, null, null, null, eyeColor5, null, null, null, null)
            };

            // Get the most common eye color.
            string? actualEyeColor = people.Where(x => x.EyeColor != null)
                                           .GroupBy(x => x.EyeColor)
                                           .OrderByDescending(x => x.Count())
                                           .Select(x => x.Key)
                                           .FirstOrDefault();
            string correctAnswer = actualEyeColor == null ? "{}" : "{\n  \"eyeColor\": \"" + actualEyeColor + "\"\n}";

            // Get the result.
            IAnswerProvider<RegisteredPerson> answerProvider = new MostCommenEyeColorAnswerProvider();
            string result = answerProvider.ProvideAnswer(people);

            // Compare results.
            Assert.IsTrue(result == correctAnswer, "Incorrect most common eye color.");
        }

        /// <summary>
        /// Tests the total balance answer provider.
        /// </summary>
        [TestMethod]
        public void TestTotalBalanceAnswerProvider() 
        {
            IAnswerProvider<RegisteredPerson> answerProvider = new TotalBalanceAnswerProvider();
            string correctAnswer;
            string result;
            List<RegisteredPerson> people;

            // Empty list.
            people = new List<RegisteredPerson>();
            correctAnswer = "{\n  \"totalBalance\": \"" + 0.00M.ToString("C") + "\"\n}";
            result = answerProvider.ProvideAnswer(people);
            Assert.IsTrue(result == correctAnswer, "Incorrect answer for empty list.");

            // No balances.
            people = new List<RegisteredPerson>()
            {
                new RegisteredPerson(null ,null, null, null, null, null, null, null, null, null, null, null, null, null, null),
                new RegisteredPerson(null ,null, null, null, null, null, null, null, null, null, null, null, null, null, null),
                new RegisteredPerson(null ,null, null, null, null, null, null, null, null, null, null, null, null, null, null),
                new RegisteredPerson(null ,null, null, null, null, null, null, null, null, null, null, null, null, null, null),
                new RegisteredPerson(null ,null, null, null, null, null, null, null, null, null, null, null, null, null, null)
            };
            correctAnswer = "{\n  \"totalBalance\": \"" + 0.00M.ToString("C") + "\"\n}";
            result = answerProvider.ProvideAnswer(people);
            Assert.IsTrue(result == correctAnswer, "Incorrect answer for no balance.");


            // Some balances.
            people = new List<RegisteredPerson>()
            {
                new RegisteredPerson(null ,null, null, null, null, null, null, null, null, null, null, null, 20.00M.ToString("C"), null, null),
                new RegisteredPerson(null ,null, null, null, null, null, null, null, null, null, null, null, null, null, null),
                new RegisteredPerson(null ,null, null, null, null, null, null, null, null, null, null, null, 55.40M.ToString("C"), null, null),
                new RegisteredPerson(null ,null, null, null, null, null, null, null, null, null, null, null, null, null, null),
                new RegisteredPerson(null ,null, null, null, null, null, null, null, null, null, null, null, 5.99M.ToString("C"), null, null)
            };
            correctAnswer = "{\n  \"totalBalance\": \"" + 81.39M.ToString("C") + "\"\n}";
            result = answerProvider.ProvideAnswer(people);
            Assert.IsTrue(result == correctAnswer, "Incorrect answer for some balances.");

            // All balances.
            people = new List<RegisteredPerson>()
            {
                new RegisteredPerson(null ,null, null, null, null, null, null, null, null, null, null, null, 1.00M.ToString("C"), null, null),
                new RegisteredPerson(null ,null, null, null, null, null, null, null, null, null, null, null, 1.00M.ToString("C"), null, null),
                new RegisteredPerson(null ,null, null, null, null, null, null, null, null, null, null, null, 1.00M.ToString("C"), null, null),
                new RegisteredPerson(null ,null, null, null, null, null, null, null, null, null, null, null, 1.00M.ToString("C"), null, null),
                new RegisteredPerson(null ,null, null, null, null, null, null, null, null, null, null, null, 1.00M.ToString("C"), null, null)
            };
            correctAnswer = "{\n  \"totalBalance\": \"" + 5.00M.ToString("C") + "\"\n}";
            result = answerProvider.ProvideAnswer(people);
            Assert.IsTrue(result == correctAnswer, "Incorrect answer for all balances.");
        }
    }
}
