using APP.Models;
using CORE.APP.Services.Session.MVC;

namespace APP.Services
{
    /// <summary>
    /// Service for managing users in a session-based cart.
    /// Uses session storage to persist cart data across requests.
    /// </summary>
    public class UserCartService : SessionServiceBase, IUserCartService
    {
        private const string CartSessionKey = "UserCart";

        /// <summary>
        /// Initializes a new instance of UserCartService.
        /// </summary>
        /// <param name="httpContextAccessor">Accessor for the current HTTP context.</param>
        public UserCartService(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor) 
            : base(httpContextAccessor)
        {
        }

        /// <summary>
        /// Gets all users in the cart.
        /// </summary>
        /// <returns>List of user cart items, or empty list if cart is empty.</returns>
        public List<UserCartItem> GetCart()
        {
            var cart = GetSession<List<UserCartItem>>(CartSessionKey);
            return cart ?? new List<UserCartItem>();
        }

        /// <summary>
        /// Adds a user to the cart if not already present.
        /// </summary>
        /// <param name="userId">The ID of the user to add.</param>
        /// <param name="userName">The user name.</param>
        /// <param name="fullName">The user's full name.</param>
        /// <param name="registrationDate">The registration date.</param>
        /// <param name="score">The score.</param>
        /// <param name="group">The group name.</param>
        /// <returns>True if user was added successfully, false if user already exists in cart.</returns>
        public bool AddToCart(int userId, string userName, string fullName, DateTime registrationDate, decimal score, string group)
        {
            var cart = GetCart();
            
            // Check if user already exists in cart
            if (!cart.Any(item => item.UserId == userId))
            {
                cart.Add(new UserCartItem
                {
                    UserId = userId,
                    UserName = userName,
                    FullName = fullName,
                    RegistrationDate = registrationDate,
                    RegistrationDateF = registrationDate.ToString("dd/MM/yyyy"),
                    Score = score,
                    ScoreF = score.ToString("N2"),
                    Group = group ?? ""
                });
                SetSession(CartSessionKey, cart);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes a user from the cart.
        /// </summary>
        /// <param name="userId">The ID of the user to remove.</param>
        public void RemoveFromCart(int userId)
        {
            var cart = GetCart();
            var itemToRemove = cart.FirstOrDefault(item => item.UserId == userId);
            
            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                SetSession(CartSessionKey, cart);
            }
        }

        /// <summary>
        /// Clears all users from the cart.
        /// </summary>
        public void ClearCart()
        {
            RemoveSession(CartSessionKey);
        }

        /// <summary>
        /// Gets the count of users in the cart.
        /// </summary>
        /// <returns>The number of users in the cart.</returns>
        public int GetCartCount()
        {
            return GetCart().Count;
        }
    }
}

