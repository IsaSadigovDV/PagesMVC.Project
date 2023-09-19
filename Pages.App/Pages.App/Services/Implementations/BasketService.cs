using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pages.App.Context;
using Pages.App.Services.Interfaces;
using Pages.App.ViewModels;
using Pages.Core.Entities;

namespace Pages.App.Services.Implementations
{
  
    public class BasketService : IBasketService
    {
        private readonly PagesDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserManager<AppUser> _usermanager;

        public BasketService(PagesDbContext context, IHttpContextAccessor httpContext, UserManager<AppUser> usermanager)
        {
            _context = context;
            _httpContext = httpContext;
            _usermanager = usermanager;
        }

        public async  Task AddBasket(int id)
        {

            if (!await _context.Books.AnyAsync(x => x.Id == id))
            {
                throw new Exception("Item not found");
            }

            if (_httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _usermanager.FindByNameAsync(_httpContext.HttpContext.User.Identity.Name);
                Basket? basket = await _context.Baskets.Where(x => !x.IsDeleted && x.AppUserId == appUser.Id).
                    Include(x => x.BasketItems.Where(y => !y.IsDeleted)).ThenInclude(x => x.Book).FirstOrDefaultAsync();

                if (basket == null)
                {

                    basket = new Basket
                    {
                        AppUserId = appUser.Id,
                        CreatedDate = DateTime.UtcNow.AddHours(4)
                    };
                    await _context.AddAsync(basket);

                    BasketItem basketItem = new BasketItem
                    {
                        Basket = basket,
                        BookId = id,
                        BookCount = 1

                    };
                    await _context.AddAsync(basketItem);
                }
                else
                {
                    BasketItem? basketItem = await _context.BasketItems.Include(x=>x.Basket).FirstOrDefaultAsync(x => x.BookId == id && !x.Basket.IsDeleted);

                    if (basketItem != null && !basketItem.IsDeleted)
                    {
                        basketItem.BookCount++;
                    }
                    else if (basketItem != null && basketItem.IsDeleted)
                    {
                        basketItem.BookCount=1;
                        basketItem.IsDeleted=false;
                    }
                    else
                    {

                        basketItem = new BasketItem
                        {
                            Basket = basket,
                            BookId = id,
                            BookCount = 1

                        };
                        await _context.AddAsync(basketItem);

                    }
                  

                }
                await _context.SaveChangesAsync();
            }
            else
            {
                var CookieJson = _httpContext?.HttpContext?.Request.Cookies["basket"];
                if (CookieJson == null)
                {
                    List<BasketViewModel> basketViewModels = new List<BasketViewModel>();
                    BasketViewModel basketViewModel = new BasketViewModel
                    {
                        BookId = id,
                        Count = 1
                    };
                    basketViewModels.Add(basketViewModel);
                    CookieJson = JsonConvert.SerializeObject(basketViewModels);

                    _httpContext?.HttpContext?.Response.Cookies.Append("basket", CookieJson);
                }
                else
                {
                    List<BasketViewModel>? basketViewModels = JsonConvert
                                    .DeserializeObject<List<BasketViewModel>>(CookieJson);
                    BasketViewModel? model =
                        basketViewModels.FirstOrDefault(x => x.BookId == id);
                    if (model != null)
                    {
                        model.Count++;
                    }
                    else
                    {
                        BasketViewModel basketViewModel = new();
                        basketViewModel.Count = 1;
                        basketViewModel.BookId = id;
                        basketViewModels.Add(basketViewModel);
                    }
                    CookieJson = JsonConvert.SerializeObject(basketViewModels);
                    _httpContext?.HttpContext?.Response.Cookies.Append("basket", CookieJson);
                }
            }

        }

        public async Task<List<BasketItemViewModel>> GetAllBaskets()
        {
            if (_httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _usermanager.FindByNameAsync(_httpContext.HttpContext.User.Identity.Name);
                Basket? basket = await _context.Baskets.Include(x => x.BasketItems.Where(y => !y.IsDeleted))
                                  .ThenInclude(x => x.Book)
                                  .Include(x => x.BasketItems)
                                  .ThenInclude(x => x.Book)
                                  .Where(x => !x.IsDeleted && x.AppUser.Id == appUser.Id).FirstOrDefaultAsync();

                if (basket != null)
                {
                    List<BasketItemViewModel> basketItemViewModels = new();
                    foreach (var item in basket.BasketItems)
                    {
                        basketItemViewModels.Add(new BasketItemViewModel
                        {
                            Image = item.Book.Image,
                            Count = item.BookCount,
                            Name = item.Book.Name,
                            BookId = item.BookId,
                            Price = (decimal)item.Book.Price
                        });
                    }
                    return basketItemViewModels;

                }
            }
            else
            {
                var jsonBasket = _httpContext?.HttpContext?.Request.Cookies["basket"];

                if (jsonBasket != null)
                {
                    List<BasketViewModel>? basketViewModels = JsonConvert
                             .DeserializeObject<List<BasketViewModel>>(jsonBasket);
                    List<BasketItemViewModel> basketItemViewModels = new();
                    foreach (var item in basketViewModels)
                    {
                        Book? book = await _context.Books
                                          .Where(x => !x.IsDeleted && x.Id == item.BookId)
                                           .FirstOrDefaultAsync();

                        if (book != null)
                        {
                            basketItemViewModels.Add(new BasketItemViewModel
                            {
                                BookId = item.BookId,
                                Count = item.Count,
                                Image = book.Image,
                                Name = book.Name,
                                Price = (decimal)book.Price
                            });

                        }
                    }
                    return basketItemViewModels;
                }
            }
            return new List<BasketItemViewModel>();
        }
        public async Task Remove(int id)
        {
            if (_httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _usermanager.FindByNameAsync(_httpContext.HttpContext.User.Identity.Name);
                Basket? basket = await _context.Baskets.Include(x => x.BasketItems.Where(y => !y.IsDeleted))
                                        .Where(x => !x.IsDeleted && x.AppUserId == appUser.Id).FirstOrDefaultAsync();


                if (basket != null)
                {
                    BasketItem? basketItem = basket.BasketItems.FirstOrDefault(x => x.BookId == id);
                    if (basketItem != null)
                    {
                        basketItem.IsDeleted = true;
                        await _context.SaveChangesAsync();
                    }
                }
            }
            else
            {
                var basketJson = _httpContext?.HttpContext?
                           .Request.Cookies["basket"];
                if (basketJson != null)
                {
                    List<BasketViewModel>? basketViewModels = JsonConvert
                             .DeserializeObject<List<BasketViewModel>>(basketJson);

                    BasketViewModel basketViewModel = basketViewModels.FirstOrDefault(x => x.BookId == id);
                    if (basketViewModel != null)
                    {
                        basketViewModels.Remove(basketViewModel);
                        basketJson = JsonConvert.SerializeObject(basketViewModels);
                        _httpContext?.HttpContext?.Response.Cookies.Append("basket", basketJson);
                    }
                }
            }
        }
    }
}
