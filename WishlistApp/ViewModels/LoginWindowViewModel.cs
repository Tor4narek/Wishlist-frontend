using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using WishlistApp.Services;

namespace WishlistApp.ViewModels;

public partial class LoginWindowViewModel : ViewModelBase
{
    
    private readonly AuthService _authService;
    private readonly Action _navigateToRegister;
    private readonly Action _navigateToMain;

    public LoginWindowViewModel(Action navigateToRegister, Action navigateToMain)
    {
        _authService = new AuthService();
        _navigateToRegister = navigateToRegister;
        _navigateToMain = navigateToMain;

        LoginCommand = new AsyncRelayCommand(LoginAsync);
        NavigateToRegisterCommand = new RelayCommand(_navigateToRegister);
    }

    [ObservableProperty]
    private string _email;

    [ObservableProperty]
    private string _password;

    [ObservableProperty]
    private string _statusMessage;

    public IAsyncRelayCommand LoginCommand { get; }
    public RelayCommand NavigateToRegisterCommand { get; }

    private async Task LoginAsync()
    {
        StatusMessage = "Processing login...";
        var (message, token, userId) = await _authService.LoginAsync(Email, Password);
        if (token != null && userId != null)
        {
            
            _navigateToMain();
        }
        else
        {
            StatusMessage = message;
        }
    }
}