using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

class Program
{
    static void Main()
    {

        // Deserialize json into an array of Individual objects
        string path = @"../data.json";
        string jsonString = File.ReadAllText(path);
        Individual[]? results = JsonSerializer.Deserialize<Individual[]>(jsonString);
        if (results == null) {
            Console.WriteLine("JSON could not be serialized.");
            return;
        }
        
        // QUESTION 1
        // What is the count of individuals over the age of 50?
        int countOfIndividualsOver50 = 0;
        foreach (Individual individual in results) {
            if (individual.Age > 50) {
                countOfIndividualsOver50++;
            }
        }
        Console.WriteLine("What is the count of individuals over the age of 50?");
        Console.WriteLine(countOfIndividualsOver50);
        Console.WriteLine();

        // QUESTION 2
        // Who is last individual (most recent) that registered who is still active?
        DateTime lastRegistrationTimeOfActiveIndividual = DateTime.MinValue;
        Individual? lastRegisteredActiveIndividual = null;
        foreach (Individual individual in results) {
            DateTime currentIndividualRegTime = DateTime.ParseExact(individual.Registered, "dddd, MMMM d, yyyy h:mm tt", CultureInfo.InvariantCulture);
            if (individual.IsActive && (currentIndividualRegTime > lastRegistrationTimeOfActiveIndividual)) {
                lastRegisteredActiveIndividual = individual;
                lastRegistrationTimeOfActiveIndividual = currentIndividualRegTime;
            }
        }
        Console.WriteLine("Who is last individual that registered who is still active?");
        if (lastRegisteredActiveIndividual != null) {
            Console.WriteLine(lastRegisteredActiveIndividual.Id);
        } else {
            Console.WriteLine("Did not find an individual that fits the criteria.");
        }
        Console.WriteLine();

        // QUESTION 3
        // What are the counts of each favorite fruit?
        Dictionary<string, int> fruitCounts = [];
        foreach (Individual individual in results) {
            string currentFruit = individual.FavoriteFruit;
            bool keyExists = fruitCounts.TryGetValue(currentFruit, out int currentFruitCount);

            if (keyExists) {
                fruitCounts[currentFruit] = currentFruitCount + 1;
            } else {
                fruitCounts.Add(currentFruit, 1);
            }
        }
        Console.WriteLine("What are the counts of each favorite fruit?");
        foreach (var kvp in fruitCounts) {
            Console.WriteLine(kvp.Key + ": " + kvp.Value);
        }
        Console.WriteLine();

        // QUESTION 4
        // What is the most common eye color?
        Dictionary<string, int> eyeColorCounts = [];
        // Get counts of each eye color:
        foreach (Individual individual in results) {
            string currentColor = individual.EyeColor;
            bool keyExists = eyeColorCounts.TryGetValue(currentColor, out int currentColorCount);

            if (keyExists) {
                eyeColorCounts[currentColor] = currentColorCount + 1;
            } else {
                eyeColorCounts.Add(currentColor, 1);
            }
        }
        // Find eye color with highest count:
        string mostCommonColor = "";
        int mostCommonColorCount = 0;
        foreach (var kvp in eyeColorCounts) {
            if (kvp.Value >= mostCommonColorCount) {
                if (mostCommonColor == "") {
                    mostCommonColor = kvp.Key;
                } else {
                    // In the case of a tie, make a comma delimited list
                    mostCommonColor = mostCommonColor + ", " + kvp.Key;
                }
                mostCommonColorCount = kvp.Value;
            }
        }
        Console.WriteLine("What is the most common eye color?");
        if (mostCommonColor == "") {
            Console.WriteLine("No common colors found.");
        }
        Console.WriteLine(mostCommonColor);
        Console.WriteLine();

        // QUESTION 5
        // What is the total balance of all individuals combined?
        decimal currentSum = 0;
        foreach (Individual individual in results) {
            currentSum += individual.GetNumericBalance();
        }
        Console.WriteLine("What is the total balance of all individuals combined?");
        Console.WriteLine("$"+currentSum);
        Console.WriteLine();

        // QUESTION 6
        // What is the full name of the individual with the id of 5aabbca3e58dc67745d720b1 in the format of lastname, firstname?
        string idToFind = "5aabbca3e58dc67745d720b1";
        string? fullName = null;
        foreach (Individual individual in results) {
            if (individual.Id == idToFind) {
                fullName = individual.Name.Last + ", " + individual.Name.First;
                break;
            }
        }
        Console.WriteLine("What is the full name of the individual with the id of 5aabbca3e58dc67745d720b1 in the format of lastname, firstname?");
        if (fullName != null) {
            Console.WriteLine(fullName);
        } else {
            Console.WriteLine("Individual with that id could not be found.");
        }
        Console.WriteLine();

        return;
    }
}

