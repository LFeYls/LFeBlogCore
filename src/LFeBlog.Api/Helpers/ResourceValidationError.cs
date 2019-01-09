namespace LFeBlog.Web.Core.Helpers
{
    public class ResourceValidationError
    {
        public ResourceValidationError(string validatorKey, string message)
        {
            ValidatorKey = validatorKey;
            Message = message;
        }

        public string ValidatorKey { get; private set; }

        public string Message { get; private set; }
        
        

    }
}