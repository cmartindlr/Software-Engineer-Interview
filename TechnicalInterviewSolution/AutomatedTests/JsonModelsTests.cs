using System.Globalization;

namespace AutomatedTests
{
    [TestClass]
    public class JsonModelsTests
    {
        /// <summary>
        /// Tests if fields are null on unformatted JSON for a person.
        /// </summary>
        [TestMethod]
        public void MissingPersonFieldsTest()
        {
            // First test is with a completely empty object.
            string json = @"{}";

            using(JsonReader reader = new JsonTextReader(new StringReader(json)))
            {
                // Read the JSON.
                JsonSerializer deserializer = new JsonSerializer();
                RegisteredPerson? person = deserializer.Deserialize<RegisteredPerson>(reader);

                // Check if data was parsed. (It should be)
                Assert.IsTrue(person != null, "Person null on empty object.");

                // Check that every field is null.
                Assert.IsTrue(person.Address == null, "Address non-null on missing address field.");
                Assert.IsTrue(person.Age == null, "Age non-null on missing age field.");
                Assert.IsTrue(person.Balance == null, "Balance non-null on missing balance field.");
                Assert.IsTrue(person.Company == null, "Company non-null on missing company field.");
                Assert.IsTrue(person.EmailAddress == null, "Email non-null on missing email field.");
                Assert.IsTrue(person.EyeColor == null, "Eye color non-null on missing eye color field.");
                Assert.IsTrue(person.FavoriteFruit == null, "Favorite fruit non-null on missing favorite fruit field.");
                Assert.IsTrue(person.Greeting == null, "Greeting non-null on missing greeting field.");
                Assert.IsTrue(person.ID == null, "ID non-null on missing ID field.");
                Assert.IsTrue(person.IsActive == null, "IsActive non-null on missing isActive field.");
                Assert.IsTrue(person.Latitude == null, "Latitude non-null on missing latitude field.");
                Assert.IsTrue(person.Longitude == null, "Longitude non-null on missing longitude field.");
                Assert.IsTrue(person.Name == null, "Name non-null on missing name field.");
                Assert.IsTrue(person.NumericBalance == null, "Numeric balance non-null on missing balance field.");
                Assert.IsTrue(person.PhoneNumber == null, "Phone number non-null on missing phone field.");
                Assert.IsTrue(person.RegistrationDate == null, "Date non-null on missing date field.");
            }

            // Next test is with a partially empty object.
            json = @"{ ""age"": 10 }";

            using(JsonReader reader = new JsonTextReader(new StringReader(json)))
            {
                // Read the JSON.
                JsonSerializer deserializer = new JsonSerializer();
                RegisteredPerson? person = deserializer.Deserialize<RegisteredPerson>(reader);

                // Check if data was parsed. (It should be)
                Assert.IsTrue(person != null, "Person null on incomplete object.");

                // Check that missing fields are null and others are not.
                Assert.IsTrue(person.Address == null, "Address non-null on missing address field.");
                Assert.IsTrue(person.Age != null, "Age null when present.");
                Assert.IsTrue(person.Balance == null, "Balance non-null on missing balance field.");
                Assert.IsTrue(person.Company == null, "Company non-null on missing company field.");
                Assert.IsTrue(person.EmailAddress == null, "Email non-null on missing email field.");
                Assert.IsTrue(person.EyeColor == null, "Eye color non-null on missing eye color field.");
                Assert.IsTrue(person.FavoriteFruit == null, "Favorite fruit non-null on missing favorite fruit field.");
                Assert.IsTrue(person.Greeting == null, "Greeting non-null on missing greeting field.");
                Assert.IsTrue(person.ID == null, "ID non-null on missing ID field.");
                Assert.IsTrue(person.IsActive == null, "IsActive non-null on missing isActive field.");
                Assert.IsTrue(person.Latitude == null, "Latitude non-null on missing latitude field.");
                Assert.IsTrue(person.Longitude == null, "Longitude non-null on missing longitude field.");
                Assert.IsTrue(person.Name == null, "Name non-null on missing name field.");
                Assert.IsTrue(person.NumericBalance == null, "Numeric balance non-null on missing balance field.");
                Assert.IsTrue(person.PhoneNumber == null, "Phone number non-null on missing phone field.");
                Assert.IsTrue(person.RegistrationDate == null, "Date non-null on missing date field.");
            }

            // Third test is with an object with no matching fields.
            json = @"{ ""nonsense"": ""qwertey"" }";

            using(JsonReader reader = new JsonTextReader(new StringReader(json)))
            {
                // Read the JSON.
                JsonSerializer deserializer = new JsonSerializer();
                RegisteredPerson? person = deserializer.Deserialize<RegisteredPerson>(reader);

                // Check if data was parsed. (It should be)
                Assert.IsTrue(person != null, "Person null on non-matching object.");

                // Check that every field is null.
                Assert.IsTrue(person.Address == null, "Address non-null on missing address field.");
                Assert.IsTrue(person.Age == null, "Age non-null on missing age field.");
                Assert.IsTrue(person.Balance == null, "Balance non-null on missing balance field.");
                Assert.IsTrue(person.Company == null, "Company non-null on missing company field.");
                Assert.IsTrue(person.EmailAddress == null, "Email non-null on missing email field.");
                Assert.IsTrue(person.EyeColor == null, "Eye color non-null on missing eye color field.");
                Assert.IsTrue(person.FavoriteFruit == null, "Favorite fruit non-null on missing favorite fruit field.");
                Assert.IsTrue(person.Greeting == null, "Greeting non-null on missing greeting field.");
                Assert.IsTrue(person.ID == null, "ID non-null on missing ID field.");
                Assert.IsTrue(person.IsActive == null, "IsActive non-null on missing isActive field.");
                Assert.IsTrue(person.Latitude == null, "Latitude non-null on missing latitude field.");
                Assert.IsTrue(person.Longitude == null, "Longitude non-null on missing longitude field.");
                Assert.IsTrue(person.Name == null, "Name non-null on missing name field.");
                Assert.IsTrue(person.NumericBalance == null, "Numeric balance non-null on missing balance field.");
                Assert.IsTrue(person.PhoneNumber == null, "Phone number non-null on missing phone field.");
                Assert.IsTrue(person.RegistrationDate == null, "Date non-null on missing date field.");
            }

            // Final test is with a partially empty object and extra fields.
            json = @"{ ""company"": 10, ""height"": 12  }";

            using(JsonReader reader = new JsonTextReader(new StringReader(json)))
            {
                // Read the JSON.
                JsonSerializer deserializer = new JsonSerializer();
                RegisteredPerson? person = deserializer.Deserialize<RegisteredPerson>(reader);

                // Check if data was parsed. (It should be)
                Assert.IsTrue(person != null, "Person null on object with extra fields.");

                // Check that missing fields are null and others are not.
                Assert.IsTrue(person.Address == null, "Address non-null on missing address field.");
                Assert.IsTrue(person.Age == null, "Age non-null on missing age field.");
                Assert.IsTrue(person.Balance == null, "Balance non-null on missing balance field.");
                Assert.IsTrue(person.Company != null, "Company null when present.");
                Assert.IsTrue(person.EmailAddress == null, "Email non-null on missing email field.");
                Assert.IsTrue(person.EyeColor == null, "Eye color non-null on missing eye color field.");
                Assert.IsTrue(person.FavoriteFruit == null, "Favorite fruit non-null on missing favorite fruit field.");
                Assert.IsTrue(person.Greeting == null, "Greeting non-null on missing greeting field.");
                Assert.IsTrue(person.ID == null, "ID non-null on missing ID field.");
                Assert.IsTrue(person.IsActive == null, "IsActive non-null on missing isActive field.");
                Assert.IsTrue(person.Latitude == null, "Latitude non-null on missing latitude field.");
                Assert.IsTrue(person.Longitude == null, "Longitude non-null on missing longitude field.");
                Assert.IsTrue(person.Name == null, "Name non-null on missing name field.");
                Assert.IsTrue(person.NumericBalance == null, "Numeric balance non-null on missing balance field.");
                Assert.IsTrue(person.PhoneNumber == null, "Phone number non-null on missing phone field.");
                Assert.IsTrue(person.RegistrationDate == null, "Date non-null on missing date field.");
            }
        }
        /// <summary>
        /// Tests if fields are null on unformatted JSON for a name.
        /// </summary>
        [TestMethod]
        public void MissingNameFieldsTest()
        {
            // First test is with a completely empty object.
            string json = @"{}";

            using(JsonReader reader = new JsonTextReader(new StringReader(json)))
            {
                // Read the JSON.
                JsonSerializer deserializer = new JsonSerializer();
                RegisteredName? name = deserializer.Deserialize<RegisteredName>(reader);

                // Check if data was parsed. (It should be)
                Assert.IsTrue(name != null, "Name null on empty object.");

                // Check that every field is null.
                Assert.IsTrue(name.FirstName == null, "First name non-null on missing first field.");
                Assert.IsTrue(name.LastName == null, "Last name non-null on missing last field.");
                Assert.IsTrue(name.FormattedName == null, "Formatted name non-null on missing fields.");
            }

            // Next test is with a partially empty object.
            json = @"{ ""first"": ""Tyler"" }";

            using(JsonReader reader = new JsonTextReader(new StringReader(json)))
            {
                // Read the JSON.
                JsonSerializer deserializer = new JsonSerializer();
                RegisteredName? name = deserializer.Deserialize<RegisteredName>(reader);

                // Check if data was parsed. (It should be)
                Assert.IsTrue(name != null, "Name null on empty object.");

                // Check that missing fields are null and others are not.
                Assert.IsTrue(name.FirstName != null, "First name null when present.");
                Assert.IsTrue(name.LastName == null, "Last name non-null on missing last field.");
                Assert.IsTrue(name.FormattedName == null, "Formatted name non-null on missing fields.");
            }

            // Third test is with an object with no matching fields.
            json = @"{ ""nonsense"": ""qwertey"" }";

            using(JsonReader reader = new JsonTextReader(new StringReader(json)))
            {
                // Read the JSON.
                JsonSerializer deserializer = new JsonSerializer();
                RegisteredName? name = deserializer.Deserialize<RegisteredName>(reader);

                // Check if data was parsed. (It should be)
                Assert.IsTrue(name != null, "Name null on non-matching object.");

                // Check that every field is null.
                Assert.IsTrue(name.FirstName == null, "First name non-null on missing first field.");
                Assert.IsTrue(name.LastName == null, "Last name non-null on missing last field.");
                Assert.IsTrue(name.FormattedName == null, "Formatted name non-null on missing fields.");
            }

            // Final test is with a partially empty object and extra fields.
            json = @"{ ""first"": ""Tyler"", ""middle"": ""Dylan""  }";

            using(JsonReader reader = new JsonTextReader(new StringReader(json)))
            {
                // Read the JSON.
                JsonSerializer deserializer = new JsonSerializer();
                RegisteredName? name = deserializer.Deserialize<RegisteredName>(reader);

                // Check if data was parsed. (It should be)
                Assert.IsTrue(name != null, "Name null on object with extra fields.");

                // Check that missing fields are null and others are not.
                Assert.IsTrue(name.FirstName != null, "First name null when present.");
                Assert.IsTrue(name.LastName == null, "Last name non-null on missing last field.");
                Assert.IsTrue(name.FormattedName == null, "Formatted name non-null on missing fields.");
            }
        }

