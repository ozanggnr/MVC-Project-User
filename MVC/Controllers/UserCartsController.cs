#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APP.Models;
using APP.Services;
using CORE.APP.Services;
using System.Linq;

namespace MVC.Controllers
{
    public class UserCartsController : Controller
    {
        private readonly IUserCartService _userCartService;
        private readonly IService<UserRequest, UserResponse> _userService;

        public UserCartsController(
            IUserCartService userCartService,
            IService<UserRequest, UserResponse> userService)
        {
            _userCartService = userCartService;
            _userService = userService;
        }

        private void SetTempData(string message, string key = "Message")
        {
            TempData[key] = message;
        }

        // GET: UserCarts
        public IActionResult Index()
        {
            var cart = _userCartService.GetCart();
            return View(cart);
        }

        // POST: UserCarts/AddToCart
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult AddToCart(int userId)
        {
            var user = _userService.Item(userId);
            if (user is not null)
            {
                // Check if user already exists in cart
                var cart = _userCartService.GetCart();
                if (cart.Any(item => item.UserId == userId))
                {
                    SetTempData($"User '{user.FullName}' is already in the cart!");
                    return RedirectToAction("Index", "Users");
                }

                // Add user to cart
                bool added = _userCartService.AddToCart(userId, user.UserName, user.FullName, user.RegistrationDate, user.Score, user.Group);
                if (added)
                {
                    SetTempData($"User '{user.FullName}' added to cart successfully.");
                }
                else
                {
                    SetTempData($"User '{user.FullName}' is already in the cart!");
                }
                return RedirectToAction("Index", "Users");
            }
            else
            {
                SetTempData("User not found!");
                return RedirectToAction("Index", "Users");
            }
        }

        // POST: UserCarts/RemoveFromCart
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult RemoveFromCart(int userId)
        {
            var cart = _userCartService.GetCart();
            var user = cart.FirstOrDefault(item => item.UserId == userId);
            if (user is not null)
            {
                _userCartService.RemoveFromCart(userId);
                SetTempData($"User '{user.FullName}' removed from cart successfully.");
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: UserCarts/ClearCart
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult ClearCart()
        {
            _userCartService.ClearCart();
            SetTempData("Cart cleared successfully.");
            return RedirectToAction(nameof(Index));
        }
    }
}

