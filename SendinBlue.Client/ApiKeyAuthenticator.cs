using RestSharp;
using RestSharp.Authenticators;

namespace SendinBlue.Client
{
    public class ApiKeyAuthenticator : IAuthenticator
    {
        private readonly string apiKey;

        public ApiKeyAuthenticator(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            request.AddHeader("api-key", apiKey);
        }
    }
}
