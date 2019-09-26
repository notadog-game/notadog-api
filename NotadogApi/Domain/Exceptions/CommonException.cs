using System;

namespace NotadogApi.Domain.Exceptions
{
    public class CommonException : Exception
    {
        public CommonError Error { get; }
        public CommonException(ErrorCode code)
        {
            Error = new CommonError(code);
        }
    }
}

