namespace SendinBlue.Client.Models
{
    /// <summary>
    /// Transactional SMS response.
    /// </summary>
    internal class TransactionalSmsResponse
    {
        /// <summary>
        /// Reference.
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// Message id.
        /// </summary>
        public long MessageId { get; set; }

        /// <summary>
        /// Count of SMS's to send multiple text messages.
        /// </summary>
        public long SmsCount { get; set; }

        /// <summary>
        /// SMS credits used per text message.
        /// </summary>
        public double UserCredits { get; set; }

        /// <summary>
        /// Remaining SMS credits of the user.
        /// </summary>
        public double RemainingCredits { get; set; }
    }
}
