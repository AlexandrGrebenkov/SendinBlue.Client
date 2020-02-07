using System.Collections.Generic;
using System.Linq;

namespace SendinBlue.Client.Models
{
    /// <summary>
    /// Parameters for transactional mailing.
    /// </summary>
    public class TransactionalParams
    {
        /// <summary>
        /// Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Common data.
        /// </summary>
        public IDictionary<string, object> Common { get; }

        /// <summary>
        /// Tabled data.
        /// </summary>
        public IEnumerable<IDictionary<string, object>> Data { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="contactData">Grouped contact data.</param>
        /// <param name="email">Email.</param>
        public TransactionalParams(IEnumerable<IDictionary<string, object>> contactData, string email)
        {
            Email = email;
            Common = contactData.First();
            Data = contactData.ToList();
        }
    }
}
