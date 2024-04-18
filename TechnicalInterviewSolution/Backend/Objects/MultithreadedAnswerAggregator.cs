using Backend.Interfaces;
using Backend.Models.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Objects
{
    /// <inheritdoc/>
    public class MultithreadedAnswerAggregator<T> : IAnswerAggregator<T>
    {
        // This follow the dependency inversion principle of SOLID by relying on abstractions that can be injected.

        /// <inheritdoc/>
        public IEnumerable<IAnswerProvider<T>> AnswerProviders { get; private set; }

        /// <summary>
        /// The amount of items before threading is used.
        /// </summary>
        private int _dataSetThreshold;

        /// <summary>
        /// Injects the answer providers into the answer aggregator and constructs it.
        /// </summary>
        /// <param name="answerProviders">
        /// The set of answers that this will aggregate.
        /// </param>
        /// <param name="dataSetThreshold">
        /// The size of the data set before threading is done. Defaults to 1000 items or more.
        /// </param>
        public MultithreadedAnswerAggregator(IEnumerable<IAnswerProvider<T>> answerProviders, int dataSetThreshold = 1000)
        {
            this.AnswerProviders = answerProviders;
            this._dataSetThreshold = dataSetThreshold;
        }

        /// <inheritdoc/>
        public IEnumerable<string> AggregateAnswers(IEnumerable<T> data)
        {
            // Create a list of results the same size as the number of answers.
            string[] results = new string[this.AnswerProviders.Count()];

            // Check whether or not to thread.
            if(this._dataSetThreshold < data.Count())
            {
                // Begin tasks to get each answer. This ends up being a bit slower for this problem, but scales better with data set size.
                List<Task> tasks = new List<Task>();
                int index = 0; // Keep track of the index.
                foreach(IAnswerProvider<T> answerProvider in this.AnswerProviders)
                {
                    // Save the task to wait on later.
                    int i = index; // Copy current index value.
                    tasks.Add(Task.Run(() =>
                    {
                        results[i] = answerProvider.ProvideAnswer(data) ?? "null";
                    }));
                    index++;
                }

                // Wait on all to finish.
                try
                {
                    Task.WhenAll(tasks).Wait();

                    // Return the results.
                    return results;
                }
                catch
                {
                    // If this operation fails, log and throw an exception.
                    Console.WriteLine("An error was encountered upon awaiting threaded answers.");
                    throw;
                }
            }
            else 
            {
                // Do the single-threaded aggregation.
                int index = 0; // Keep track of the index.
                foreach(IAnswerProvider<T> answerProvider in this.AnswerProviders)
                {
                    // Save the task to wait on later.
                    results[index] = answerProvider.ProvideAnswer(data) ?? "null";
                    index++;
                }

                return results;
            }
        }
    }
}
