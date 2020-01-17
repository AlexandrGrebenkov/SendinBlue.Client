using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using SendinBlue.Client.Models;

namespace SendinBlue.Client
{
    public class SendinBlueClient
    {
        private const string apiUrl = "https://api.sendinblue.com/v3/";
        private readonly RestClient client;
        private readonly IExceptionFactory exceptionFactory;
        private readonly JsonSerializerSettings serializerSettings;

        public SendinBlueClient(string apiKey, IExceptionFactory exceptionFactory)
        {
            this.exceptionFactory = exceptionFactory;

            client = new RestClient(apiUrl);
            client.Authenticator = new ApiKeyAuthenticator(apiKey);

            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver
            };
        }

        #region Contacts

        /// <summary>
        /// Get list of all attributes.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task<IEnumerable<ContactAttribute>> GetContactAttributesAsync(CancellationToken cancellationToken)
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "contacts/attributes";
            var response = await client.ExecuteAsync<ContactAttributes>(request);

            if (response.StatusCode == HttpStatusCode.OK)
                return response.Data.Attributes;
            throw exceptionFactory.CreateException(response);
        }

        /// <summary>
        /// Create contact attribute.
        /// </summary>
        /// <param name="attribute">Attribute.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task CreateContactAttributeAsync(ContactAttribute attribute, CancellationToken cancellationToken)
        {
            var request = new RestRequest(Method.POST);
            var attributeCategory = attribute.Category;
            var attributeName = attribute.Name;
            request.Resource = $"contacts/attributes/{attributeCategory}/{attributeName}";
            request.AddHeader("accept", "application/json");
            request.AddHeader("content-type", "application/json");
            var json = ToJson(new CreatingAttribute() { Type = attribute.Type });
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            var response = await client.ExecuteAsync(request);
            if (response.StatusCode >= HttpStatusCode.BadRequest)
                throw exceptionFactory.CreateException(response);
        }

        /// <summary>
        /// Import contact to new list.
        /// </summary>
        /// <param name="contacts">Contacts.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
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
        public async Task<IEnumerable<EmailTemplateSummary>> GetEmailTemplatesListAsync(CancellationToken cancellationToken)
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "smtp/templates";
            var response = await client.ExecuteAsync<EmailTemplatesList>(request);

            if (response.StatusCode == HttpStatusCode.OK)
                return response.Data.Templates;
            throw exceptionFactory.CreateException(response);
        }

        /// <summary>
        /// Create email campaigns list.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task CreateEmailCampaignsAsync(CreatingEmailCampaign campaign, CancellationToken cancellationToken)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = $"emailCampaigns";
            request.AddHeader("accept", "application/json");
            request.AddHeader("content-type", "application/json");
            var json = ToJson(campaign);
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            var response = await client.ExecuteAsync(request);
            if (response.StatusCode >= HttpStatusCode.BadRequest)
                throw exceptionFactory.CreateException(response);
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

        private string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, serializerSettings);
        }
    }
}
