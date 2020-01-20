namespace SendinBlue.Client.Models
{
    /// <summary>
    /// DTO for creating a new list of contacts.
    /// </summary>
    public class NewContactsList
    {
        /// <summary>
        /// List with listName will be created first and users will be imported in it.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Id of the folder where this new list shall be created.
        /// </summary>
        public int FolderId { get; set; }
    }
}
