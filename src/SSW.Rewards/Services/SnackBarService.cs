﻿using CommunityToolkit.Maui.Views;
using SSW.Rewards.Mobile.Controls;

namespace SSW.Rewards.Mobile.Services;

public class SnackBarService : ISnackbarService
{
    public async Task ShowSnackbar(SnackbarOptions options)
    {
        var snack = new Snackbar(options);

        await App.Current.MainPage.ShowPopupAsync(snack);

        await Task.Delay(5000);

        snack.Close();
    }
}
