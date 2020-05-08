using System;

namespace Dot.Log
{
    public class LogUnkownException : Exception
    {
        public LogUnkownException():base("Unknown exception")
        {
        }
    }

    public class LogNotInitilizedException : Exception
    {
        public LogNotInitilizedException() : base("Logger is not initialized.")
        {
        }
    }
}
