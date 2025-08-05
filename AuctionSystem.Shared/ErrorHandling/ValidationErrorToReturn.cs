namespace AuctionSystem.Shared.ErrorHandling
{
    public class ValidationErrorToReturn
    {
        public IEnumerable<ValidationError> ValidationErrors { get; set; } = [];
    }
}
