using System;
using System.Runtime.CompilerServices;
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
        Exception CreateException(IRestResponse response, [CallerMemberName] string methodName = null);
        Exception CreateException(string message, [CallerMemberName] string methodName = null);
    }
}
