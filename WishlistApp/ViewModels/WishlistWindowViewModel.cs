using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using WishlistApp.Models;
using WishlistApp.Services;

namespace WishlistApp.ViewModels;

public class WishlistWindowViewModel : ViewModelBase
{
    private readonly PresentQueryService _presentQueryService;
    private readonly PresentCommandsService _presentCommandsService;
    private bool _isCreatePresentModalVisible;
    private readonly Action _navigateToMain;
    private Wishlist _wishlist;

    public WishlistWindowViewModel(Action navigateToMain)
    {
        _presentQueryService = new PresentQueryService();
        _presentCommandsService = new PresentCommandsService();
        _navigateToMain = navigateToMain;
        Presents = new ObservableCollection<Present>();
        NavigateToMainCommand = new RelayCommand(NavigateToMain);
        CreatePresentCommand = new AsyncRelayCommand(CreatePresentAsync);
        OpenCreatePresentModalCommand = new RelayCommand(OpenCreatePresentModal);
        CloseCreatePresentModalCommand = new RelayCommand(CloseCreatePresentModal);
        LoadPresentsCommand = new AsyncRelayCommand(LoadPresentsAsync);
        OnPresentSelectedCommand = new RelayCommand<Present>(OnPresentSelected);
        // Загрузка подарков для данного вишлиста
        _ = LoadPresentsAsync();
    }

    public string WishlistName => $"{WishlistNavigationService.CurrentWishlist.Name}";
    public bool IsUserWishlist => WishlistNavigationService.isUserWishlist;

    public ObservableCollection<Present> Presents { get; set; }

    public ICommand NavigateToMainCommand { get; }
    public ICommand CreatePresentCommand { get; }
    public ICommand OpenCreatePresentModalCommand { get; }
    public ICommand CloseCreatePresentModalCommand { get; }
    public ICommand LoadPresentsCommand { get; }
    public ICommand OnPresentSelectedCommand { get; }

    private void NavigateToMain()
    {
        _navigateToMain();
    }

    private async Task LoadPresentsAsync(CancellationToken token = default)
    {
        try
        {
            _wishlist = WishlistNavigationService.CurrentWishlist;
            var presents = await _presentQueryService.GetWishlistPresentsAsync(_wishlist.Id, token);
            Console.WriteLine($"Loaded {presents.Count} presents for wishlist {_wishlist.Id}");
            Presents.Clear();
            foreach (var present in presents)
            {
                Presents.Add(present);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading presents: {ex.Message}");
        }
    }

    private string _newPresentName;
    public string NewPresentName
    {
        get => _newPresentName;
        set => SetProperty(ref _newPresentName, value);
    }

    private string _newPresentDescription;
    public string NewPresentDescription
    {
        get => _newPresentDescription;
        set => SetProperty(ref _newPresentDescription, value);
    }

    public bool IsCreatePresentModalVisible
    {
        get => _isCreatePresentModalVisible;
        set => SetProperty(ref _isCreatePresentModalVisible, value);
    }

    private async Task CreatePresentAsync(CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(NewPresentName)) return;

        var newPresent = new PresentCreationModel
        {
            Name = NewPresentName,
            Description = NewPresentDescription,
            ReserverId = "", // Подарок создается без резервирования
            WishlistId = _wishlist.Id // Укажите текущий вишлист, в который добавляется подарок
        };

        try
        {
            await _presentCommandsService.AddPresentAsync(
                newPresent.Name,
                newPresent.Description,
                newPresent.ReserverId,
                newPresent.WishlistId,
                token);

            // Обновление списка подарков
            _ = LoadPresentsAsync(token);
            CloseCreatePresentModal();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating present: {ex.Message}");
        }
    }

    private void OpenCreatePresentModal()
    {
        IsCreatePresentModalVisible = true;
    }

    private void CloseCreatePresentModal()
    {
        IsCreatePresentModalVisible = false;
        NewPresentName = string.Empty;
        NewPresentDescription = string.Empty;
    }

    public void OnPresentSelected(Present present)
    
    {
        if (IsUserWishlist == false)
        {
            if (present == null) return;

            try
            {
                // Вызываем метод резервирования подарка
                _ = ReservePresentAsync(present);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reserving present: {ex.Message}");
            }
        }
    }

    private async Task ReservePresentAsync(Present present, CancellationToken token = default)
    {
        try
        {
            await _presentCommandsService.ReservePresentAsync(present.Id, UserService._user.Id, token);

            // Обновляем список подарков
            _ = LoadPresentsAsync(token);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reserving present: {ex.Message}");
        }
    }

    public class PresentCreationModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ReserverId { get; set; }
        public string WishlistId { get; set; }
    }
}
