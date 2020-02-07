using Newtonsoft.Json;

namespace SendinBlue.Client.Models
{
    /// <summary>
    /// DTO for email sender. Sender details including id or email and name (optional).
    /// Only one of either Sender's email or Sender's ID shall be passed
    /// in one request at a time.
    /// </summary>
    public class EmailSender
    {
        /// <summary>
        /// Sender Name.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Id { get; }

        /// <summary>
        /// Sender email.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; }

        /// <summary>
        /// Select the sender for the campaign on the basis of sender id. In order
        /// to select a sender with specific pool of IP’s, dedicated ip users
        /// shall pass id (instead of email).
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <param name="name">Name.</param>
        public EmailSender(string email, string name)
        {
            Email = email;
            Name = name;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="name">Name.</param>
        public EmailSender(int? id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
