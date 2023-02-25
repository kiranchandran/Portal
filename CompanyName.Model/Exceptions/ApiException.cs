namespace CompanyName.Model.Exceptions
{
    /// <summary>
    /// Exception for warpping all the business exceptions.
    /// </summary>
    public class ApiException : Exception
    {
        private readonly List<KeyValuePair<string, string>> errors;
        public ApiException() : base("Validation Errors")
        {
            errors = new List<KeyValuePair<string, string>>();
        }

        public ApiException(string message) : base(message)
        {
            errors = new List<KeyValuePair<string, string>>();
        }

        public IReadOnlyList<KeyValuePair<string, string>> Errors => errors;

        public void AddError(string key, string errorMessage)
        {
            errors.Add(new KeyValuePair<string, string>(key, errorMessage));
        }

        public bool HasErrors => (Errors != null && Errors.Any());
    }
}
