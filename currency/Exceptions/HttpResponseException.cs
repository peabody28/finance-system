using System.Net;

namespace currency.Exceptions
{
    public class HttpResponseException : Exception
    {
        public HttpResponseException(HttpStatusCode statusCode, object? value = null) =>
            (StatusCode, Value) = (statusCode, value);

        public HttpStatusCode StatusCode { get; }

        public object? Value { get; }
    }
}
