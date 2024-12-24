using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using WishlistApp.Models;
using WishlistApp.Services;

namespace WishlistApp.ViewModels;

public class UserWishlistWindowViewModel : ViewModelBase
{
     private readonly WishlistService _wishlistService;
     
        private readonly Action _navigateToSearchUser;
      
        public UserWishlistWindowViewModel(Action navigateToSearchUser)
        {
            _navigateToSearchUser = navigateToSearchUser;
            _wishlistService = new WishlistService();
            Wishlists = new ObservableCollection<Wishlist>();
            LoadWishlistsCommand = new AsyncRelayCommand(LoadWishlistsAsync);
            NavigateToSearchUserCommand = new RelayCommand(NavigateToSearchUser);
            OnWishlistSelectedComand = new RelayCommand<Wishlist>(OnWishlistSelected);
            _ = LoadWishlistsAsync();
        }
        
      

        public ObservableCollection<Wishlist> Wishlists { get; set; }

        public string Greeting => $"Wishlists, {UserService._userSearched.Name}";

       

        public ICommand LoadWishlistsCommand { get; }
        public ICommand NavigateToSearchUserCommand { get; }
        public ICommand OnWishlistSelectedComand { get; }
       

        private void NavigateToSearchUser()
        {
            _navigateToSearchUser?.Invoke();
        }
       
        private async Task LoadWishlistsAsync(CancellationToken token = default)
        {
            try
            {
                var wishlists = await _wishlistService.GetUserWishlistsAsync(UserService._userSearched.Id, token);
                Console.WriteLine($"Loaded {wishlists.Count} wishlists"); // Для отладки
                Wishlists.Clear();
                foreach (var wishlist in wishlists)
                {
                    Wishlists.Add(wishlist);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading wishlists: {ex.Message}");
            }
        }
    
        public void OnWishlistSelected(Wishlist clickedWishlist)
        {
            WishlistNavigationService.CurrentWishlist = clickedWishlist;
            WishlistNavigationService.isUserWishlist = false;
           Console.WriteLine(clickedWishlist.Name);
           WishlistNavigationService.CurrentAction();

        }
}