using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WeddingForward.Api.Controllers.Extensions;
using WeddingForward.Api.Extensions;
using WeddingForward.ApplicationServices.Accounts;
using WeddingForward.ApplicationServices.Accounts.Models;
using WeddingForward.ApplicationServices.ServiceReults;

namespace WeddingForward.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsService _accountsService;

        private readonly ILogger<AccountsController> _logger;

        public AccountsController(IAccountsService accountsService, ILogger<AccountsController> logger)
        {
            _accountsService = accountsService;
            _logger = logger;
        }

        [HttpGet("{accountName}")]
        public async Task<IActionResult> SearchForAccounts(string accountName)
        {
            _logger.LogDebug($"Query information about {accountName}...");

            ServiceResult<Account> serviceResult =
                await _accountsService.SearchForAccount(accountName)
                    .ConfigureAwait(false);

            _logger.LogDebug($"Result about {accountName}:\n {JsonConvert.SerializeObject(serviceResult)}");

            serviceResult.ThrowExceptionOnErrorResult();

            if (serviceResult.ResultType == ServiceResultType.Error)
            {
                return BadRequest(JsonConvert.SerializeObject(serviceResult));
            }

            Account result = serviceResult.Result;

            return Ok(result);
        }
    }
}