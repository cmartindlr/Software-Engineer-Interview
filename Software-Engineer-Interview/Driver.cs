using Software_Engineer_Interview.DAL;
using Software_Engineer_Interview.Models;
using System.Reflection;

namespace Software_Engineer_Interview
{
    internal class Driver
    {
        const string JsonFileName = @"assets\data.json";

        const string TargetId = "5aabbca3e58dc67745d720b1";
        public void Execute()
        {
            Repository handler = new Repository();
            List<Individual> individualsList = handler.ReadJsonData(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), JsonFileName));

            int countOver50 = handler.GetCountOfIndividualWithAgeMoreThan50(individualsList);
            Console.WriteLine($"Count of individuals over the age of 50: {countOver50}");

            Individual lastActiveIndividual = handler.GetLastActiveIndividual(individualsList);
            Console.WriteLine($"Last individual that registered and is still active: {lastActiveIndividual.Name.Last}, {lastActiveIndividual.Name.First}");

            Console.WriteLine("Favourite Fruits:");
            Dictionary<string, int> countOfEachFavoriteFruite = handler.GetCountOfEachFavoriteFruite(individualsList);
            foreach (var kvp in countOfEachFavoriteFruite)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
            Console.WriteLine();

            string mostCommonEyeColor = handler.GetMostCommonEyeColor(individualsList);
            Console.WriteLine($"Most common eye color: {mostCommonEyeColor}");

            decimal totalBalanceOfIndividuals = handler.GetTotalBalanceOfIndividuals(individualsList);
            Console.WriteLine($"Total balance of all individuals combined: ${totalBalanceOfIndividuals}");

            string nameOfSpecificIndividual = handler.GetNameOfSpecificIndividual(individualsList, TargetId);
            Console.WriteLine($"Full name of individual with id {TargetId}: {nameOfSpecificIndividual}");
        }
    }
}
