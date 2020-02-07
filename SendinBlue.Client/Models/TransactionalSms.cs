namespace SendinBlue.Client.Models
{
    /// <summary>
    /// Model for creating a transactional SMS.
    /// </summary>
    public class TransactionalSms
    {
        /// <summary>
        /// Name of the sender. Only alphanumeric characters. No more than 11 characters.
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// Mobile number to send SMS with the country code.
        /// </summary>
        public string Recipient { get; set; }

        /// <summary>
        /// Content of the message. If more than 160 characters long,
        /// will be sent as multiple text messages.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Type of the SMS. Marketing SMS messages are those sent typically with
        /// marketing content. Transactional SMS messages are sent to individuals
        /// and are triggered in response to some action, such as a sign-up, purchase, etc.
        /// </summary>
        public string Type => "transactional";
    }
}
