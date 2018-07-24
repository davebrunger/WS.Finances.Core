using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WS.Finances.Core.Lib.Services;
using WS.Finances.Core.Web.Models;

namespace WS.Finances.Core.Web.Controllers.Api
{
    [Route("api/[controller]")]
    public class TransactionsController : Controller
    {
        private readonly TransactionService _transactionService;
        private readonly MapService _mapService;

        public TransactionsController(TransactionService transactionService, MapService mapService)
        {
            _transactionService = transactionService;
            _mapService = mapService;
        }

        [HttpGet("{year}/{month}/{accountName}")]
        public IActionResult GetTransactions(int year, int month, string accountName)
        {
            var transactions = _transactionService.Get(year, month, accountName).ToList();
            return Ok(new
            {
                Mapped = transactions.Where(t => !string.IsNullOrEmpty(t.Category)),
                Unmapped = transactions.Where(t => string.IsNullOrEmpty(t.Category))
            });
        }

        [HttpGet("{year}/{month}/{accountName}/{transactionId}")]
        public IActionResult GetTransactions(int year, int month, string accountName, int transactionId)
        {
            var transactions = _transactionService.Get(year, month, accountName, transactionId: transactionId).ToList();
            if (transactions.Count == 1)
            {
                return Ok(transactions[0]);
            }
            throw new ArgumentException("Supplied paramater did not uniquely identify a transaction");
        }

        [HttpGet("search")]
        public IActionResult Search(int year, string descriptionPattern)
        {
            var transactions = _transactionService.Get(year, null, null, null, descriptionPattern).ToList();
            var transactionCount = transactions.Count;
            var top100Transactions = transactions
                .OrderBy(t => t.Timestamp)
                .Take(100)
                .ToList();
            return Ok(new
            {
                Mapped = top100Transactions.Where(t => !string.IsNullOrEmpty(t.Category)),
                Unmapped = top100Transactions.Where(t => string.IsNullOrEmpty(t.Category)),
                TransactionCount = transactionCount
            });
        }

        [HttpPost("map/{year}/{month}/{accountName}")]
        public IActionResult MapTransaction(int year, int month, string accountName, MapTransactionRequest request)
        {
            if (_mapService.Get().All(m => m.Category != request.Category))
            {
                throw new ArgumentException($"Unknown category: {request.Category}", nameof(request));
            }
            var transactionsToMap = _transactionService.Get(year, month, accountName, descriptionPattern: request.Pattern);
            foreach (var transaction in transactionsToMap)
            {
                transaction.Category = request.Category;
                _transactionService.Put(transaction);
            }
            return Ok();
        }
    }
}
