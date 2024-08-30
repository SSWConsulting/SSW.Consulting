﻿namespace SSW.Rewards.Mobile;

public partial class App : Application
{
    public static object UIParent { get; set; }

    public App(LoginPage page)
    {
        InitializeComponent();
        Current.UserAppTheme = AppTheme.Dark;

        MainPage = page;
    }

    protected override async void OnStart()
    {
        //await UpdateAccessTokenAsync();
        await CheckApiCompatibilityAsync();

        // HACK - Resource dictionary isn't available here :(
        // See discussion: https://github.com/dotnet/maui/discussions/5263
        //MainPage = new LoginPage(loginPageViewModel);

        //await App.Current.MainPage.Navigation.PushModalAsync<LoginPage>();
    }

    protected override void OnSleep()
    {
        // Handle when your app sleeps
    }

    protected override void OnResume()
    {
        // Handle when your app resumes
    }

    private async Task CheckApiCompatibilityAsync()
    {
        try
        {
            ApiInfo info = new ApiInfo(Constants.ApiBaseUrl);

            bool compatible = await info.IsApiCompatibleAsync();

            if (!compatible)
            {
                await Application.Current.MainPage.DisplayAlert("Update Required", "Looks like you're using an older version of the app. You can continue, but some features may not function as expected.", "OK");
            }
        }
        catch (Exception ex)
        {
            // TODO: log these instead to AppCenter
            Console.WriteLine("[App] ERROR checking API compat");
            Console.WriteLine($"[App] {ex.Message}");
            Console.WriteLine($"[App {ex.StackTrace}");
        }
    }
}
