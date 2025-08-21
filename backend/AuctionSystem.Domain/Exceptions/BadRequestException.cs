namespace AuctionSystem.Domain.Exceptions
{
    public class BadRequestException : Exception
    {
        public List<string> Errors { get; }

        public BadRequestException(string message)
            : base(message)
        {
        }

        public BadRequestException(List<string> errors)
            : base("Bad Request")
        {
            Errors = errors;
        }
    }
}
