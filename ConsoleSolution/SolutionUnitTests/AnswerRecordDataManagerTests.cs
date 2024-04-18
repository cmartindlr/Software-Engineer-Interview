
using ConsoleSolution.Models.Sql;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace SolutionUnitTests
{
    [TestClass]
    public class AnswerRecordDataManagerTests
    {
        /// <summary>
        /// The mocked factory.
        /// </summary>
        private static Mock<IDbContextFactory<AnswerContext>> _mockFactory;

        /// <summary>
        /// The answer record data manager.
        /// </summary>
        private static AnswerRecordDataManager _dataManager;

        /// <summary>
        /// Sets up the answer record data manager to test.
        /// </summary>
        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            // Setup the factory to make in-memory databases.
            AnswerRecordDataManagerTests._mockFactory = new Mock<IDbContextFactory<AnswerContext>>();
            AnswerRecordDataManagerTests._mockFactory.Setup(x => x.CreateDbContext())
                             // Credit https://www.meziantou.net/testing-ef-core-in-memory-using-sqlite.htm
                             .Returns(() => 
                             {
                                 // Credit https://stackoverflow.com/questions/56319638/entityframeworkcore-sqlite-in-memory-db-tables-are-not-created
                                 SqliteConnection keepAliveConnection = new SqliteConnection("DataSource=Memory;mode=memory;cache=shared");
                                 keepAliveConnection.Open();
                                 using(AnswerContext database = new AnswerContext(new DbContextOptionsBuilder<AnswerContext>().UseSqlite(keepAliveConnection).Options))
                                 {
                                     database.Database.EnsureCreated();
                                 }
                                 return new AnswerContext(new DbContextOptionsBuilder<AnswerContext>().UseSqlite(keepAliveConnection).Options);
                             });

            // Create the object.
            AnswerRecordDataManagerTests._dataManager = new AnswerRecordDataManager(AnswerRecordDataManagerTests._mockFactory.Object);
        }

        /// <summary>
        /// Initializes the test.
        /// </summary>
        [TestInitialize]
        public void InitializeTest() 
        {
            // Clear invocations at the start of each test.
            AnswerRecordDataManagerTests._mockFactory.Invocations.Clear();
        }

        /// <summary>
        /// Cleans up the database between tests.
        /// </summary>
        [TestCleanup]
        public void CleanupTest() 
        {
            using(AnswerContext database = AnswerRecordDataManagerTests._mockFactory.Object.CreateDbContext())
            {
                // Remove anything added.
                foreach(AnswerRecord record in database.AnswerRecords) 
                { 
                    database.AnswerRecords.Remove(record);
                }
                database.SaveChanges();
            }
        }

        /// <summary>
        /// Tests the save functionality for the answer record data manager.
        /// </summary>
        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(5)]
        [DataRow(10)]
        [DataRow(50)]
        [DataRow(100)]
        [DataRow(500)]
        public void TestAnswerRecordDataManagerSaves(int totalRecords) 
        {
            // Create data.
            List<AnswerRecord> answers = new List<AnswerRecord>();
            for(int i = 0; i < totalRecords; i++) 
            { 
                answers.Add(new AnswerRecord() 
                { 
                    Answer = i.ToString(),
                    Question = i.ToString(),
                    AnswerDate = DateTime.Now,
                    FileName = i.ToString()
                });
            }

            // Save data.
            AnswerRecordDataManagerTests._dataManager.Save(answers);

            // Check that the database context is created.
            AnswerRecordDataManagerTests._mockFactory.Verify(x => x.CreateDbContext(), "DbContext from factory not used.");

            // Verify that the data is saved.
            using(AnswerContext database = AnswerRecordDataManagerTests._mockFactory.Object.CreateDbContext()) 
            {
                foreach(AnswerRecord answer in answers) 
                {
                    Assert.IsTrue(database.AnswerRecords.Any(x => x.AnswerDate == answer.AnswerDate &&
                                                                  x.Question == answer.Question &&
                                                                  x.Answer == answer.Answer &&
                                                                  x.FileName == answer.FileName), "Record is not saved.");
                }
            }
        }
    }
}
