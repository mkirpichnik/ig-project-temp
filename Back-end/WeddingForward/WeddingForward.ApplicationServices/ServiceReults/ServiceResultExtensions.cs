namespace WeddingForward.ApplicationServices.ServiceReults
{
    public static class ServiceResultExtensions
    {
        public static ServiceResult<TErrorResult> CreateErrorResult<TResult, TErrorResult>(
            this ServiceResult<TResult> serviceResult)
        {
            return new ServiceErrorResult<TErrorResult>(serviceResult.ErrorMessage);
        }
    }
}