        /// <summary>
        /// Tests whether the name is properly formatted as last, first.
        /// </summary>
        /// <param name="first">
        /// The first name.
        /// </param>
        /// <param name="last">
        /// The last name.
        /// </param>
        [TestMethod]
        [DataRow("Tyler", "Carson")]
        [DataRow("", "")]
        [DataRow("First", "Last")]
        public void ProperNameFormatTest(string first, string last) 
        { 
            RegisteredName name = new RegisteredName(last, first);
            Assert.IsTrue(name.FormattedName == last +", " + first, "Name formatted in the wrong order.");
        }

        /// <summary>
        /// Tests whether the balance can be parsed as a decimal.
        /// </summary>
        /// <param name="balance"></param>
        [TestMethod]
        [DataRow("$123")]
        [DataRow("$168.23")]
        [DataRow("0.23")]
        public void BalanceParseTest(string balance) 
        { 
            decimal value = decimal.Parse(balance, NumberStyles.AllowCurrencySymbol | NumberStyles.Number, new CultureInfo("en-US"));
            RegisteredPerson person = new RegisteredPerson(null, null, null, null, null, null, null, null, null, null, null, null, balance, null, null);
            Assert.IsTrue(value == person.NumericBalance, "Numeric balanace does not match parsed balance.");
        }

