using System.Collections.Generic;

namespace SendinBlue.Client.Models
{
    public class FoldersList
    {
        /// <summary>
        /// Count of folders.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Folders.
        /// </summary>
        public IEnumerable<Folder> Folders { get; set; }
    }
}
