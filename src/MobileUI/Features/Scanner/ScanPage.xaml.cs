﻿namespace SSW.Rewards.Mobile.Pages;

public partial class ScanPage
{
    private readonly ScanViewModel _viewModel;
    private readonly IFirebaseAnalyticsService _firebaseAnalyticsService;

    public ScanPage(ScanViewModel viewModel, IFirebaseAnalyticsService firebaseAnalyticsService,
        ScanPageSegments segment = ScanPageSegments.Scan)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _viewModel.Navigation = Navigation;
        _firebaseAnalyticsService = firebaseAnalyticsService;
        BindingContext = _viewModel;

        _viewModel.SetSegment(segment);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.OnDisappearing();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnAppearing();
        _firebaseAnalyticsService.Log("ScanPage");
    }
}