        /// <summary>
        /// Tests whether the fields are properly parsed in a person from a json.
        /// </summary>
        [TestMethod]
        [DataRow("a", "b", 0, 1, "c", "d", "e", "f", "g", "h", "i", 2, "j", false, "k")]
        [DataRow("e", "rgdg", -356, 60, "dg5", "n tr", "89 7i", "213ew", "dfs ", "5t", "sf", 43, "$213.49",true, "thfdbg")]
        public void PersonParsedFieldsMatchTest(string favoriteFruit, 
                                                string greeting, 
                                                double longitude, 
                                                double latitude, 
                                                string address, 
                                                string phoneNumber, 
                                                string emailAddress, 
                                                string company, 
                                                string lastName, 
                                                string firstName, 
                                                string eyeColor, 
                                                int age, 
                                                string balance, 
                                                bool isActive, 
                                                string id) 
        {
            // Cannot pass in datetime through attribute flexibly. Random should suffice.
            DateTime date = new DateTime(Random.Shared.Next(2000, 3000), Random.Shared.Next(1, 13), Random.Shared.Next(1, 29), Random.Shared.Next(0, 24), Random.Shared.Next(0, 60), Random.Shared.Next(0, 60));
            string json = $"{{\"favoriteFruit\": \"{favoriteFruit}\", \"greeting\": \"{greeting}\", \"longitude\": \"{longitude}\", \"latitude\": \"{latitude}\", \"registered\": {JsonConvert.SerializeObject(date)}, \"address\": \"{address}\", \"phone\": \"{phoneNumber}\", \"email\": \"{emailAddress}\", \"company\": \"{company}\", \"name\": {{ \"last\": \"{lastName}\", \"first\": \"{firstName}\" }}, \"eyeColor\": \"{eyeColor}\", \"age\": {age}, \"balance\": \"{balance}\", \"isActive\": {isActive.ToString().ToLower()}, \"id\": \"{id}\" }}";

            // Check all of the parsed fields match those passed.
            using(JsonReader reader = new JsonTextReader(new StringReader(json)))
            {
                // Read the JSON.
                JsonSerializer deserializer = new JsonSerializer();
                RegisteredPerson person = deserializer.Deserialize<RegisteredPerson>(reader);

                // Check that the fields match.
                Assert.IsTrue(person.Address == address, "Address does not match.");
                Assert.IsTrue(person.Age == age, "Age does not match.");
                Assert.IsTrue(person.Balance == balance, "Balance does not match.");
                Assert.IsTrue(person.Company == company, "Company does not match.");
                Assert.IsTrue(person.EmailAddress == emailAddress, "Email does not match.");
                Assert.IsTrue(person.EyeColor == eyeColor, "Eye color does not match.");
                Assert.IsTrue(person.FavoriteFruit == favoriteFruit, "Favorite fruit does not match.");
                Assert.IsTrue(person.Greeting == greeting, "Greeting does not match.");
                Assert.IsTrue(person.ID == id, "ID does not match.");
                Assert.IsTrue(person.IsActive == isActive, "IsActive does not match.");
                Assert.IsTrue(person.Latitude == latitude, "Latitude does not match.");
                Assert.IsTrue(person.Longitude == longitude, "Longitude does not match.");
                Assert.IsTrue(person.Name.FirstName == firstName, "Name does not match.");
                Assert.IsTrue(person.Name.LastName == lastName, "Name does not match.");
                Assert.IsTrue(person.PhoneNumber == phoneNumber, "Phone number does not match.");
                Assert.IsTrue(person.RegistrationDate == date, "Date does not match.");
            }
        }

        /// <summary>
        /// Tests whether the fields are properly parsed in a person from a json.
        /// </summary>
        [TestMethod]
        [DataRow("Tyler", "Carson")]
        [DataRow("A", "B")]
        public void NameParsedFieldsMatchTest(string firstName, string lastName)
        {
            string json = $"{{ \"last\": \"{lastName}\", \"first\": \"{firstName}\" }}";

            // Check all of the parsed fields match those passed.
            using(JsonReader reader = new JsonTextReader(new StringReader(json)))
            {
                // Read the JSON.
                JsonSerializer deserializer = new JsonSerializer();
                RegisteredName name = deserializer.Deserialize<RegisteredName>(reader);

                // Check that the fields match.
                Assert.IsTrue(name.FirstName == firstName, "First name does not match.");
                Assert.IsTrue(name.LastName == lastName, "Last name does not match.");
            }
        }
    }
}