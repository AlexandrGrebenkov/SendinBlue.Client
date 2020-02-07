using System.Collections.Generic;
using Newtonsoft.Json;

namespace SendinBlue.Client.Models
{
    /// <summary>
    /// Model for sending a transactional email.
    /// </summary>
    public class TransactionalEmail
    {
        /// <summary>
        /// List of email addresses and names (optional) of the recipients.
        /// For example, [{'name':'Jimmy', 'email':'jimmy98@example.com'}, {'name':'Joe', 'email':'joe@example.com'}].
        /// </summary>
        public IEnumerable<EmailSender> To { get; }

        /// <summary>
        /// Id of the template.
        /// </summary>
        public int TemplateId { get; set; }

        /// <summary>
        /// Pass the set of attributes to customize the template. For example, {'FNAME':'Joe', 'LNAME':'Doe'}.
        /// It's considered only if template is in New Template Language format.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionalParams Params { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="name"></param>
        public TransactionalEmail(string email, string name = null)
        {
            To = new List<EmailSender>
            {
                new EmailSender(email, name),
            };
        }
    }
}
