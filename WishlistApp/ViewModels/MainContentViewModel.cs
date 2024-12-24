using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using WishlistApp.Services;
using WishlistApp.Models;

namespace WishlistApp.ViewModels
{
    public class MainContentViewModel : ViewModelBase
    {
        private readonly WishlistService _wishlistService;
       

        private bool _isCreateWishlistModalVisible;
        private string _newWishlistName;
        private string _newWishlistDescription;
      
        public MainContentViewModel()
        {
            _wishlistService = new WishlistService();
            Wishlists = new ObservableCollection<Wishlist>();
            LoadWishlistsCommand = new AsyncRelayCommand(LoadWishlistsAsync);
            CreateWishlistCommand = new AsyncRelayCommand(CreateWishlistAsync);
            OpenCreateWishlistModalCommand = new RelayCommand(OpenCreateWishlistModal);
            CloseCreateWishlistModalCommand = new RelayCommand(CloseCreateWishlistModal);
            OpenSearchUserCommand = new RelayCommand(OpenSearchUser);
            OnWishlistSelectedCommand = new RelayCommand<Wishlist>(OnWishlistSelected);
            _ = LoadWishlistsAsync();
        }

      

        public ObservableCollection<Wishlist> Wishlists { get; set; }

        public string Greeting => $"Hello, {UserService._user.Name}!";
        public ICommand OnWishlistSelectedCommand { get; }
        public bool IsCreateWishlistModalVisible
        {
            get => _isCreateWishlistModalVisible;
            set => SetProperty(ref _isCreateWishlistModalVisible, value);
        }

        public string NewWishlistName
        {
            get => _newWishlistName;
            set => SetProperty(ref _newWishlistName, value);
        }

        public string NewWishlistDescription
        {
            get => _newWishlistDescription;
            set => SetProperty(ref _newWishlistDescription, value);
        }

        public ICommand LoadWishlistsCommand { get; }
        public ICommand CreateWishlistCommand { get; }
        public ICommand OpenCreateWishlistModalCommand { get; }
        public ICommand CloseCreateWishlistModalCommand { get; }
        public ICommand OpenSearchUserCommand { get; }

        private void OpenSearchUser()
        {
            NavigationService.CurrentAction();
        }
        private async Task LoadWishlistsAsync(CancellationToken token = default)
        {
            try
            {
                var wishlists = await _wishlistService.GetUserWishlistsAsync(UserService._user.Id, token);
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

        private async Task CreateWishlistAsync(CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(NewWishlistName)) return;

            var newWishlist = new WishlistCreationModel
            {
                Name = NewWishlistName,
                Description = NewWishlistDescription,
                OwnerId = UserService._user.Id,
                PresentsNumber = "0"
            };

            await _wishlistService.AddWishlistAsync(newWishlist, token);
            
            _ = LoadWishlistsAsync(token);
            CloseCreateWishlistModal();
        }

        private void OpenCreateWishlistModal()
        {
            IsCreateWishlistModalVisible = true;
        }

        private void CloseCreateWishlistModal()
        {
            IsCreateWishlistModalVisible = false;
            NewWishlistName = string.Empty;
            NewWishlistDescription = string.Empty;
        }

        public void OnWishlistSelected(Wishlist clickedWishlist)
        {
            WishlistNavigationService.CurrentWishlist = clickedWishlist;
            WishlistNavigationService.isUserWishlist = true;
           Console.WriteLine(clickedWishlist.Name);
           WishlistNavigationService.CurrentAction();

        }


        public void Scum()
        {
            throw new StackOverflowException();
        }
       
    }
}