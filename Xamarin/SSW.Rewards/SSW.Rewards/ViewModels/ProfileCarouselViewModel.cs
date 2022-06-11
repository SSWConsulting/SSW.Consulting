﻿using SSW.Rewards.Models;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace SSW.Rewards.ViewModels
{
    public class ProfileCarouselViewModel
    {
        public CarouselType Type { get; set; }

        public List<ProfileAchievement> Achievements { get; set; } = new List<ProfileAchievement>();

        public List<Activity> RecentActivity { get; set; } = new List<Activity>();

        public List<Notification> Notifications { get; set; } = new List<Notification>();

        public bool IsMe { get; set; }

        public string ProfileName { get; set; }

        public string EmptyHeader
        {
            get
            {
                return IsMe ? "You have no recent activity" : $"{ProfileName} has no recent activity";
            }
        }
    }

    public enum CarouselType
    {
        Achievements,
        RecentActivity,
        Notifications
    }

    public class ProfileAchievement : Achievement
    {
        public ICommand AchievementTappedCommand { get; set; }

        public static string AchievementTappedMessage = "AchivementTapped";

        public ProfileAchievement()
        {
            AchievementTappedCommand = new Command(() => MessagingCenter.Send<object>(this, AchievementTappedMessage));
        }
    }

    public static class AchievementHelpers
    {
        public static ProfileAchievement ToProfileAchievement(this Achievement achievement)
        {
            return new ProfileAchievement
            {
                AwardedAt = achievement.AwardedAt,
                Complete = achievement.Complete,
                Name = achievement.Name,
                Type = achievement.Type,
                Value = achievement.Value,
                AchievementIcon = achievement.AchievementIcon,
                IconIsBranded = achievement.IconIsBranded
            };
        }
    }
}
