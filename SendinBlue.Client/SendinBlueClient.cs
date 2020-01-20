using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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
            if (attribute.Name == "EMAIL") return; // HACK: EMAIL attribute always exist but not returns by 'GET' contacts/attributes request.
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
        /// <returns>List id.</returns>
        public async Task<int> ImportContactsAsync(IEnumerable<IDictionary<string, object>> contacts, CancellationToken cancellationToken)
        {
            var attributes = GetHeaders(contacts);
            var attributesFromService = (await GetContactAttributesAsync(cancellationToken)).Select(_ => _.Name).ToList();
            var attributesToCreate = attributes.Except(attributesFromService).ToList();
            foreach (var attribute in attributesToCreate)
            {
                await CreateContactAttributeAsync(new ContactAttribute { Name = attribute }, cancellationToken);
            }

            var listId = await CreateListAsync(DateTimeOffset.UtcNow.ToString(), cancellationToken);

            var fileBody = CreateFileBody(contacts, attributes);

            var request = new RestRequest(Method.POST);
            request.Resource = "contacts/import";
            var dto = new ImportingContacts()
            {
                FileBody = fileBody,
                ListIds = new int[] { listId },
            };
            var json = ToJson(dto);
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            var response = await client.ExecuteAsync(request);
            if (response.StatusCode >= HttpStatusCode.BadRequest)
                throw exceptionFactory.CreateException(response);

            return listId;
        }

        private static IEnumerable<string> GetHeaders(IEnumerable<IDictionary<string, object>> contacts)
        {
            return contacts.First().Select(_ => _.Key.ToUpper());
        }

        private static string CreateFileBody(IEnumerable<IDictionary<string, object>> contacts, IEnumerable<string> attributes)
        {
            var builder = new StringBuilder();
            builder.AppendLine(string.Join(";", attributes));
            foreach (var contact in contacts)
            {
                builder.AppendLine(string.Join(";", contact.Values));
            }
            return builder.ToString();
        }

        /// <summary>
        /// Get list of all folders.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task<IEnumerable<Folder>> GetFoldersAsync(CancellationToken cancellationToken)
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "contacts/folders";
            var response = await client.ExecuteAsync<FoldersList>(request);

            if (response.StatusCode == HttpStatusCode.OK)
                return response.Data.Folders;
            throw exceptionFactory.CreateException(response);
        }

        /// <summary>
        /// Creates a contacts list.
        /// </summary>
        /// <param name="name">List name.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task<int> CreateListAsync(string name, CancellationToken cancellationToken)
        {
            var folder = (await GetFoldersAsync(cancellationToken))?.FirstOrDefault()
                ?? throw exceptionFactory.CreateException("The account has no folders for lists of lontacts. Create this first.");

            var request = new RestRequest(Method.POST);
            request.Resource = "contacts/lists";
            var dto = new NewContactsList { Name = name, FolderId = folder.Id };
            var json = ToJson(dto);
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            var response = await client.ExecuteAsync<IdDto>(request);
            if (response.StatusCode >= HttpStatusCode.BadRequest)
                throw exceptionFactory.CreateException(response);

            return response.Data.Id;
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
