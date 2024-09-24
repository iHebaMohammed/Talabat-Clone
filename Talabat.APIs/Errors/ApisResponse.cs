
namespace Talabat.APIs.Errors
{
    public class ApisResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ApisResponse(int statusCode, string message = null) 
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request, you have made",
                401 => "Autherized, you are not",
                404 => "Resource found, it was not",
                500 => "Errors are the path to the dark side. Errors lead to anger. Anger lead to hate",
                _ => null
            };
        }
    }
}
