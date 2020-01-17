﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SendinBlue.Client.Models;

namespace SendinBlue.Client
{
    public class SendinBlueClient
    {
        #region Contacts

        /// <summary>
        /// Get list of all attributes.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        public Task<ContactAttribute> GetContactAttributesAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create contact attribute.
        /// </summary>
        /// <param name="attribute">Attribute.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public Task CreateContactAttributeAsync(ContactAttribute attribute, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Import contact to new list.
        /// </summary>
        /// <param name="contacts">Contacts.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns></returns>
        public Task ImportContactsAsync(IEnumerable<dynamic> contacts, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Email

        /// <summary>
        /// Get email templates list.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        public Task<IEnumerable<EmailTemplateSummary>> GetEmailTemplatesListAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create email campaigns list.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        public Task CreateEmailCampaignsAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Schedule the email campaign.
        /// </summary>
        /// <param name="campaignId">Campaign's id.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public Task ScheduleEmailCampaign(int campaignId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get all sender addresses.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        public Task<IEnumerable<EmailSender>> GetEmailSenders(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region SMS

        /// <summary>
        /// Send SMS for contact.
        /// </summary>
        /// <param name="telNumber">Telephone number (with country code).</param>
        /// <param name="smsBody">SMS message body.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public Task SendSms(string telNumber, string smsBody, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}