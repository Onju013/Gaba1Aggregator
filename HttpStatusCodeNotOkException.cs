using System;
using System.Net;

namespace Gaba1Aggregator
{
    public class HttpStatusCodeNotOkException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public HttpStatusCodeNotOkException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
    }
}