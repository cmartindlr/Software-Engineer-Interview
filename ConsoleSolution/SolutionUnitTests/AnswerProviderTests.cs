using System.Text;

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
            Assert.IsTrue(int.Parse(answer.Replace("{", "").Replace("\"answer\":", "").Replace("}", "").Trim()) == result, "Count is not the correct count.");
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
        [DataRow(0, true, 0, true, null, null, null, null, null, null)]
        public void TestMostRecentStillActiveAnswerProvider(int? date1, bool? isActive1, 
                                                            int? date2, bool? isActive2, 
                                                            int? date3, bool? isActive3, 
                                                            int? date4, bool? isActive4, 
                                                            int? date5, bool? isActive5) 
        {
            List<List<RegisteredPerson>> peopleGroups = new List<List<RegisteredPerson>>();

            // Years
            DateTime? dateTime1 = date1 == null ? null : new DateTime(2000 + date1.Value, 1, 1);
            DateTime? dateTime2 = date2 == null ? null : new DateTime(2000 + date2.Value, 1, 1);
            DateTime? dateTime3 = date3 == null ? null : new DateTime(2000 + date3.Value, 1, 1);
            DateTime? dateTime4 = date4 == null ? null : new DateTime(2000 + date4.Value, 1, 1);
            DateTime? dateTime5 = date5 == null ? null : new DateTime(2000 + date5.Value, 1, 1);

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
            dateTime1 = date1 == null ? null : new DateTime(2000, 1 + date1.Value, 1);
            dateTime2 = date2 == null ? null : new DateTime(2000, 1 + date2.Value, 1);
            dateTime3 = date3 == null ? null : new DateTime(2000, 1 + date3.Value, 1);
            dateTime4 = date4 == null ? null : new DateTime(2000, 1 + date4.Value, 1);
            dateTime5 = date5 == null ? null : new DateTime(2000, 1 + date5.Value, 1);

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
            dateTime1 = date1 == null ? null : new DateTime(2000, 1, 1 + date1.Value);
            dateTime2 = date2 == null ? null : new DateTime(2000, 1, 1 + date2.Value);
            dateTime3 = date3 == null ? null : new DateTime(2000, 1, 1 + date3.Value);
            dateTime4 = date4 == null ? null : new DateTime(2000, 1, 1 + date4.Value);
            dateTime5 = date5 == null ? null : new DateTime(2000, 1, 1 + date5.Value);

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
                IEnumerable<RegisteredPerson> persons = people.Where(x => x.IsActive == true &&
                                                                          x.RegistrationDate != null)
                                                              .OrderByDescending(x => x.RegistrationDate);
                if(persons != null &&
                   persons.Any()) 
                {
                    DateTime maxDate = persons.Max(x => x.RegistrationDate.Value);
                    persons = persons.Where(x => x.RegistrationDate == maxDate);
                }

                string result;
                if(persons == null || 
                   !persons.Any()) 
                {
                    result = "{}";
                }
                else if(persons.Count() > 1)
                {
                    // Build a list of answers.
                    StringBuilder matchingPeople = new StringBuilder();
                    matchingPeople.Append("[");
                    foreach(RegisteredPerson person in persons.OrderBy(x => x.Name?.FormattedName ?? ""))
                    {
                        matchingPeople.Append($"\n  {{\n    \"answer\": {JsonConvert.SerializeObject(person, Formatting.Indented).Replace("\n", "\n    ")}\n  }},");
                    }
                    matchingPeople.Remove(matchingPeople.Length - 1, 1);
                    matchingPeople.Append("\n]");
                    result = matchingPeople.ToString();
                }
                else 
                {
                    result = "{\n  \"answer\": " + JsonConvert.SerializeObject(persons.First(), Formatting.Indented).Replace("\n", "\n  ") + "\n}";
                }

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
            correctAnswer = "{\n  \"answer\": {\n    \"apple\": 1,\n    \"banana\": 1\n  }\n}";
            resultingAnswer = answerProvider.ProvideAnswer(people);
            Assert.IsTrue(resultingAnswer == correctAnswer, "Incorrect return value for one null.");

            // All same fruit.
            people = new List<RegisteredPerson>()
            {
                new RegisteredPerson("apple" ,null, null, null, null, null, null, null, null, null, null, null, null, null, null),
                new RegisteredPerson("apple" ,null, null, null, null, null, null, null, null, null, null, null, null, null, null),
                new RegisteredPerson("apple" ,null, null, null, null, null, null, null, null, null, null, null, null, null, null)
            };
            correctAnswer = "{\n  \"answer\": {\n    \"apple\": 3\n  }\n}";
            resultingAnswer = answerProvider.ProvideAnswer(people);
            Assert.IsTrue(resultingAnswer == correctAnswer, "Incorrect return value for all same fruit.");

            // All different fruits.
            people = new List<RegisteredPerson>()
            {
                new RegisteredPerson("lemon" ,null, null, null, null, null, null, null, null, null, null, null, null, null, null),
                new RegisteredPerson("lime" ,null, null, null, null, null, null, null, null, null, null, null, null, null, null),
                new RegisteredPerson("orange" ,null, null, null, null, null, null, null, null, null, null, null, null, null, null)
            };
            correctAnswer = "{\n  \"answer\": {\n    \"lemon\": 1,\n    \"lime\": 1,\n    \"orange\": 1\n  }\n}";
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
            correctAnswer = "{\n  \"answer\": {\n    \"apple\": 3,\n    \"cherry\": 2,\n    \"lime\": 1,\n    \"raspberry\": 3,\n    \"strawberry\": 1\n  }\n}";
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
        [DataRow("brown", "brown", "blue", "blue", "green")]
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
            string correctAnswer;
            IEnumerable<IGrouping<string, RegisteredPerson>> eyeColorGroups = people.Where(x => x.EyeColor != null)
                                                                                    .GroupBy(x => x.EyeColor ?? "");
            if(eyeColorGroups != null &&
               eyeColorGroups.Any())
            {
                int maxCount = eyeColorGroups.Max(x => x.Count());
                IEnumerable<string> eyeColors = eyeColorGroups.Where(x => x.Count() == maxCount)
                                                              .Select(x => x.Key);

                if(eyeColors == null ||
                   !eyeColors.Any())
                {
                    correctAnswer = "{}";
                }
                else if(eyeColors.Count() > 1)
                {
                    // Build a list of answers.
                    StringBuilder matchingPeople = new StringBuilder();
                    matchingPeople.Append("[");
                    foreach(string eyeColor in eyeColors.OrderBy(x => x))
                    {
                        matchingPeople.Append($"\n  {{\n    \"answer\": \"{eyeColor}\"\n  }},");
                    }
                    matchingPeople.Remove(matchingPeople.Length - 1, 1);
                    matchingPeople.Append("\n]");
                    correctAnswer = matchingPeople.ToString();
                }
                else 
                {
                    string actualEyeColor = eyeColors.First();
                    correctAnswer = "{\n  \"answer\": \"" + actualEyeColor + "\"\n}";
                }
            }
            else 
            {
                correctAnswer = "{}";
            }

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
            correctAnswer = "{\n  \"answer\": \"" + 0.00M.ToString("C") + "\"\n}";
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
            correctAnswer = "{\n  \"answer\": \"" + 0.00M.ToString("C") + "\"\n}";
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
            correctAnswer = "{\n  \"answer\": \"" + 81.39M.ToString("C") + "\"\n}";
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
            correctAnswer = "{\n  \"answer\": \"" + 5.00M.ToString("C") + "\"\n}";
            result = answerProvider.ProvideAnswer(people);
            Assert.IsTrue(result == correctAnswer, "Incorrect answer for all balances.");
        }

        /// <summary>
        /// Tests the full name answer provider.
        /// </summary>
        [TestMethod]
        public void TestFullNameAnswerProvider() 
        {
            RegisteredPerson namedPerson = new RegisteredPerson(null, null, null, null, null, null, null, null, null, 
                                                                new RegisteredName("Carson", "Tyler"), 
                                                                null, null, null, null, "5aabbca3e58dc67745d720b1");
            RegisteredPerson unnamedPerson = new RegisteredPerson(null, null, null, null, null, null, null, null, null, 
                                                                  null, null, null, null, null, "5aabbca3e58dc67745d720b1");
            RegisteredPerson emptyPerson = new RegisteredPerson(null, null, null, null, null, null, null, null, null,
                                                                  null, null, null, null, null, null);

            // ID is present and single person in list.
            List<RegisteredPerson> people = new List<RegisteredPerson>()
            {
                namedPerson
            };
            string correctAnswer = "{\n  \"answer\": \"" + namedPerson.Name.FormattedName + "\"\n}";
            IAnswerProvider<RegisteredPerson> answerProvider = new FullNameAnswerProvider();
            string result = answerProvider.ProvideAnswer(people);
            Assert.IsTrue(result == correctAnswer, "Incorrect when only person with correct ID is in list.");

            // ID is present for multiple people in the list.
            people = new List<RegisteredPerson>()
            {
                namedPerson,
                namedPerson
            };
            correctAnswer = "[\n  {\n    \"answer\": \"" + namedPerson.Name.FormattedName + "\"\n  },\n  {\n    \"answer\": \"" + namedPerson.Name.FormattedName + "\"\n  }\n]";
            result = answerProvider.ProvideAnswer(people);
            Assert.IsTrue(result == correctAnswer, $"Incorrect when multiple people with correct ID are in list.\n{correctAnswer}\n{result}");

            // ID is present and several people in list.
            people = new List<RegisteredPerson>()
            {
                namedPerson,
                emptyPerson,
                emptyPerson,
                emptyPerson
            };
            correctAnswer = "{\n  \"answer\": \"" + namedPerson.Name.FormattedName + "\"\n}";
            result = answerProvider.ProvideAnswer(people);
            Assert.IsTrue(result == correctAnswer, "Incorrect when person with correct ID is in list with others.");

            // ID is present and no name.
            people = new List<RegisteredPerson>()
            {
                emptyPerson,
                emptyPerson,
                emptyPerson,
                unnamedPerson
            };
            correctAnswer = "{}";
            result = answerProvider.ProvideAnswer(people);
            Assert.IsTrue(result == correctAnswer, "Incorrect when person with correct ID is in list without name.");

            // ID is not present, but list in not empty.
            people = new List<RegisteredPerson>()
            {
                emptyPerson,
                emptyPerson,
                emptyPerson
            };
            correctAnswer = "{}";
            result = answerProvider.ProvideAnswer(people);
            Assert.IsTrue(result == correctAnswer, "Incorrect when person with correct ID is not in list.");

            // ID is not present and list is empty.
            people = new List<RegisteredPerson>();
            correctAnswer = "{}";
            result = answerProvider.ProvideAnswer(people);
            Assert.IsTrue(result == correctAnswer, "Incorrect when no one is in list.");
        }
    }
}
