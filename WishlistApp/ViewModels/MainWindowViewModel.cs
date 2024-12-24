using CommunityToolkit.Mvvm.ComponentModel;
using System;
using WishlistApp.Models;
using WishlistApp.Services;

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
        var mainContentViewModel = new MainContentViewModel();
        CurrentViewModel = mainContentViewModel;
        WishlistNavigationService.CurrentAction = SwitchToWishList;
        NavigationService.CurrentAction = SwitchToSearchUsers;

    }
    private void SwitchToWishList()
    {
        CurrentViewModel = new WishlistWindowViewModel(SwitchToMain);
    }

    private void SwitchToSearchUsers()
    {
        CurrentViewModel = new SearchUsersWindowViewModel(SwitchToMain);
        NavigationService.CurrentAction = SwitchToUserWishLists;
    }

    private void SwitchToUserWishLists()
    {
        CurrentViewModel = new UserWishlistWindowViewModel(SwitchToSearchUsers);
    }
}