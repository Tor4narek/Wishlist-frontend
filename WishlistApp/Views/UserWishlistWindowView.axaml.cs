using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using WishlistApp.Models;
using WishlistApp.ViewModels;

namespace WishlistApp.Views;

public partial class UserWishlistWindowView : UserControl
{
    public UserWishlistWindowView()
    {
        InitializeComponent();
    }
    private void OnWishlistButtonClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is Wishlist clickedWishlist)
        {
            // Здесь вызывается метод ViewModel с информацией о выбранном Wishlist
            var viewModel = DataContext as UserWishlistWindowViewModel;
            viewModel?.OnWishlistSelected(clickedWishlist);
        }
    }
}