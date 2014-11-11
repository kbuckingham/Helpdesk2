using System;

//the generic exception for credit card encryption

namespace SecurityLib
{
    public class SecureCardException : Exception
    {
        public SecureCardException(string message)
            : base(message)
        {

        }
    }
}