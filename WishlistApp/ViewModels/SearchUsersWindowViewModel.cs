using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WishlistApp.Models;
using WishlistApp.Services;

namespace WishlistApp.ViewModels
{
    public class SearchUsersWindowViewModel : ViewModelBase
    {
        private readonly Action _navigateToMain;
        private readonly AuthService _authService;

        public ObservableCollection<User> Users { get; set; }
        
        public string SearchEmail { get; set; }
        public ICommand SearchCommand { get; }
        public ICommand OnWishlistSelectedCommand { get; }

        public SearchUsersWindowViewModel(Action navigateToMain)
        {
            _navigateToMain = navigateToMain;
            _authService = new AuthService();
            Users = new ObservableCollection<User>();
            SearchCommand = new AsyncRelayCommand(SearchUsersAsync);
            OnWishlistSelectedCommand = new RelayCommand<User>(OnUserSelected);
            NavigateToMain = new RelayCommand(() => _navigateToMain());
        }
        public ICommand NavigateToMain { get; }
            
        private async Task SearchUsersAsync()
        {
            try
            {
                Console.WriteLine("SearchUsersAsync called");
                if (!string.IsNullOrEmpty(SearchEmail))
                {
                    Console.WriteLine($"Searching for email: {SearchEmail}");
                    var user = await _authService.GetUserByEmailAsync(SearchEmail);
                    Users.Clear();

                    if (user != null)
                    {
                        Console.WriteLine($"User found: {user.Name}");
                        Users.Add(user);
                    }
                    else
                    {
                        Console.WriteLine("No user found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during search: {ex.Message}");
            }
        }
        public void OnUserSelected(User clickedUser)
        {
           UserService._userSearched = clickedUser;
            Console.WriteLine(clickedUser.Name);
            NavigationService.CurrentAction();

        }
    }
}