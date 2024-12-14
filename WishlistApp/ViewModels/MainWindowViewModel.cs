using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace WishlistApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        // Начальная страница — Логин
        CurrentViewModel = new LoginWindowViewModel(SwitchToRegister, SwitchToMain);
    }

    [ObservableProperty]
    private ViewModelBase? _currentViewModel;

    private void SwitchToRegister()
    {
        CurrentViewModel = new RegisterWindowViewModel(SwitchToLogin);
    }

    private void SwitchToLogin()
    {
        CurrentViewModel = new LoginWindowViewModel(SwitchToRegister, SwitchToMain);
    }

    private void SwitchToMain()
    {
        CurrentViewModel = new MainContentViewModel();
    }
}