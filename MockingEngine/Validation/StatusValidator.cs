namespace MockingJay.Validation
{
    public class StatusValidator : IValidator
    {
        public string ErrorMessage { get; protected set; }

        public bool IsValidConfiguration(Configuration configuration)
        {
            int statusCode = configuration.Return.StatusCode;
            if (statusCode >= 100 && statusCode <= 511)
                return true;
            else
            {
                ErrorMessage = $"This: {statusCode} is not valid. Valid value is between 100 and 511.";
                return false;
            }
        }
    }
}