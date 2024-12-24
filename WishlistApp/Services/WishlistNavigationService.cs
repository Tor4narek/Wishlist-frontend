using System;
using WishlistApp.Models;

namespace WishlistApp.Services
{
    public static class WishlistNavigationService
    {
        // Статическое свойство для хранения текущего вишлиста
        public static Wishlist CurrentWishlist { get; set; }
        public static Action CurrentAction { get; set; }
        public static bool isUserWishlist { get; set; }
    }
}