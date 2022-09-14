using System;


namespace ClearApplicationFoundation.Exceptions
{
    public class DependencyRegistrationMissingException : Exception
    {
        public DependencyRegistrationMissingException() : base("A unknown dependency has not been registered with the DI container.")
        {

        }
        public DependencyRegistrationMissingException(string? message): base(message)
        {
            
        }
    }
}
