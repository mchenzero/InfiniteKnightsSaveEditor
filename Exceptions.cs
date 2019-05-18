using System;

namespace InfiniteKnightsSaveEditor
{
    public class InvalidOptionException : Exception
    {
        public InvalidOptionException(string message) : base(message) { }
    }

    public class AbortedException : Exception
    {
        public AbortedException() : base("Operation aborted.") { }

        public AbortedException(string message) : base(message) { }
    }
}
