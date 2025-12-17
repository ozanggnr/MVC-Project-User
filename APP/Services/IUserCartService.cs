using APP.Models;

namespace APP.Services
{
    /// <summary>
    /// Interface for user cart service operations.
    /// Provides methods to manage users in a session-based cart.
    /// </summary>
    public interface IUserCartService
    {
        /// <summary>
        /// Gets all users in the cart.
        /// </summary>
        /// <returns>List of user cart items.</returns>
        List<UserCartItem> GetCart();

        /// <summary>
        /// Adds a user to the cart.
        /// </summary>
        /// <param name="userId">The ID of the user to add.</param>
        /// <param name="userName">The user name.</param>
        /// <param name="fullName">The user's full name.</param>
        /// <param name="registrationDate">The registration date.</param>
        /// <param name="score">The score.</param>
        /// <param name="group">The group name.</param>
        /// <returns>True if user was added successfully, false if user already exists in cart.</returns>
        bool AddToCart(int userId, string userName, string fullName, DateTime registrationDate, decimal score, string group);

        /// <summary>
        /// Removes a user from the cart.
        /// </summary>
        /// <param name="userId">The ID of the user to remove.</param>
        void RemoveFromCart(int userId);

        /// <summary>
        /// Clears all users from the cart.
        /// </summary>
        void ClearCart();

        /// <summary>
        /// Gets the count of users in the cart.
        /// </summary>
        /// <returns>The number of users in the cart.</returns>
        int GetCartCount();
    }
}

