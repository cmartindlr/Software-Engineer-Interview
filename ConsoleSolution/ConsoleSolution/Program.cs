// Credit newtonsoft.
using Newtonsoft.Json;

using ConsoleSolution.Models.Json;

namespace ConsoleSolution
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // The collection of people.
            List<RegisteredPerson> people;

            using(JsonReader reader = new JsonTextReader(File.OpenText("data.json")))
            {
                // Deserialize the entire json.
                JsonSerializer deserializer = new JsonSerializer();
                people = deserializer.Deserialize<List<RegisteredPerson>>(reader) ?? new List<RegisteredPerson>();
            }
        }
    }
}