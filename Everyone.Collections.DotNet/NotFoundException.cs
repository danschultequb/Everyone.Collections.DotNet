using System;

namespace Everyone
{
    /// <summary>
    /// An <see cref="Exception"/> that occurs when an operation is performed that can't find what
    /// it is looking for.
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException(string message)
            : base(message)
        {
        }
    }
}
