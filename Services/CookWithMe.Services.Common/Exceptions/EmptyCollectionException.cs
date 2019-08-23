namespace CookWithMe.Services.Common
{
    using System;

    public class EmptyCollectionException : Exception
    {
        public EmptyCollectionException(string message)
            : base(message)
        {

        }
    }
}
