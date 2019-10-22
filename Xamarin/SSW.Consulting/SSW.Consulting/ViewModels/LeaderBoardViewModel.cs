﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using SSW.Consulting.Models;
using SSW.Consulting.Services;
using SSW.Consulting.Views;
using System.Linq;
using Xamarin.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SSW.Consulting.ViewModels
{
    public class LeaderBoardViewModel : BaseViewModel
    {


        public bool IsRunning { get; set; }
        public bool IsRefreshing { get; set; }

   


        private ILeaderService _leaderService;
        private IUserService _userService;
        public ICommand LeaderTapped
        {
            get
            {
                return new Command<LeaderSummaryViewModel>((x) => HandleLeaderTapped(x));
            }
        }
        public ICommand OnRefreshCommand { get; set; }

        public ObservableCollection<LeaderSummaryViewModel> Leaders { get; set; }

        public Action<LeaderSummaryViewModel> ScrollToMe { get; set; }

        private List<LeaderSummaryViewModel> searchResults = new List<LeaderSummaryViewModel>();


        public LeaderBoardViewModel(ILeaderService leaderService, IUserService userService)
        {
            Title = "SSW Leaderboard";
            OnRefreshCommand = new Command(Refresh);
            _leaderService = leaderService;
            _userService = userService;
            Leaders = new ObservableCollection<LeaderSummaryViewModel>();
            MessagingCenter.Subscribe<object>(this, "NewAchievement", (obj) => { Refresh(); });
            Initialise();
        }

        public ICommand SearchTextChanged => new Command<string>((string query) =>
        {
            if (query != null || query != String.Empty)
            {
                var filtered = Leaders.Where(l => l.Name.ToLower().Contains(query.ToLower())).ToList();
                SearchResults = filtered;
                return;
            }
            SearchResults = Leaders.ToList();

        });

        public List<LeaderSummaryViewModel> SearchResults
        {
            get
            {
                return searchResults;
            }
            set
            {
                searchResults = value;
                RaisePropertyChanged("SearchResults");

            }
        }





        private async void Initialise()
        {
            IsRunning = true;
            RaisePropertyChanged("IsRunning");
            var summaries = await _leaderService.GetLeadersAsync(false);
            int myId = await _userService.GetMyUserIdAsync();

            foreach(var summary in summaries)
            {
                LeaderSummaryViewModel vm = new LeaderSummaryViewModel(summary);
                vm.IsMe = (summary.id == myId);

                Leaders.Add(vm);
            }

            IsRunning = false;
            RaisePropertyChanged("IsRunning");

            SearchResults = Leaders.ToList();
            RaisePropertyChanged("SearchResults");


            var mysummary = Leaders.FirstOrDefault(l => l.IsMe == true);
            ScrollToMe?.Invoke(mysummary);
        }

        public async void Refresh()
        {
            //IsRunning = true;
            //RaisePropertyChanged("IsRunning");
            var summaries = await _leaderService.GetLeadersAsync(false);
            int myId = await _userService.GetMyUserIdAsync();

            Leaders.Clear();

            foreach (var summary in summaries)
            {
                LeaderSummaryViewModel vm = new LeaderSummaryViewModel(summary);
                vm.IsMe = (summary.id == myId);

                Leaders.Add(vm);
            }

            //IsRunning = false;
            IsRefreshing = false;
            RaisePropertyChanged("IsRefreshing");
        }

        private void HandleLeaderTapped(LeaderSummaryViewModel leader)
        {
            if (leader.IsMe)
                Shell.Current.Navigation.PushAsync(new MyProfile());
            else
                Shell.Current.Navigation.PushAsync(new MyProfile(leader));
        }
    }
}
