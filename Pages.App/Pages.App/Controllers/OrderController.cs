using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pages.App.Context;
using Pages.App.Services.Interfaces;
using Pages.Core.Entities;

namespace Pages.App.Controllers
{
 
    public class OrderController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly PagesDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IBasketService _basketService;



        public OrderController(UserManager<AppUser> userManager, PagesDbContext context, IHttpContextAccessor httpContext, IBasketService basketService)
        {
            _userManager = userManager;
            _context = context;
            _httpContext = httpContext;
            _basketService = basketService;
        }


        public async Task<IActionResult> CheckOut()
        {
            AppUser appUser = null;
            if (User.Identity.IsAuthenticated)
            {
                appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            else
            {
                return RedirectToAction("index", "home");
            }
            var baskets = await _context.Baskets.Where(x => !x.IsDeleted && x.AppUserId == appUser.Id)
                .Include(x => x.BasketItems.Where(y => !y.IsDeleted))
                .ThenInclude(x => x.Book)
                .FirstOrDefaultAsync();
            if(baskets is null)
            {
                return RedirectToAction("index", "book");
            }
            //double totalPrice = 0;
            //foreach(var item in baskets.BasketItems)
            //{
            //    totalPrice += item.BookCount * item.Book.Price;
            //}
            //Order order = new Order
            //{
            //    AppUserId = baskets.AppUserId,
            //    TotalPrice = totalPrice,
            //    isPaid = false
            //};
            //await _context.Orders.AddAsync(order);
            //foreach (var item in baskets.BasketItems)
            //{
            //    OrderItem orderitem = new OrderItem
            //    {
            //        BookId = item.BookId,
            //        Order = order,
            //        ProductCount = item.BookCount
            //    };
            //    await _context.OrderItems.AddAsync(orderitem);
            //}
            //await _context.SaveChangesAsync();
            return View(baskets);
        }

        public async Task<IActionResult> CreateOrder()
        {
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var basket = _context.Baskets.Where(x => !x.IsDeleted && x.AppUserId == appUser.Id)
                .Include(x => x.BasketItems.Where(y => !y.IsDeleted))
                .ThenInclude(x => x.Book)
                .Include(x => x.BasketItems.Where(y => !y.IsDeleted))
                .ThenInclude(x => x.Book)
                .FirstOrDefault();


            if (basket == null || basket.BasketItems.Count() == 0)
            {
                TempData["Empty Basket"] = "Basket Is Empty";
                return RedirectToAction(nameof(Index));
            }
            Order order = new Order
            {
                AppUserId = appUser.Id,
                CreatedDate = DateTime.Now,
                isPaid=false
            };
            decimal totalprice = 0;
            foreach (var item in basket.BasketItems)
            {
                totalprice += (int)item.Book.Price;
                OrderItem orderItem = new OrderItem
                {
                    Order = order,
                    CreatedDate = DateTime.Now,
                    BookId = item.BookId,
                    ProductCount = item.BookCount
                };
                _context.Add(orderItem);
            }
            order.TotalPrice = (double)totalprice;
            _context.Add(order);
            basket.IsDeleted = true;
            _context.SaveChanges();
            Order orderId = await _context.Orders.Where(x=>!x.IsDeleted && !x.isPaid && x.AppUserId==appUser.Id).FirstOrDefaultAsync();
            TempData["Order Created"] = "Order succesfully created";
            return RedirectToAction("index", "payment", new { @id = orderId.Id });
        }
        //public async Task<IActionResult> RemoveBasket(int id)
        //{
        //    AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
        //    var basket = _context.Baskets.Where(x => !x.IsDeleted && x.AppUserId == appUser.Id)
        //        .Include(x => x.BasketItems.Where(y => !y.IsDeleted))
        //        .ThenInclude(x => x.Book)
        //        .Include(x => x.BasketItems.Where(y => !y.IsDeleted))
        //        .ThenInclude(x => x.Book)
        //        .FirstOrDefault();

        //    await _basketService.Remove(id);
        //    return RedirectToAction(nameof(Index));
        //}
      
        [HttpPost]
        public async Task<IActionResult> Increase(int? id)
        {
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);

            var baskets = await _context.Baskets.FirstOrDefaultAsync(x => x.AppUserId == appUser.Id);

            if (baskets is null)
            {
                return NotFound("Basket is not found");
            }

            var basketProduct = await _context.BasketItems.FirstOrDefaultAsync(x => !x.IsDeleted && x.BookId == id && x.BasketId == baskets.Id);
            if (basketProduct is null)
            {
                return NotFound("Basket item cannot be found");
            }

            var books = await _context.Books.FindAsync(basketProduct.BookId);
            if (books is null)
            {
                return NotFound("Product is not found");
            }

            basketProduct.BookCount++;
            _context.BasketItems.Update(basketProduct);
            await _context.SaveChangesAsync();

            decimal totalPrice = basketProduct.BookCount * (decimal)books.Price;

            return Ok(new { TotalPrice = totalPrice });
        }

        [HttpPost]
        public async Task<IActionResult> Decrease(int? id)
        {
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);

            var baskets = await _context.Baskets.FirstOrDefaultAsync(x => x.AppUserId == appUser.Id);

            if (baskets is null)
            {
                return NotFound("Basket is not found");
            }

            var basketProduct = await _context.BasketItems.FirstOrDefaultAsync(x => !x.IsDeleted && x.BookId == id && x.BasketId == baskets.Id);
            if (basketProduct is null)
            {
                return NotFound("Basket item cannot be found");
            }

            var books = await _context.Books.FindAsync(basketProduct.BookId);
            if (books is null)
            {
                return NotFound("Product is not found");
            }

            if (basketProduct.BookCount < 2)
            {
                return NotFound("You cannot decrease product count less than 1");
            }

            basketProduct.BookCount--;
            _context.BasketItems.Update(basketProduct);
            await _context.SaveChangesAsync();

            decimal totalPrice = basketProduct.BookCount * (decimal)books.Price;

            return Ok(new { TotalPrice = totalPrice });
        }

    }
}
