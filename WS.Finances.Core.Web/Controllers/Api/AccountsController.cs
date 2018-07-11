using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WS.Finances.Core.Lib.Models;
using WS.Finances.Core.Lib.Services;

namespace WS.Finances.Core.Web.Controllers.Api
{
    [Route("api/[controller]")]
    public class AccountsController : Controller
    {
        private readonly AccountService _accountService;

        public AccountsController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_accountService.Get());
        }

    }
}
