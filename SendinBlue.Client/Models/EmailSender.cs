namespace SendinBlue.Client.Models
{
    /// <summary>
    /// Email sender.
    /// </summary>
    public class EmailSender
    {
        /// <summary>
        /// Sender id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Sender name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Sender email.
        /// </summary>
        public string Email { get; set; }
    }
}
