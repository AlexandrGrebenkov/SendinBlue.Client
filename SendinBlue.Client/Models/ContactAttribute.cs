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
        [JsonConstructor]
        protected ContactAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactAttribute" /> class.
        /// </summary>
        /// <param name="value">Id of the value (required).</param>
        /// <param name="label">Label of the value (required).</param>
        public ContactAttribute(int value, string label)
        {
            Value = value;
            Label = label ?? throw new InvalidDataException("label is a required property for ContactAttribute and cannot be null");
        }

        /// <summary>
        /// Id of the value.
        /// </summary>
        /// <value>Id of the value.</value>
        [DataMember(Name = "value", EmitDefaultValue = false)]
        public int Value { get; set; }

        /// <summary>
        /// Label of the value.
        /// </summary>
        /// <value>Label of the value.</value>
        [DataMember(Name = "label", EmitDefaultValue = false)]
        public string Label { get; set; }
    }
}
