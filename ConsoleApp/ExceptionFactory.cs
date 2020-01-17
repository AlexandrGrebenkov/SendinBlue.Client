using System;
using System.Runtime.CompilerServices;
using RestSharp;
using SendinBlue.Client;

namespace ConsoleApp
{
    public class ExceptionFactory : IExceptionFactory
    {
        public Exception CreateException(IRestResponse response, [CallerMemberName] string methodName = null)
        {
            throw new Exception(response.StatusDescription);
        }
    }
}
