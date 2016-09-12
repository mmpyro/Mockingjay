namespace MockingJay.Validation
{
    public interface IValidator
    {
        string ErrorMessage { get; }

        bool IsValidConfiguration(Configuration configuration);
    }
}