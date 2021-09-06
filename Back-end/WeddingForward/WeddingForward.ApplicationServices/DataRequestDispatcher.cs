using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WeddingForward.ApplicationServices.ServiceReults;

namespace WeddingForward.ApplicationServices
{
    internal class DataRequestDispatcher : IDataRequestDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public DataRequestDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<ServiceResult<TResult>> ExecuteAsync<TRequest, TResult>(TRequest request) where TRequest : IDataRequest<TResult>
        {
            try
            {
                IDataRequestHandler<TRequest, TResult> dataRequestHandler = _serviceProvider.GetService<IDataRequestHandler<TRequest, TResult>>();
                if (dataRequestHandler == null)
                {
                    return new ServiceErrorResult<TResult>("Could not resolve request handler.");
                }

                TResult result = await dataRequestHandler.ExecuteAsync(request).ConfigureAwait(false);

                return new SuccessServiceResult<TResult>(result);
            }
            catch (Exception e)
            {
                return new ServiceErrorResult<TResult>(e.Message)
                {
                    Exception = e
                };
            }
        }
    }
}
