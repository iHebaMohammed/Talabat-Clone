namespace Talabat.APIs.Errors
{
    public class ApisValidationErrorResponse : ApisResponse
    {
        public ApisValidationErrorResponse() : base(400)
        {
        }

        public IEnumerable<string> Errors { get; set; }

    }
}
