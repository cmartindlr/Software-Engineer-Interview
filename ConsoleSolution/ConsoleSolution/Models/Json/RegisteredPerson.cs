// Credit Newtonsoft.
using Newtonsoft.Json;
using System.Globalization;

namespace ConsoleSolution.Models.Json
{
    /// <summary>
    /// Represents a person who has been registered.
    /// </summary>
    public class RegisteredPerson
    {
        // All fields are nullable since the source of the data may be improperly structured and
        // matching fields can still be read at least to analyze.

        /// <summary>
        /// The individual's favorite fruit.
        /// </summary>
        [JsonProperty("favoriteFruit")]
        public string? FavoriteFruit { get; private set; }

        /// <summary>
        /// The greeting for the person.
        /// </summary>
        [JsonProperty("greeting")]
        public string? Greeting { get; private set; }

        /// <summary>
        /// The person's longitude.
        /// </summary>
        [JsonProperty("longitude")]
        public double? Longitude { get; private set; }

        /// <summary>
        /// The person's latitude.
        /// </summary>
        [JsonProperty("latitude")]
        public double? Latitude { get; private set; }

        /// <summary>
        /// The date the person was registered.
        /// </summary>
        [JsonProperty("registered")]
        public DateTime? RegistrationDate { get; private set; }

        /// <summary>
        /// The person's address.
        /// </summary>
        [JsonProperty("address")]
        public string? Address { get; private set; }

        /// <summary>
        /// The person's phone number.
        /// </summary>
        [JsonProperty("phone")]
        public string? PhoneNumber { get; private set; }

        /// <summary>
        /// The person's email.
        /// </summary>
        [JsonProperty("email")]
        public string? EmailAddress { get; private set; }

        /// <summary>
        /// The person's company.
        /// </summary>
        [JsonProperty("company")]
        public string? Company { get; private set; }

        /// <summary>
        /// The person's name.
        /// </summary>
        [JsonProperty("name")]
        public RegisteredName? Name { get; private set; }

        /// <summary>
        /// The person's eye color.
        /// </summary>
        [JsonProperty("eyeColor")]
        public string? EyeColor { get; private set; }

        /// <summary>
        /// The person's age.
        /// </summary>
        [JsonProperty("age")]
        public int? Age { get; private set; }

        /// <summary>
        /// The balance for the person.
        /// </summary>
        [JsonProperty("balance")]
        public string? Balance { get; private set; }

        /// <summary>
        /// The numeric value of the balance.
        /// </summary>
        public decimal? NumericBalance
        {
            get
            {
                if (this.Balance != null &&
                   // Parse only characters that are part of a valid number.
                   decimal.TryParse(this.Balance, 
                                    NumberStyles.AllowCurrencySymbol | NumberStyles.Number, // Parse as a number or currency
                                    new CultureInfo("en-US"), // Values are in USD
                                    out decimal balance))
                {
                    return balance;
                }
                else
                {
                    // On errors, return null.
                    return null;
                }
            }
        }

        /// <summary>
        /// Whether the person is still active.
        /// </summary>
        [JsonProperty("isActive")]
        public bool? IsActive { get; private set; }

        /// <summary>
        /// The person's ID.
        /// </summary>
        [JsonProperty("id")]
        public string? ID { get; private set; }

        /// <summary>
        /// Creates a registered person given all of the fields.
        /// </summary>
        /// <param name="favoriteFruit">
        /// The person's favorite fruit.
        /// </param>
        /// <param name="greeting">
        /// The greeting for the person.
        /// </param>
        /// <param name="longitude">
        /// The longitude of the person.
        /// </param>
        /// <param name="latitude">
        /// The latitude of the person.
        /// </param>
        /// <param name="registrationDate">
        /// The person's registration date.
        /// </param>
        /// <param name="address">
        /// The address of the person.
        /// </param>
        /// <param name="phoneNumber">
        /// The phone number of the person.
        /// </param>
        /// <param name="emailAddress">
        /// The email of the person.
        /// </param>
        /// <param name="company">
        /// The company of the person.
        /// </param>
        /// <param name="name">
        /// The name of the person.
        /// </param>
        /// <param name="eyeColor">
        /// The eye color of the person.
        /// </param>
        /// <param name="age">
        /// The age of the person.
        /// </param>
        /// <param name="balance">
        /// The balance of the person.
        /// </param>
        /// <param name="isActive">
        /// Whether the person is active.
        /// </param>
        /// <param name="ID">
        /// The ID of the person.
        /// </param>
        public RegisteredPerson(string? favoriteFruit,
                                string? greeting,
                                double? longitude,
                                double? latitude,
                                DateTime? registrationDate,
                                string? address,
                                string? phoneNumber,
                                string? emailAddress,
                                string? company,
                                RegisteredName? name,
                                string? eyeColor,
                                int? age,
                                string? balance,
                                bool? isActive,
                                string? ID) 
        { 
            this.FavoriteFruit = favoriteFruit;
            this.Greeting = greeting;
            this.Longitude = longitude;
            this.Latitude = latitude;
            this.RegistrationDate = registrationDate;
            this.Address = address;
            this.PhoneNumber = phoneNumber;
            this.EmailAddress = emailAddress;
            this.Company = company;
            this.Name = name;
            this.EyeColor = eyeColor;
            this.Age = age;
            this.Balance = balance;
            this.IsActive = isActive;
            this.ID = ID;
        }
    }
}
