

namespace SolutionUnitTests
{
    [TestClass]
    public class MultithreadedAnswerAggregatorTests
    {
        /// <summary>
        /// Tests the single-threaded case.
        /// </summary>
        /// <param name="size">
        /// The size of the data set.
        /// </param>
        /// <param name="threshold">
        /// The threshold when threading begins.
        /// </param>
        [TestMethod]
        [DataRow(int.MaxValue, 1)]
        [DataRow(int.MaxValue, 5)]
        [DataRow(int.MaxValue, 10)]
        [DataRow(int.MaxValue, 50)]
        [DataRow(int.MaxValue, 100)]
        [DataRow(int.MaxValue, 500)]
        [DataRow(int.MaxValue, 1000)]
        [DataRow(int.MaxValue, 5000)]
        [DataRow(int.MaxValue, 10000)]
        [DataRow(0, 1)]
        [DataRow(0, 5)]
        [DataRow(0, 10)]
        [DataRow(0, 50)]
        [DataRow(0, 100)]
        [DataRow(0, 500)]
        [DataRow(0, 1000)]
        [DataRow(0, 5000)]
        [DataRow(0, 10000)]
        public void TestAnswerAggregator(int threshold, int size) 
        {
            // Set up mocks.
            string result1 = "Mock1";
            string result2 = "Mock2";
            string result3 = "Mock3";
            string result4 = "Mock4";
            string result5 = "Mock5";
            Mock<IAnswerProvider<int>> mock1 = new Mock<IAnswerProvider<int>>();
            mock1.Setup(x => x.ProvideAnswer(It.IsAny<IEnumerable<int>>())).Returns(result1);
            Mock<IAnswerProvider<int>> mock2 = new Mock<IAnswerProvider<int>>();
            mock2.Setup(x => x.ProvideAnswer(It.IsAny<IEnumerable<int>>())).Returns(result2);
            Mock<IAnswerProvider<int>> mock3 = new Mock<IAnswerProvider<int>>();
            mock3.Setup(x => x.ProvideAnswer(It.IsAny<IEnumerable<int>>())).Returns(result3);
            Mock<IAnswerProvider<int>> mock4 = new Mock<IAnswerProvider<int>>();
            mock4.Setup(x => x.ProvideAnswer(It.IsAny<IEnumerable<int>>())).Returns(result4);
            Mock<IAnswerProvider<int>> mock5 = new Mock<IAnswerProvider<int>>();
            mock5.Setup(x => x.ProvideAnswer(It.IsAny<IEnumerable<int>>())).Returns(result5);
            List<Mock<IAnswerProvider<int>>> mocks = new List<Mock<IAnswerProvider<int>>>()
            {
                mock1, mock2, mock3, mock4, mock5
            };

            // Set up answer aggregator.
            IAnswerAggregator<int> answerAggregator = new MultithreadedAnswerAggregator<int>(mocks.Select(x => x.Object), threshold);

            // Set up data set.
            List<int> data = new List<int>();
            for(int i = 0; i < size; i++) 
            {
                data.Add(i);
            }

            // Run function.
            IEnumerable<string> result = answerAggregator.AggregateAnswers(data);

            // Test calls were made.
            mock1.Verify(x => x.ProvideAnswer(data), "First answer provider not called to provide answer.");
            mock2.Verify(x => x.ProvideAnswer(data), "Second answer provider not called to provide answer.");
            mock3.Verify(x => x.ProvideAnswer(data), "Third answer provider not called to provide answer.");
            mock4.Verify(x => x.ProvideAnswer(data), "Fourth answer provider not called to provide answer.");
            mock5.Verify(x => x.ProvideAnswer(data), "Fifth answer provider not called to provide answer.");

            // Test results put in the output in order.
            Assert.IsFalse(result.Count() < 5, "Not all results returned");
            Assert.IsFalse(result.Count() > 5, "Too many results returned");
            Assert.IsTrue(result1 == result.ElementAt(0), "First result not in place.");
            Assert.IsTrue(result2 == result.ElementAt(1), "Second result not in place.");
            Assert.IsTrue(result3 == result.ElementAt(2), "Third result not in place.");
            Assert.IsTrue(result4 == result.ElementAt(3), "Fourth result not in place.");
            Assert.IsTrue(result5 == result.ElementAt(4), "Fifth result not in place.");
        }
    }
}
