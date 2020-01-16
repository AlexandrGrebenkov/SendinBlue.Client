using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SendinBlue.Client.Models
{
    /// <summary>
    /// Type of the attribute. Use only if the attribute&#39;s category is &#39;normal&#39;, &#39;category&#39; or &#39;transactional&#39; ( type &#39;boolean&#39; is only available if the category is &#39;normal&#39; attribute, type &#39;id&#39; is only available if the category is &#39;transactional&#39; attribute &amp; type &#39;category&#39; is only available if the category is &#39;category&#39; attribute ).
    /// </summary>
    /// <value>Type of the attribute. Use only if the attribute&#39;s category is &#39;normal&#39;, &#39;category&#39; or &#39;transactional&#39; ( type &#39;boolean&#39; is only available if the category is &#39;normal&#39; attribute, type &#39;id&#39; is only available if the category is &#39;transactional&#39; attribute &amp; type &#39;category&#39; is only available if the category is &#39;category&#39; attribute ).</value>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AttributeType
    {
        /// <summary>
        /// Enum Text for value: text
        /// </summary>
        [EnumMember(Value = "text")]
        Text = 1,

        /// <summary>
        /// Enum Date for value: date
        /// </summary>
        [EnumMember(Value = "date")]
        Date = 2,

        /// <summary>
        /// Enum Float for value: float
        /// </summary>
        [EnumMember(Value = "float")]
        Float = 3,

        /// <summary>
        /// Enum Boolean for value: boolean
        /// </summary>
        [EnumMember(Value = "boolean")]
        Boolean = 4,

        /// <summary>
        /// Enum Id for value: id
        /// </summary>
        [EnumMember(Value = "id")]
        Id = 5,

        /// <summary>
        /// Enum Category for value: category
        /// </summary>
        [EnumMember(Value = "category")]
        Category = 6,
    }
}
