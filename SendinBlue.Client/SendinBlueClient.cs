using System;
using System.Collections.Generic;
using System.Linq;
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
            var response = await client.ExecuteAsync<ContactAttributes>(request, cancellationToken);

            if (!response.IsSuccessful)
                throw exceptionFactory.CreateException(response);
            return response.Data.Attributes;
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

            var response = await client.ExecuteAsync(request, cancellationToken);
            if (!response.IsSuccessful)
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
            if (!response.IsSuccessful)
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
            var response = await client.ExecuteAsync<FoldersList>(request, cancellationToken);

            if (!response.IsSuccessful)
                throw exceptionFactory.CreateException(response);
            return response.Data.Folders;
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
            if (!response.IsSuccessful)
                throw exceptionFactory.CreateException(response);

            return response.Data.Id;
        }

        /// <inheritdoc/>
        public async Task<ListDetails> GetListDetails(int listId, CancellationToken cancellationToken)
        {
            var request = new RestRequest(Method.GET);
            request.Resource = $"contacts/lists/{listId}";
            var response = await client.ExecuteAsync<ListDetails>(request, cancellationToken);

            if (!response.IsSuccessful)
            {
                throw exceptionFactory.CreateException(response);
            }
            return response.Data;
        }

        /// <inheritdoc/>
        public async Task WaitForImportingDone(int contactsCount, int listId, CancellationToken cancellationToken)
        {
            while (true)
            {
                var info = await GetListDetails(listId, cancellationToken);
                if (info.TotalSubscribers == contactsCount)
                {
                    return;
                }
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
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
            var response = await client.ExecuteAsync<EmailTemplatesList>(request, cancellationToken);

            if (!response.IsSuccessful)
                throw exceptionFactory.CreateException(response);
            return response.Data.Templates;
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

            var response = await client.ExecuteAsync(request, cancellationToken);
            if (!response.IsSuccessful)
                throw exceptionFactory.CreateException(response);
        }

        /// <summary>
        /// Create the email campaign.
        /// </summary>
        /// <param name="campaign">Campaign data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Campaign id.</returns>
        public async Task<int> CreateEmailCampaign(CreatingEmailCampaign campaign, CancellationToken cancellationToken)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "emailCampaigns";
            request.AddHeader("accept", "application/json");
            request.AddHeader("content-type", "application/json");
            var json = ToJson(campaign);
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            var response = await client.ExecuteAsync<IdDto>(request, cancellationToken);

            if (!response.IsSuccessful)
            {
                throw exceptionFactory.CreateException(response);
            }
            return response.Data.Id;
        }

        /// <inheritdoc/>
        public async Task SendEmailCampaign(int campaignId, CancellationToken cancellationToken)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = $"emailCampaigns/{campaignId}/sendNow";
            request.AddHeader("accept", "application/json");
            request.AddHeader("content-type", "application/json");

            var response = await client.ExecuteAsync(request, cancellationToken);

            if (!response.IsSuccessful)
            {
                throw exceptionFactory.CreateException(response);
            }
        }

        /// <summary>
        /// Get all sender addresses.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        public Task<IEnumerable<EmailSender>> GetEmailSenders(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<string> SendTransactionalEmail(TransactionalEmail email, CancellationToken cancellationToken)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = $"smtp/email";
            request.AddHeader("accept", "application/json");
            var json = ToJson(email);
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            var response = await client.ExecuteAsync<string>(request, cancellationToken);

            if (!response.IsSuccessful)
            {
                throw exceptionFactory.CreateException(response);
            }
            return response.Data;
        }

        #endregion

        #region SMS

        /// <inheritdoc/>
        public async Task<int> CreateSmsCampaign(CreatingSmsCampaign campaign, CancellationToken cancellationToken)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "smsCampaigns";
            request.AddHeader("accept", "application/json");
            request.AddHeader("content-type", "application/json");
            var json = ToJson(campaign);
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            var response = await client.ExecuteAsync<IdDto>(request, cancellationToken);

            if (!response.IsSuccessful)
            {
                throw exceptionFactory.CreateException(response);
            }
            return response.Data.Id;
        }

        /// <inheritdoc/>
        public async Task SendSmsCampaign(int campaignId, CancellationToken cancellationToken)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = $"smsCampaigns/{campaignId}/sendNow";
            request.AddHeader("accept", "application/json");

            var response = await client.ExecuteAsync(request, cancellationToken);

            if (!response.IsSuccessful)
            {
                throw exceptionFactory.CreateException(response);
            }
        }

        /// <inheritdoc/>
        public async Task<string> SendTransactionalSms(TransactionalSms sms, CancellationToken cancellationToken)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = $"transactionalSMS/sms";
            request.AddHeader("accept", "application/json");
            var json = ToJson(sms);
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            var response = await client.ExecuteAsync<TransactionalSmsResponse>(request, cancellationToken);

            if (!response.IsSuccessful)
            {
                throw exceptionFactory.CreateException(response);
            }
            return response.Data.MessageId.ToString();
        }


        #endregion

        private string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, serializerSettings);
        }
    }
}
