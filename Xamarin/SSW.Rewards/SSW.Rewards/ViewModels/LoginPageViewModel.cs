﻿using System;
using System.Windows.Input;
using SSW.Rewards.Services;
using Xamarin.Forms;
using SSW.Rewards.Models;
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
                //Crashes.TrackError(exception);
                Console.WriteLine("ERROR logging in");
                Console.WriteLine(exception.Message);
                await App.Current.MainPage.DisplayAlert("Login Failure", exception.Message, "OK");
            }

            Console.WriteLine("Login status:");
            Console.WriteLine(status);

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

        public async Task Refresh()
        {
            if (_userService.HasCachedAccount)
            {
                isRunning = true;
                ButtonText = "Logging you in...";
                RaisePropertyChanged("isRunning", "ButtonText", "LoginButtonEnabled");

                try
                {
                    await _userService.RefreshLoginAsync();

                    await _userService.UpdateMyDetailsAsync();

                    await OnAfterLogin();
                }
                catch (Exception e)
                {
                    // Everything else is fatal
                    //Crashes.TrackError(e);
                    await Application.Current.MainPage.DisplayAlert("Login Failure",
                        "There seems to have been a problem logging you in. Please try again. " + e.Message, "OK");
                }
                finally
                {
                    isRunning = false;
                    ButtonText = "Sign up / Log in";
                    RaisePropertyChanged(nameof(ButtonText), nameof(isRunning), nameof(LoginButtonEnabled));
                }
            }
        }

        private async Task OnAfterLogin()
        {
            var qr = _userService.MyQrCode;
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

        async Task OnForgotPassword()
        {
            await _userService.ResetPassword();
        }
    }
}