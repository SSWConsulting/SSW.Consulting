﻿using Rg.Plugins.Popup.Services;
using SSW.Rewards.Controls;
using SSW.Rewards.Models;
using SSW.Rewards.PopupPages;
using SSW.Rewards.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SSW.Rewards.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private IUserService _userService;

        public string ProfilePic { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Points { get; set; }
        public int Balance { get; set; }

        private int userId { get; set; }

        public bool IsLoading { get; set; }

        public ObservableCollection<ProfileCarouselViewModel> ProfileSections { get; set; } = new ObservableCollection<ProfileCarouselViewModel>();

        public SnackbarOptions SnackOptions { get; set; }

        public ICommand CameraCommand => new Command(async () => await ShowCameraPageAsync());

        private bool _isMe;

        public EventHandler<ShowSnackbarEventArgs> ShowSnackbar;

        public ProfileViewModel(IUserService userService)
        {
            IsLoading = true;
            RaisePropertyChanged("IsLoading");
            _userService = userService;

            SnackOptions = new SnackbarOptions
            {
                ActionCompleted = true,
                GlyphIsBand = true,
                Glyph = "\uf420",
                Message = "You have completed the Angular quiz",
                Points = 1000,
                ShowPoints = true
            };
        }

        public ProfileViewModel(LeaderSummaryViewModel vm)
        {
            IsLoading = true;
            RaisePropertyChanged("IsLoading");
            ProfilePic = vm.ProfilePic;
            Name = vm.Name;
            Email = vm.Title;
            userId = vm.Id;
            Points = vm.Score;
            // TODO: add this to LeaderSummaryViewModel
            // Balance = vm.Balance;
        }

        public async Task Initialise(bool me)
        {
            MessagingCenter.Subscribe<object>(this, "ProfilePicChanged", async (obj) => { await Refresh(); });
            MessagingCenter.Subscribe<object>(this, ProfileAchievement.AchievementTappedMessage, (obj) => ShowAchievementSnackbar((ProfileAchievement)obj));

            _isMe = me;

            if (_isMe)
            {
                var profilePic = _userService.MyProfilePic;

                //initialise me
                ProfilePic = profilePic;
                Name = _userService.MyName;
                Email = _userService.MyEmail;
                Points = _userService.MyPoints;
                Balance = _userService.MyBalance;
            }
            else
            {
                //initialise other
                _userService = Resolver.Resolve<IUserService>();
            }

            if (!ProfileSections.Any())
            {
                await LoadProfileSections();
            }

            IsLoading = false;

            RaisePropertyChanged(nameof(IsLoading), nameof(Name), nameof(ProfilePic), nameof(Points), nameof(Balance));
        }

        private async Task ShowCameraPageAsync()
        {
            await PopupNavigation.Instance.PushAsync(new CameraPage());
        }

        private async Task LoadProfileSections()
        {
            var rewardList = await _userService.GetRewardsAsync();
            var profileAchievements = await _userService.GetProfileAchievementsAsync();
            var achievementList = await _userService.GetAchievementsAsync();

            //===== Achievements =====

            var achivementsSection = new ProfileCarouselViewModel();
            achivementsSection.Type = CarouselType.Achievements;

            foreach (var achievement in profileAchievements)
            {
                achivementsSection.Achievements.Add(achievement.ToProfileAchievement());
            }

            ProfileSections.Add(achivementsSection);
            
            // ===== Recent activity =====

            var activitySection = new ProfileCarouselViewModel();
            activitySection.Type = CarouselType.RecentActivity;

            var activityList = new List<Activity>();

            var recentAchievements = achievementList.OrderByDescending(a => a.AwardedAt).Take(10);

            foreach (var achievement in recentAchievements)
            {
                activityList.Add(new Activity
                {
                    ActivityName = $"{achievement.Type.ToActivityType()} {achievement.Name}",
                    OcurredAt = achievement.AwardedAt,
                    Type = achievement.Type.ToActivityType()
                });
            }

            var recentRewards = rewardList
                .Where(r => r.Awarded == true)
                .OrderByDescending(r => r.AwardedAt).Take(10);

            foreach (var reward in rewardList)
            {
                activityList.Add(new Activity
                {
                    ActivityName = $"Claimed {reward.Name}",
                    OcurredAt = reward.AwardedAt?.DateTime,
                    Type = ActivityType.Claimed
                });
            }

            activityList.OrderByDescending(a => a.OcurredAt).Take(10).ToList().ForEach(a => activitySection.RecentActivity.Add(a));

            ProfileSections.Add(activitySection);

            // ===== Notifications =====

            if (_isMe)
            {
                var notificationsSection = new ProfileCarouselViewModel();
                notificationsSection.Type = CarouselType.Notifications;

#if DEBUG
                notificationsSection.Notifications.Add(new Notification
                {
                    Message = "New tech trivia available: Scrum",
                    Type = NotificationType.Alert
                });

                notificationsSection.Notifications.Add(new Notification
                {
                    Message = "Upcoming event: SSW User Group June 2022 with Jason Taylor",
                    Type = NotificationType.Event
                });

                notificationsSection.Notifications.Add(new Notification
                {
                    Message = "Upcoming event: How to design darkmode mobile UI with Jayden Alchin",
                    Type = NotificationType.Event
                });
#endif

                ProfileSections.Add(notificationsSection);
            }
        }

        private async Task Refresh()
        {
            ProfilePic = _userService.MyProfilePic;
            RaisePropertyChanged(nameof(ProfilePic));
        }

        private void ShowAchievementSnackbar(ProfileAchievement achievement)
        {
            // TODO: set Glyph when given values
            var optsions = new SnackbarOptions
            {
                ActionCompleted = achievement.Complete,
                Points = achievement.Value,
                Message = $"{achievement.Type} {achievement.Name}"
            };

            var args = new ShowSnackbarEventArgs { Options = optsions };

            ShowSnackbar.Invoke(this, args);
        }
    }
}
