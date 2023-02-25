namespace CompanyName.Model.Exceptions
{
    /// <summary>
    /// Exception for throwing not found error (ie 404).
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
}
