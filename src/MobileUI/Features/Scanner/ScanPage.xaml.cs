﻿using BarcodeScanning;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mopups.Services;
using SSW.Rewards.Mobile.Messages;

namespace SSW.Rewards.Mobile.Pages;

public partial class ScanPage : IRecipient<EnableScannerMessage>
{
    private readonly ScanResultViewModel _viewModel;
    private readonly IFirebaseAnalyticsService _firebaseAnalyticsService;
    private const float ZoomFactorStep = 1.0f;

    public ScanPage(ScanResultViewModel viewModel, IFirebaseAnalyticsService firebaseAnalyticsService)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _firebaseAnalyticsService = firebaseAnalyticsService;
    }

    private void Handle_OnScanResult(object sender, OnDetectionFinishedEventArg e)
    {
        // the handler is called on a thread-pool thread
        App.Current.Dispatcher.Dispatch(() =>
        {
            if (!ScannerView.CameraEnabled || e.BarcodeResults.Length == 0)
            {
                return;
            }
            
            ToggleScanner(false);
            
            var result = e.BarcodeResults.FirstOrDefault()?.RawValue;

            var popup = new PopupPages.ScanResult(_viewModel, result);
            MopupService.Instance.PushAsync(popup);
        });
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        
        // Reset zoom when exiting camera
        if (ScannerView.CurrentZoomFactor > -1)
        {
            ScannerView.RequestZoomFactor = ScannerView.MinZoomFactor;
        }
        
        ToggleScanner(false);
        WeakReferenceMessenger.Default.Unregister<EnableScannerMessage>(this);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _firebaseAnalyticsService.Log("ScanPage");
        WeakReferenceMessenger.Default.Register(this);
        
        ToggleScanner(true);
    }


    public void Receive(EnableScannerMessage message)
    {
        ToggleScanner(true);
    }

    private void ToggleScanner(bool toggleOn)
    {
        ScannerView.CameraEnabled = toggleOn;
    }
    
    [RelayCommand]
    private async Task Dismiss()
    {
        if (Navigation.ModalStack.Count > 0)
        {
            await Navigation.PopModalAsync();
        }
    }
    
    [RelayCommand]
    private void ZoomIn()
    {
        // CurrentZoomFactor can default to -1, so we start at the MinZoomFactor in this case
        var currentZoom = Math.Max(ScannerView.CurrentZoomFactor, ScannerView.MinZoomFactor);
        var maxZoom = ScannerView.MaxZoomFactor;
        
        ScannerView.RequestZoomFactor = Math.Min(currentZoom + ZoomFactorStep, maxZoom);
    }
    
    [RelayCommand]
    private void ZoomOut()
    {
        var currentZoom = ScannerView.CurrentZoomFactor;
        var minZoom = ScannerView.MinZoomFactor;
        
        ScannerView.RequestZoomFactor = Math.Max(currentZoom - ZoomFactorStep, minZoom);
    }
}