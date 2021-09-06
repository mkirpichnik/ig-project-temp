using WeddingForward.ApplicationServices.ServiceReults;

namespace WeddingForward.Api.Controllers.Extensions
{
    public static class ServiceResultExtensions
    {
        public static void ThrowExceptionOnErrorResult<TResult>(this ServiceResult<TResult> serviceResult)
        {
            if (serviceResult.ResultType == ServiceResultType.Error)
            {
                throw serviceResult.Exception;
            }
        }
    }
}
