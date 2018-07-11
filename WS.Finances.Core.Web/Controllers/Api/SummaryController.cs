using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WS.Finances.Core.Lib.Models;
using WS.Finances.Core.Lib.Services;

namespace WS.Finances.Core.Web.Controllers.Api
{
    [Route("api/[controller]")]
    public class SummaryController : Controller
    {
        private readonly SummaryService _summaryService;

        public SummaryController(SummaryService summaryService)
        {
            _summaryService = summaryService;
        }

        [HttpGet("{year}")]
        public IActionResult GetYear(int year)
        {
            return Ok(_summaryService.Get(year));
        }

        [HttpGet("{year}/{month}")]
        public IActionResult GetMonth(int year, int month)
        {
            return Ok(_summaryService.Get(year, month));
        }
    }
}
