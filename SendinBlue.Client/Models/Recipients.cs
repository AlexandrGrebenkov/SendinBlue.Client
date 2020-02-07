namespace SendinBlue.Client.Models
{
    /// <summary>
    /// List ids to include/exclude from campaign
    /// </summary>
    public class Recipients
    {
        /// <summary>
        /// List Ids to send the campaign to.
        /// </summary>
        public int[] ListIds { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="listId">List of contacts.</param>
        public Recipients(int listId)
        {
            ListIds = new int[] { listId };
        }
    }
}
