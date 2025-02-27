﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using WishlistApp.Models;
using WishlistApp.ViewModels;

namespace WishlistApp.Views;

public partial class MainContentView : UserControl
{
    public MainContentView()
    {
        InitializeComponent();
       

    }
    // Метод для обработки нажатий на кнопки внутри ItemsControl
    private void OnWishlistButtonClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is Wishlist clickedWishlist)
        {
            // Здесь вызывается метод ViewModel с информацией о выбранном Wishlist
            var viewModel = DataContext as MainContentViewModel;
            viewModel?.OnWishlistSelected(clickedWishlist);
        }
    }


   
}