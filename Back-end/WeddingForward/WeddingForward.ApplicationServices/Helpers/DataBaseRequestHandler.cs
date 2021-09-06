using System.Threading.Tasks;
using AutoMapper;
using WeddingForward.Data;

namespace WeddingForward.ApplicationServices.Helpers
{
    internal abstract class DataBaseRequestHandler<TRequest, TResult>: IDataRequestHandler<TRequest, TResult> where TRequest : IDataRequest<TResult>
    {
        protected IMapper Mapper { get; private set; }

        protected WeddingForwardContext Context { get; private set; }

        protected DataBaseRequestHandler(IMapper mapper, WeddingForwardContext context)
        {
            Mapper = mapper;
            Context = context;
        }

        public abstract Task<TResult> ExecuteAsync(TRequest request);
    }
}
