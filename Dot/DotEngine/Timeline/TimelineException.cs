using System;

namespace DotEngine.Timeline
{
    public class TimelineException : Exception
    {
        public TimelineException(int error): base(TimelineConst.GetErrorMessage(error,null))
        {

        }

        public TimelineException(int error,params object[] values) : base(TimelineConst.GetErrorMessage(error,values))
        {

        }
    }
}
