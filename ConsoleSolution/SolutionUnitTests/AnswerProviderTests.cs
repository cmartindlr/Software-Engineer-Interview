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
        /// Tests whether the greater age answer provider gives the correct count.
        /// </summary>
        /// <param name="age">
        /// The threshold for the age.
        /// </param>
        /// <param name="age1">
        /// The first age in the list.
        /// </param>
        /// <param name="age2">
        /// The second age in the list.
        /// </param>
        /// <param name="age3">
        /// The third age in the list.
        /// </param>
        /// <param name="age4">
        /// The fourth age in the list.
        /// </param>
        /// <param name="age5">
        /// The fifth age in the list.
        /// </param>
        [TestMethod]
        [DataRow(1, 2, 3, 4, 5)]
        [DataRow(1, 2, 3, 4, 50)]
        [DataRow(1, 2, 3, 4, 51)]
        [DataRow(1, 2, 3, 4, 5)]
        [DataRow(1, 2, 3, 4, 5)]
        [DataRow(80, 23, 65, 45, 75)]
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
            Assert.IsTrue(int.Parse(answer) == result, "Count is not the correct count.");
        }
    }
}
