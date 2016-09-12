using System;

namespace MockingJay.Validation
{
    public class UrlValidator : IValidator
    {
        public string ErrorMessage { get; protected set; }

        public bool IsValidConfiguration(Configuration configuration)
        {
            string url = configuration.Url;
            if(!Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                ErrorMessage = $"This: {url} is not valid url.";
                return false;
            }
            return true;
        }
    }
}