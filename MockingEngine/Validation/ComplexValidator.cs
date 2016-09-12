namespace MockingJay.Validation
{
    public class ComplexValidator : IValidator
    {
        private readonly IValidator[] _validators;

        public ComplexValidator(params IValidator[] validators)
        {
            _validators = validators;
        }

        public string ErrorMessage { get; protected set; }

        public bool IsValidConfiguration(Configuration configuration)
        {
            bool result = true;
            foreach (var validator in _validators)
            {
                if (!validator.IsValidConfiguration(configuration))
                {
                    result = false;
                    ErrorMessage = validator.ErrorMessage;
                    break;
                }
            }
            return result;
        }
    }
}