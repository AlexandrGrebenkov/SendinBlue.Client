using System;

namespace SendinBlue.Client.Models
{
    /// <summary>
    /// Details of a list.
    /// </summary>
    public class ListDetails
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// List name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Count of contacts.
        /// </summary>
        public int TotalSubscribers { get; set; }

        /// <summary>
        /// Folder id.
        /// </summary>
        public int FolderId { get; set; }

        /// <summary>
        /// Date of creation.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
