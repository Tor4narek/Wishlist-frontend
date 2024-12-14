using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using WishlistApp.Services;

namespace WishlistApp.ViewModels;

public partial class RegisterWindowViewModel : ViewModelBase
{
    private readonly AuthService _authService;
    private readonly Action _navigateToLogin;

    public RegisterWindowViewModel(Action navigateToLogin)
    {
        _authService = new AuthService();
        _navigateToLogin = navigateToLogin;

        RegisterCommand = new AsyncRelayCommand(RegisterAsync);
        NavigateToLoginCommand = new RelayCommand(_navigateToLogin);
    }

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _email;

    [ObservableProperty]
    private string _password;

    [ObservableProperty]
    private string _statusMessage;

    public IAsyncRelayCommand RegisterCommand { get; }
    public RelayCommand NavigateToLoginCommand { get; }

    private async Task RegisterAsync()
    {
        StatusMessage = "Processing registration...";
        var result = await _authService.RegisterAsync(Name, Email, Password);
        if (result.Contains("successful"))
        {
            _navigateToLogin();
        }
        else
        {
            StatusMessage = result;
        }
    }
}