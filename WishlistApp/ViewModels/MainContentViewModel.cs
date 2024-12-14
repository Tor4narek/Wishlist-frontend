using CommunityToolkit.Mvvm.ComponentModel;
using WishlistApp.Services;

namespace WishlistApp.ViewModels;

public partial class MainContentViewModel : ViewModelBase
{
    
    private AuthService _authService;
    public MainContentViewModel()
    {
        Greeting = "Welcome to WishlistApp!";
    }

    [ObservableProperty]
    private string _greeting;
}