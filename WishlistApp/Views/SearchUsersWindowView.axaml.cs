using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using WishlistApp.Models;
using WishlistApp.ViewModels;

namespace WishlistApp.Views;

public partial class SearchUsersWindowView : UserControl
{
    public SearchUsersWindowView()
    {
        InitializeComponent();
    }
    private void OnWishlistButtonClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is User clickedUser)
        {
            // Здесь вызывается метод ViewModel с информацией о выбранном Wishlist
            var viewModel = DataContext as SearchUsersWindowViewModel;
            viewModel?.OnUserSelected(clickedUser);
        }
    }
}