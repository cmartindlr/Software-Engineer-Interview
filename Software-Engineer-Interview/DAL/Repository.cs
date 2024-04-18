using Newtonsoft.Json;
using Software_Engineer_Interview.Models;

namespace Software_Engineer_Interview.DAL
{
    internal class Repository
    {
        public List<Individual> ReadJsonData(string jsonFileName)
        {
            try
            {
                string jsonData = File.ReadAllText(jsonFileName);

                List<Individual> individuals = JsonConvert.DeserializeObject<List<Individual>>(jsonData);
                return individuals;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetCountOfIndividualWithAgeMoreThan50(List<Individual> individuals)
        {
            try
            {
                return individuals.Count(individual => individual.Age > 50);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Individual GetLastActiveIndividual(List<Individual> individuals)
        {
            try
            {
                return individuals
                .Where(individual => individual.IsActive)
                .OrderByDescending(individual => individual.Registered)
                .FirstOrDefault() ?? new Individual();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<string, int> GetCountOfEachFavoriteFruite(List<Individual> individuals)
        {
            try
            {
                return individuals
                        .GroupBy(individual => individual.FavoriteFruit)
                        .ToDictionary(group => group.Key, group => group.Count());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetMostCommonEyeColor(List<Individual> individuals)
        {
            try
            {
                return individuals
                        .GroupBy(individual => individual.EyeColor)
                        .OrderByDescending(group => group.Count())
                        .Select(group => group.Key)
                        .FirstOrDefault() ?? "";
            }
            catch (Exception)
            {
                throw;
            }
        }

        public decimal GetTotalBalanceOfIndividuals(List<Individual> individuals)
        {
            try
            {
                return individuals
                        .Sum(individual => decimal.Parse(individual.Balance.TrimStart('$')));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetNameOfSpecificIndividual(List<Individual> individuals, string targetId)
        {
            try
            {
                return individuals
                        .Where(individual => individual.Id == targetId)
                        .Select(individual => $"{individual.Name.Last}, {individual.Name.First}")
                        .FirstOrDefault() ?? "";
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
