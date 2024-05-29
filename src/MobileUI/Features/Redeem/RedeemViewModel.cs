using System.Collections.ObjectModel;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using SSW.Rewards.ApiClient.Services;
using SSW.Rewards.Mobile.PopupPages;
using IRewardService = SSW.Rewards.Mobile.Services.IRewardService;
using IUserService = SSW.Rewards.Mobile.Services.IUserService;

namespace SSW.Rewards.Mobile.ViewModels;

public partial class RedeemViewModel : BaseViewModel
{
    private readonly IRewardService _rewardService;
    private readonly IUserService _userService;
    private readonly IAddressService _addressService;
    private bool _isLoaded;
    private IDispatcherTimer _timer;
    
    private const int AutoScrollInterval = 6;

    public ObservableCollection<Reward> Rewards { get; set; } = [];
    public ObservableCollection<Reward> CarouselRewards { get; set; } = [];

    [ObservableProperty]
    private int _credits;
    
    [ObservableProperty]
    private bool _isRefreshing;
    
    [ObservableProperty]
    private int _carouselPosition;

    public RedeemViewModel(IRewardService rewardService, IUserService userService, IAddressService addressService)
    {
        Title = "Rewards";
        _rewardService = rewardService;
        _userService = userService;
        _addressService = addressService;
        _userService.MyBalanceObservable().Subscribe(balance => Credits = balance);
        _userService.MyUserIdObservable().DistinctUntilChanged().Subscribe(OnUserChanged);
        
        _timer = Application.Current.Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromSeconds(AutoScrollInterval);
    }

    public void OnDisappearing()
    {
        _timer.Stop();
        _timer.Tick -= OnScrollTick;
    }

    private void OnUserChanged(int userId)
    {
        Rewards.Clear();
        CarouselRewards.Clear();
        _isLoaded = false;
    }

    public async Task Initialise()
    {
        if (_isLoaded)
        {
            return;
        }

        await LoadData();
        BeginAutoScroll();
    }

    private async Task LoadData()
    {
        if (!_isLoaded)
            IsBusy = true;

        var rewardList = await _rewardService.GetRewards();
        var pendingRedemptions = (await _userService.GetPendingRedemptionsAsync()).ToList();

        Rewards.Clear();
        CarouselRewards.Clear();
        _timer.Stop();
        
        foreach (var reward in rewardList.Where(reward => !reward.IsHidden))
        {
            var pendingRedemption = pendingRedemptions.FirstOrDefault(x => x.RewardId == reward.Id);
            reward.CanAfford = reward.Cost <= Credits;

            if (pendingRedemption != null)
            {
                reward.IsPendingRedemption = true;
                reward.PendingRedemptionCode = pendingRedemption.Code;
            }

            Rewards.Add(reward);

            if (reward.IsCarousel)
            {
                CarouselRewards.Add(reward);
            }
        }

        CarouselPosition = 0;

        IsBusy = false;
        _isLoaded = true;
        _timer.Start();
    }
    
    private void BeginAutoScroll()
    {
        _timer.Tick += OnScrollTick;
        _timer.Start();
    }
    
    private void OnScrollTick(object sender, object args)
    {
        MainThread.BeginInvokeOnMainThread(Scroll);
    }
    
    private void Scroll()
    {
        var count = CarouselRewards.Count;
        
        if (count > 0)
            CarouselPosition = (CarouselPosition + 1) % count;
    }
    
    [RelayCommand]
    private void CarouselScrolled()
    {
        // Reset timer when scrolling
        _timer.Stop();
        _timer.Start();
    }
    
    [RelayCommand]
    private async Task RefreshRewards()
    {
        await LoadData();
        IsRefreshing = false;
    }

    [RelayCommand]
    public async Task RedeemReward(int id)
    {
        var reward = Rewards.FirstOrDefault(r => r.Id == id);
        if (reward != null)
        {
            Application.Current.Resources.TryGetValue("Background", out var statusBarColor);
            var popup = new RedeemRewardPage(
                new RedeemRewardViewModel(_userService, _rewardService, _addressService),
                reward,
                statusBarColor as Color);
            popup.CallbackEvent += async (_, _) =>
            {
                await LoadData();
                await _userService.UpdateMyDetailsAsync();
            };
            await MopupService.Instance.PushAsync(popup);
        }
    }
}