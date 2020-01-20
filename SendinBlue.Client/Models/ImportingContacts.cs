namespace SendinBlue.Client.Models
{
    /// <summary>
    /// DTO for importing contacts list.
    /// </summary>
    public class ImportingContacts
    {
        /// <summary>
        /// CSV content to be imported. Use semicolon to separate multiple attributes.
        /// </summary>
        public string FileBody { get; set; }

        /// <summary>
        /// List params
        /// </summary>
        public NewContactsList NewList { get; set; }

        /// <summary>
        /// Ids of the lists in which the contacts shall be imported. 
        /// </summary>
        public int[] ListIds { get; set; }

        /// <summary>
        /// To blacklist all the contacts for email.
        /// </summary>
        public bool EmailBlacklist => false;

        /// <summary>
        /// To blacklist all the contacts for sms.
        /// </summary>
        public bool SmsBlacklist => false;

        /// <summary>
        /// To facilitate the choice to update the existing contacts.
        /// </summary>
        public bool UpdateExistingContacts => true;

        /// <summary>
        /// To facilitate the choice to erase any attribute of the existing contacts 
        /// with empty value. emptyContactsAttributes = true means the empty fields 
        /// in your import will erase any attribute that currently contain data in SendinBlue, 
        /// & emptyContactsAttributes = false means the empty fields will not affect your existing 
        /// data ( only available if updateExistingContacts set to true ).
        /// </summary>
        public bool EmptyContactsAttributes => false;
    }
}
