using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WS.Finances.Core.Lib.Models;
using WS.Finances.Core.Lib.Services;

namespace WS.Finances.Core.Web.Controllers.Api
{
    [Route("api/[controller]")]
    public class CsvController : Controller
    {
        private readonly CsvService _csvService;
        private readonly TransactionService _transactionService;

        public CsvController(CsvService csvService, TransactionService transactionService)
        {
            _csvService = csvService;
            _transactionService = transactionService;
        }

        [HttpPost("{year}/{month}/{accountName}")]
        public IActionResult UploadFiles(int year, int month, string accountName)

        {
            var files = Request.Form.Files;
            if (files.Count != 1)
            {
                return BadRequest("Please supply one, and only one, CSV file to process");
            }
            using (var stream = files[0].OpenReadStream())
            {
                var transactions = _csvService.GetCsvData(year, month, accountName, stream, files[0].FileName, true).ToList();
                _transactionService.Clear(year, month, accountName);
                _transactionService.Put(transactions);
                return Ok(new
                {
                    Mapped = transactions.Where(t => !string.IsNullOrEmpty(t.Category)),
                    Unmapped = transactions.Where(t => string.IsNullOrEmpty(t.Category))
                });
            }
        }
    }
}
