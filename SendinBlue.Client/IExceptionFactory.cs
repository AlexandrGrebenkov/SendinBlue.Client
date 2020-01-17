using System;
using RestSharp;

namespace SendinBlue.Client
{
    public interface IExceptionFactory
    {
        /// <summary>
        /// Exception factory.
        /// </summary>
        /// <param name="methodName">Method name.</param>
        /// <param name="response">Response.</param>
        Exception CreateException(IRestResponse response, string methodName = null);
    }
}
