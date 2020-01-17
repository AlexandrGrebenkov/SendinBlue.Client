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

        public string Category { get; set; } = "normal";

        public AttributeType Type { get; set; }
    }
}
