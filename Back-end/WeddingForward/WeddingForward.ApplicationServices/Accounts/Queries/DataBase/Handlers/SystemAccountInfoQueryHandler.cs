//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;
//using WeddingForward.ApplicationServices.Accounts.Models;
//using WeddingForward.Data;
//using WeddingForward.Data.Models;

//namespace WeddingForward.ApplicationServices.Accounts.Queries.DataBase.Handlers
//{
//    internal class SystemAccountInfoQueryHandler: IDataRequestHandler<SystemAccountInfoQuery, SystemAccount>
//    {
//        private readonly WeddingForwardContext _db;

//        public SystemAccountInfoQueryHandler(WeddingForwardContext db)
//        {
//            _db = db;
//        }

//        public async Task<SystemAccount> ExecuteAsync(SystemAccountInfoQuery request)
//        {
//            SystemAccounts systemAccounts = await _db.SystemAccounts
//                .FirstOrDefaultAsync(accounts => accounts.Login.Equals(request.Login))
//                .ConfigureAwait(false);

//            if (systemAccounts == null)
//            {
//                return null;
//            }

//            AuthDataInternal authDataBytes = TransformFromAuthDataBytes(systemAccounts.Session);

//            return new SystemAccount
//            {
//                Pass = authDataBytes.Pass,
//                Login = request.Login,
//                Session = authDataBytes.Session,
//                IsBlocked = systemAccounts.IsBlocked
//            };
//        }

//        private AuthDataInternal TransformFromAuthDataBytes(byte[] bytes)
//        {
//            string json = Encoding.UTF8.GetString(bytes);

//            return JsonConvert.DeserializeObject<AuthDataInternal>(json);
//        }

//        private class AuthDataInternal
//        {
//            public string Pass { get; set; }

//            public AccountAuthData Session { get; set; }
//        }
//    }
//}