// Represents an Individual with properties defined by ../data.json
class Individual(string favoriteFruit, string greeting, string longitude, string latitude,
    string registered, string address, string phone, string email, string company, NameObj name,
    string eyeColor, int age, string balance, bool isActive, string id)
{
    [JsonPropertyName("favoriteFruit")]
    public string FavoriteFruit { get; set; } = favoriteFruit;

    [JsonPropertyName("greeting")]
    public string Greeting { get; set; } = greeting;

    [JsonPropertyName("longitude")]
    public string Longitude { get; set; } = longitude;

    [JsonPropertyName("latitude")]
    public string Latitude { get; set; } = latitude;

    [JsonPropertyName("registered")]
    public string Registered { get; set; } = registered;

    [JsonPropertyName("address")]
    public string Address { get; set; } = address;

    [JsonPropertyName("phone")]
    public string Phone { get; set; } = phone;

    [JsonPropertyName("email")]
    public string Email { get; set; } = email;

    [JsonPropertyName("company")]
    public string Company { get; set; } = company;

    [JsonPropertyName("name")]
    public NameObj Name { get; set; } = name;

    [JsonPropertyName("eyeColor")]
    public string EyeColor { get; set; } = eyeColor;

    [JsonPropertyName("age")]
    public int Age { get; set; } = age;

    [JsonPropertyName("balance")]
    public string Balance { get; set; } = balance;

    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; } = isActive;

    [JsonPropertyName("id")]
    public string Id { get; set; } = id;

    // Print all properties for the Individual on separate lines 
    public void PrintInfoForAnIndividual() {
        Console.WriteLine(this.FavoriteFruit);
        Console.WriteLine(this.Greeting);
        Console.WriteLine(this.Longitude);
        Console.WriteLine(this.Latitude);
        Console.WriteLine(this.Registered);
        Console.WriteLine(this.Address);
        Console.WriteLine(this.Phone);
        Console.WriteLine(this.Email);
        Console.WriteLine(this.Company);
        
        Console.WriteLine(this.Name.Last);
        Console.WriteLine(this.Name.First);

        Console.WriteLine(this.EyeColor);
        Console.WriteLine(this.Age);
        Console.WriteLine(this.Balance);
        Console.WriteLine(this.IsActive);
        Console.WriteLine(this.Id);
        return;
    }

    // Get the Individual's balance in a numeric form that can be used for computation
    // "$1,475.79" would return 1475.79
    public decimal GetNumericBalance() {
        // Remove the dollar sign and comma
        string amountString = this.Balance.Replace("$", "").Replace(",", "");
        
        // Parse the string as a decimal using CultureInfo to handle the decimal separator
        decimal amount = decimal.Parse(amountString, NumberStyles.Any, CultureInfo.InvariantCulture);

        return amount;
    }
}

// Class that makes up an Individual's full name
class NameObj(string first, string last)
{
    [JsonPropertyName("first")]
    public string First { get; set; } = first;

    [JsonPropertyName("last")]
    public string Last { get; set; } = last;
}