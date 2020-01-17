using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SendinBlue.Client.Models
{
    public class CreatingAttribute
    {
        /// <summary>
        /// Value of the attribute. Use only if the attribute's category is 'calculated' or 'global'.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// List of values and labels that the attribute can take. Use only if the attribute's 
        /// category is "category".
        /// </summary>
        public IEnumerable<string> Enumeration { get; set; }

        /// <summary>
        /// Type of the attribute.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("type")]
        public AttributeType Type { get; set; }
    }
}
