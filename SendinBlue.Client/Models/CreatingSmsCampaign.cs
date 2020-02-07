namespace SendinBlue.Client.Models
{
    /// <summary>
    /// DTO for creates an SMS campaign.
    /// </summary>
    public class CreatingSmsCampaign
    {
        /// <summary>
        /// Name of the campaign.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name of the sender. The number of characters is limited to 11.
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// Content of the message. The maximum characters used per SMS is 160,
        /// if used more than that, it will be counted as more than one SMS.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Recipients.
        /// </summary>
        public Recipients Recipients { get; set; }
    }
}
