using System.Net;

namespace CheckBikeTheftAPI.CheckBikeTheftAPI.Data.Exceptions;

public class HttpErrorStatusCodeException : HttpRequestException
{
    public HttpErrorStatusCodeException(HttpStatusCode errorStatusCode)
    {
        ErrorStatusCode = errorStatusCode;
    }
    public HttpStatusCode ErrorStatusCode { get; set; }
}