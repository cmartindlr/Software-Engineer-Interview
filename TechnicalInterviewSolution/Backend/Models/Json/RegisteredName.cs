﻿// Credit Newtonsoft.
using Backend.Objects;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Backend.Models.Json
{
    /// <summary>
    /// A name of a registered person.
    /// </summary>
    
    public class RegisteredName
    {
        // All fields are nullable since the source of the data may be improperly structured and
        // matching fields can still be read at least to analyze.

        // This follows the single responsibility principle of SOLID since the sole purpose is to be a name of a registered person.

        /// <summary>
        /// The person's last name.
        /// </summary>
        [JsonProperty("last")]
        public string? LastName { get; private set; }

        /// <summary>
        /// The person's first name.
        /// </summary>
        [JsonProperty("first")]
        public string? FirstName { get; private set; }

        /// <summary>
        /// The name formatted as last, first.
        /// </summary>
        public string? FormattedName 
        { 
            get 
            {
                if(this.FirstName != null &&
                   this.LastName != null)
                {
                    // Only return if both names are present.
                    return this.LastName + ", " + this.FirstName;
                }
                else 
                {
                    return null;
                }
            } 
        }

        /// <summary>
        /// Creates a name of a registered person.
        /// </summary>
        /// <param name="lastName">
        /// The last name of the person. This defaults to null.
        /// </param>
        /// <param name="firstName">
        /// The first name of the person. This defaults to null.
        /// </param>
        public RegisteredName(string? lastName = null, string? firstName = null) 
        {
            this.LastName = lastName;
            this.FirstName = firstName;

            // Log any null fields since this is not the expected result and indicates a
            // problem with the JSON under normal circumstances. Assuming the JSON was
            // transmitted via an API, this would indicate a problem with that API, for
            // instance.
            if(this.LastName == null)
            {
                FileLogger.Log("Last name missing on JSON.");
            }
            if(this.FirstName == null)
            {
                FileLogger.Log("First name missing on JSON.");
            }
        }
    }
}
