//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using AutoMapper;
//using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;
//using WeddingForward.ApplicationServices.Accounts.Models;
//using WeddingForward.ApplicationServices.Helpers;
//using WeddingForward.Data;
//using WeddingForward.Data.Models;

//namespace WeddingForward.ApplicationServices.Accounts.Commands.Handlers
//{
//    internal class StoreSystemAccountInfoCommandHandler: DataBaseRequestHandler<StoreSystemAccountInfoCommand, bool>
//    {
//        public StoreSystemAccountInfoCommandHandler(IMapper mapper, WeddingForwardContext context)
//            : base(mapper, context)
//        {
//        }

//        public override async Task<bool> ExecuteAsync(StoreSystemAccountInfoCommand request)
//        {
//            SystemAccounts systemAccounts = await Context.SystemAccounts
//                .Where(accounts => accounts.Login.Equals(request.Login)).FirstOrDefaultAsync()
//                .ConfigureAwait(false);

//            byte[] authDataBytes = TransformToAuthDataBytes(request.Pass, request.Session);

//            if (systemAccounts != null)
//            {
//                systemAccounts.Session = authDataBytes;

//                Context.Update(systemAccounts);
//            }
//            else
//            {
//                systemAccounts = new SystemAccounts
//                {
//                    Login = request.Login,
//                    Session = authDataBytes
//                };

//                Context.SystemAccounts.Add(systemAccounts);
//            }

//            return await Context.SaveChangesAsync().ConfigureAwait(false) > 0;
//        }

//        private byte[] TransformToAuthDataBytes(string pass, AccountAuthData authData)
//        {
//            var authDataInternal = new 
//            {
//                Pass = pass,
//                Session = authData
//            };

//            string serializeObject = JsonConvert.SerializeObject(authDataInternal);

//            return Encoding.UTF8.GetBytes(serializeObject);
//        }
//    }
//}
