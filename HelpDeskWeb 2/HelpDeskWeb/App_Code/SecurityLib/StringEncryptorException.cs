using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Basic exception for any problems that occure encryping a string
/// </summary>
/// 

namespace SecurityLib
{
    public class StringEncryptorException : Exception
    {
        public StringEncryptorException(string message)
            : base(message)
        {

        }
    }
}