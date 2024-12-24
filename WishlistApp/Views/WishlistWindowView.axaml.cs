using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using WishlistApp.Services;
using WishlistApp.ViewModels;

namespace WishlistApp.Views;

public partial class WishlistWindowView : UserControl
{
    public WishlistWindowView()
    {
        InitializeComponent();
    }
    private void OnPresentButtonClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is Present clickedPresent)
        {
            var viewModel = DataContext as WishlistWindowViewModel;
            if (viewModel != null && !viewModel.IsUserWishlist)
            {
                viewModel.OnPresentSelected(clickedPresent);
            }
        }
    }
}