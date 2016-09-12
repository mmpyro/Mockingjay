
namespace MockingJay.Validation
{
    public class NullResponseValidator : IValidator
    {
        public string ErrorMessage { get; protected set; }

        public bool IsValidConfiguration(Configuration configuration)
        {
            var response = configuration.Return;
            if (response == null)
            {
                ErrorMessage = "Return object cannot be null.";
                return false;
            }
            return true;
        }
    }
}