using System;
using System.ComponentModel;

namespace APP.Models
{
    /// <summary>
    /// Represents a user item in the cart.
    /// </summary>
    public class UserCartItem
    {
        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the user's full name.
        /// </summary>
        [DisplayName("Full Name")]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the registration date.
        /// </summary>
        [DisplayName("Registration Date")]
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// Gets or sets the formatted registration date.
        /// </summary>
        [DisplayName("Registration Date")]
        public string RegistrationDateF { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        [DisplayName("Score")]
        public decimal Score { get; set; }

        /// <summary>
        /// Gets or sets the formatted score.
        /// </summary>
        [DisplayName("Score")]
        public string ScoreF { get; set; }

        /// <summary>
        /// Gets or sets the group name.
        /// </summary>
        [DisplayName("Group")]
        public string Group { get; set; }
    }
}

