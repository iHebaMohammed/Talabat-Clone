namespace Talabat.APIs.Errors
{
    public class ApisExceptionResponse : ApisResponse
    {
        public string Details { get; set; }

        public ApisExceptionResponse(int statusCode , string message = null , string details = null): base(statusCode , message) 
        {
            this.Details = details;
        }
    }
}
