﻿namespace CookWithMe.Common.Exceptions
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
