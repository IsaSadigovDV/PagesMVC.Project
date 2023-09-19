using Braintree;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pages.App.Context;
using Pages.App.Services.Interfaces;
using Pages.App.ViewModels;
using Pages.Core.Entities;

namespace Pages.App.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IBraintreeService _braintreeService;
        private readonly PagesDbContext _context;

        public PaymentController(IBraintreeService braintreeService, PagesDbContext context)
        {
            _braintreeService = braintreeService;
            _context = context;
        }

        public async Task<IActionResult> Index(int id)
        {
            var gateway = _braintreeService.GetGateway();
            var clientToken = gateway.ClientToken.Generate(); 
            ViewBag.ClientToken = clientToken;
            Order? order = await _context.Orders.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            var data = new BookPurchaseVM
            {
                Id =id,
                TotalPrice = order.TotalPrice,
            };

            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Create(BookPurchaseVM model)
        {
            var gateway = _braintreeService.GetGateway();
            Order? order = await _context.Orders.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            var request = new TransactionRequest
            {
                Amount = Convert.ToDecimal(order.TotalPrice),
                PaymentMethodNonce = model.Nonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);

            if (result.IsSuccess())
            {
                order.isPaid = true;
                await _context.SaveChangesAsync();
                return RedirectToAction("index","home");
            }
            else
            {
                return View("Failure");
            }
        }
    }
}

