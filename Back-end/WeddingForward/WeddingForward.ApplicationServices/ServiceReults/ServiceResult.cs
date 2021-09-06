using System;

namespace WeddingForward.ApplicationServices.ServiceReults
{
    public abstract class ServiceResult<TResult>
    {
        public TResult Result { get; protected set; }

        public abstract ServiceResultType ResultType { get; }

        public Exception Exception { get; set; }

        public string ErrorMessage { get; protected set; }
    }

    public class SuccessServiceResult<TResult> : ServiceResult<TResult>
    {
        public SuccessServiceResult(TResult result)
        {
            Result = result;
        }

        public override ServiceResultType ResultType { get; } = ServiceResultType.Success;
    }

    public class ServiceErrorResult<TResult> : ServiceResult<TResult>
    {
        public ServiceErrorResult(string error)
        {
            ErrorMessage = error;
        }

        public override ServiceResultType ResultType { get; } = ServiceResultType.Error;
    }

    public enum ServiceResultType
    {
        Success,
        Error
    }
}
