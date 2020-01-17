using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace SendinBlue.Client.Models
{
    /// <summary>
    /// Name of contact data column.
    /// </summary>
    public class ContactAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactAttribute" /> class.
        /// </summary>
        //[JsonConstructor]
        public ContactAttribute()
        {
        }

        public string Name { get; set; }

        public string Category { get; set; }

        public AttributeType Type { get; set; }
    }
}
