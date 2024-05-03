﻿using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using SSW.Rewards.ApiClient.Services;
using SSW.Rewards.Enums;
using SSW.Rewards.Shared.DTOs.AddressTypes;
using SSW.Rewards.Shared.DTOs.Rewards;
using IRewardService = SSW.Rewards.Mobile.Services.IRewardService;
using IUserService = SSW.Rewards.Mobile.Services.IUserService;

namespace SSW.Rewards.Mobile.ViewModels;

public partial class RedeemRewardViewModel(IUserService userService, IRewardService rewardService, IAddressService addressService) : BaseViewModel
{
    private Reward _reward;

    [ObservableProperty]
    private string _image;

    [ObservableProperty]
    private string _heading;

    [ObservableProperty]
    private string _description;

    [ObservableProperty]
    private int _cost;

    [ObservableProperty]
    private int _userBalance;

    [ObservableProperty]
    private bool _isBalanceVisible = true;

    [ObservableProperty]
    private bool _isQrCodeVisible;

    [ObservableProperty]
    private bool _isAddressVisible;

    [ObservableProperty]
    private bool _isSearching;

    [ObservableProperty]
    private Address? _selectedAddress;

    [ObservableProperty]
    private bool _confirmEnabled;

    [ObservableProperty]
    private bool _isAddressEditExpanded;

    [ObservableProperty]
    private bool _sendingClaim;

    [ObservableProperty]
    private bool _claimError;

    [ObservableProperty]
    private bool _claimSuccess;

    [ObservableProperty]
    private string _qrCode;

    public ObservableCollection<Address> SearchResults { get; set; } = [];
    public bool ShouldCallCallback { get; set; }

    public void Initialise(Reward reward)
    {
        _reward = reward;
        Image = reward.ImageUri;
        Heading = $"You are about to get:{Environment.NewLine}{reward.Name}";
        Description = reward.Description;
        Cost = reward.Cost;

        if (reward.IsPendingRedemption)
        {
            ShowQrCode(reward);
        }

        userService.MyBalanceObservable().Subscribe(myBalance => UserBalance = myBalance);
    }

    private void ShowQrCode(Reward reward)
    {
        IsBalanceVisible = false;
        ConfirmEnabled = false;
        Heading = $"Ready to claim:{Environment.NewLine}{reward.Name}";
        QrCode = reward.PendingRedemptionCode;
        IsQrCodeVisible = true;
        ShouldCallCallback = true;
    }

    [RelayCommand]
    private async Task SearchAddress(string addressQuery)
    {
        IsSearching = true;

        SearchResults.Clear();

        if (string.IsNullOrEmpty(addressQuery))
        {
            IsSearching = false;
            return;
        }

        var results = await addressService.Search(addressQuery);

        results = results.ToList();

        if (results.Any())
        {
            foreach (var result in results)
            {
                SearchResults.Add(result);
            }
        }

        IsSearching = false;
    }

    [RelayCommand]
    private static async Task ClosePopup()
    {
        await MopupService.Instance.PopAsync();
    }

    [RelayCommand]
    private void NextClicked()
    {
        IsBalanceVisible = false;
        IsAddressVisible = true;
    }

    [RelayCommand]
    private void AddressSelected()
    {
        ConfirmEnabled = SelectedAddress != null;

        if (!ConfirmEnabled)
        {
            return;
        }

        IsAddressVisible = false;
    }

    [RelayCommand]
    private async Task RedeemInPersonClicked()
    {
        IsBalanceVisible = false;
        ConfirmEnabled = false;
        SendingClaim = true;
        Heading = "Claiming reward...";

        var claimResult = await rewardService.CreatePendingRedemption(new CreatePendingRedemptionDto
        {
            Id = _reward.Id
        });

        SendingClaim = false;

        if (claimResult.status == RewardStatus.Pending)
        {
            Heading = "Ready to claim!";
            QrCode = claimResult.Code;
            IsQrCodeVisible = true;
            ShouldCallCallback = true;
        }
        else
        {
            Heading = "Error";
            Description = "Something went wrong - please try again later";
            ClaimError = true;
        }
    }

    [RelayCommand]
    private async Task CancelPendingRedemptionClicked()
    {
        IsBusy = true;

        await rewardService.CancelPendingRedemption(new CancelPendingRedemptionDto
        {
            Id = _reward.Id
        });

        IsBusy = false;
        await MopupService.Instance.PopAsync();
    }

    [RelayCommand]
    private async Task ConfirmClicked()
    {
        if (SelectedAddress is null)
        {
            return;
        }

        ConfirmEnabled = false;
        SendingClaim = true;
        Heading = "Claiming reward...";

        var claimResult = await rewardService.ClaimReward(new ClaimRewardDto()
        {
            Id = _reward.Id,
            InPerson = false,
            Address = SelectedAddress
        });

        SendingClaim = false;

        if (claimResult.status == RewardStatus.Claimed)
        {
            Heading = "Success!";
            Description = "Your reward is on the way!";
            ClaimSuccess = true;
        }
        else
        {
            Heading = "Error";
            Description = "Something went wrong - please try again later";
            ClaimError = true;
        }
    }

    [RelayCommand]
    private void EditAddress()
    {
        IsAddressEditExpanded = !IsAddressEditExpanded;
    }

    [RelayCommand]
    private void SearchAgain()
    {
        ConfirmEnabled = false;
        IsAddressVisible = true;
    }
}