﻿using System;
using System.Windows.Input;
using SSW.Rewards.Services;
using Xamarin.Forms;
using Xamarin.Essentials;
using SSW.Rewards.Models;
using Microsoft.AppCenter.Crashes;
using System.Collections.Generic;
using Microsoft.Identity.Client;
using System.Linq;
using System.Threading.Tasks;

namespace SSW.Rewards.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        public ICommand LoginTappedCommand { get; set; }
        private IUserService _userService { get; set; }
        public bool isRunning { get; set; }
        public bool LoginButtonEnabled { get { return !isRunning; } }

        bool _isStaff = false;

        public string ButtonText { get; set; }

        public LoginPageViewModel(IUserService userService)
        {
            _userService = userService;
            LoginTappedCommand = new Command(SignIn);
            ButtonText = "Sign up / Log in";
            OnPropertyChanged("ButtonText");
            Refresh();
        }

        private async void SignIn()
        {
            isRunning = true;
            RaisePropertyChanged(new string[] { "isRunning", "LoginButtonEnabled" });

            ApiStatus status;
            try
            {
                status = await _userService.SignInAsync();
            }
            catch (Exception exception)
            {
                status = ApiStatus.LoginFailure;
                Crashes.TrackError(exception);
            }

            switch (status)
            {
                case ApiStatus.Success:
                    await OnAfterLogin();
                    break;
                case ApiStatus.Unavailable:
                    await App.Current.MainPage.DisplayAlert("Service Unavailable", "Looks like the SSW.Rewards service is not currently available. Please try again later.", "OK");
                    break;
                case ApiStatus.LoginFailure:
                    await App.Current.MainPage.DisplayAlert("Login Failure", "There seems to have been a problem logging you in. Please try again.", "OK");
                    break;
                default:
                    await App.Current.MainPage.DisplayAlert("Unexpected Error", "Something went wrong there, please try again later.", "OK");
                    break;
            }

            isRunning = false;
            RaisePropertyChanged(new string[] { "isRunning", "LoginButtonEnabled" });
        }

        private async void Refresh()
        {
            bool loggedIn = await _userService.IsLoggedInAsync();

            if (loggedIn)
            {
                isRunning = true;
                ButtonText = "Logging you in...";
                RaisePropertyChanged("isRunning", "ButtonText", "LoginButtonEnabled");

                AuthenticationResult result;

                try
                {
                    IEnumerable<IAccount> accounts = await App.AuthenticationClient.GetAccountsAsync();

                    result = await App.AuthenticationClient
                        .AcquireTokenSilent(App.Constants.Scopes, accounts.FirstOrDefault())
                        .ExecuteAsync();

                    string token = result.AccessToken;

                    await SecureStorage.SetAsync("auth_token", token);

                    await _userService.UpdateMyDetailsAsync();

                    await OnAfterLogin();
                }
                catch (MsalUiRequiredException)
                {
                    // Interactive signin required again
                    // Refresh token probably expired
                    await _userService.SignInAsync();
                }
                catch (MsalException ex)
                {
                    // Forgot password
                    if (ex.Message != null && ex.Message.Contains("AADB2C90118"))
                    {
                        try
                        {
                            result = await OnForgotPassword();
                            if (!string.IsNullOrWhiteSpace(result.AccessToken))
                            {
                                string token = result.AccessToken;

                                await SecureStorage.SetAsync("auth_token", token);


                                await OnAfterLogin();
                            }
                        }
                        catch (Exception e)
                        {
                            // Exception while resetting password
                            Crashes.TrackError(e);
                            await Application.Current.MainPage.DisplayAlert("Login Failure",
                                "There seems to have been a problem logging you in. Please try again. " + e.Message,
                                "OK");
                        }
                    }
                    else
                    {
                        // Fatal MSAL exception
                        Crashes.TrackError(ex);
                        await Application.Current.MainPage.DisplayAlert("Login Failure",
                            "There seems to have been a problem logging you in. Please try again. " + ex.Message, "OK");
                    }
                }
                catch (Exception e)
                {
                    // Everything else is fatal
                    Crashes.TrackError(e);
                    await Application.Current.MainPage.DisplayAlert("Login Failure", "There seems to have been a problem logging you in. Please try again. " + e.Message, "OK");
                }
            }
        }

        private async Task OnAfterLogin()
        {
            var qr = await _userService.GetMyQrCode();
            if (!string.IsNullOrWhiteSpace(qr))
            {
                _isStaff = true;
            }
            else
            {
                _isStaff = false;
            }

            Application.Current.MainPage = Resolver.ResolveShell(_isStaff);
            await Shell.Current.GoToAsync("//main");
        }

        async Task<AuthenticationResult> OnForgotPassword()
        {
            try
            {
                return await App.AuthenticationClient
                    .AcquireTokenInteractive(App.Constants.Scopes)
                    .WithPrompt(Prompt.SelectAccount)
                    .WithParentActivityOrWindow(App.UIParent)
                    .WithB2CAuthority(App.Constants.AuthorityReset)
                    .ExecuteAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}