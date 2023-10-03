using System;

namespace Everyone
{
    /// <summary>
    /// An <see cref="Exception"/> that occurs when an operation is performed on an empty object.
    /// </summary>
    public class EmptyException : Exception
    {
        public EmptyException()
        {
        }

        public EmptyException(string message)
            : base(message)
        {
        }
    }
}
