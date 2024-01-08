﻿using CommunityToolkit.Mvvm.Messaging;
using IdentityModel.Client;
using IdentityModel.OidcClient;
using Microsoft.AppCenter.Crashes;
using SSW.Rewards.Mobile.Messages;
using System.IdentityModel.Tokens.Jwt;
using IBrowser = IdentityModel.OidcClient.Browser.IBrowser;

namespace SSW.Rewards.Mobile.Services;

public class UserService : ApiBaseService, IUserService
{
    private UserClient _userClient { get; set; }

    private readonly OidcClientOptions _options;

    private bool _loggedIn = false;

    private string RefreshToken;

    public bool HasCachedAccount { get => Preferences.Get(nameof(HasCachedAccount), false); }

    public UserService(IBrowser browser, IHttpClientFactory clientFactory, ApiOptions options) : base(clientFactory, options)
    {
        _options = new OidcClientOptions
        {
            Authority = Constants.AuthorityUri,
            ClientId = Constants.ClientId,
            Scope = Constants.Scope,
            RedirectUri = Constants.AuthRedirectUrl,
            Browser = browser,

        };

        _userClient = new UserClient(BaseUrl, AuthenticatedClient);
    }

    public bool IsLoggedIn { get => _loggedIn; }

    public async Task<bool> DeleteProfileAsync()
    {
        try
        {
            var result = await _userClient.DeleteMyProfileAsync();

            SignOut();

            return true;
        }
        catch
        {
            return false;
        }

    }


    #region USERDETAILS

    public int MyUserId { get => Preferences.Get(nameof(MyUserId), 0); }

    public string MyEmail { get => Preferences.Get(nameof(MyEmail), string.Empty); }

    public string MyName { get => Preferences.Get(nameof(MyName), string.Empty); }

    public int MyPoints { get => Preferences.Get(nameof(MyPoints), 0); }

    public int MyBalance { get => Preferences.Get(nameof(MyBalance), 0); }

    public string MyQrCode { get => Preferences.Get(nameof(MyQrCode), string.Empty); }

    public string MyProfilePic
    {
        get
        {
            var pic = Preferences.Get(nameof(MyProfilePic), string.Empty);
            if (!string.IsNullOrWhiteSpace(pic))
                return pic;

            return "v2sophie";
        }
    }

    public bool IsStaff { get => !string.IsNullOrWhiteSpace(MyQrCode); }

    public async Task<string> UploadImageAsync(Stream image)
    {
        var contentType = "image/png";
        var fileName = $"{MyUserId}_{DateTime.UtcNow.Ticks}_profilepic.png";

        FileParameter parameter = new FileParameter(image, fileName, contentType);

        var response = await _userClient.UploadProfilePicAsync(parameter);
        Preferences.Set(nameof(MyProfilePic), response.PicUrl);

        if (response.AchievementAwarded)
        {
            await UpdateMyDetailsAsync();
        }

        WeakReferenceMessenger.Default.Send(new ProfilePicUpdatedMessage
        {
            ProfilePic = MyProfilePic
        });
        return response.PicUrl;
    }

    public async Task UpdateMyDetailsAsync()
    {
        var user = await _userClient.GetAsync();

        if (user is null)
        {
            return;
        }

        if (!string.IsNullOrWhiteSpace(user.FullName))
        {
            Preferences.Set(nameof(MyName), user.FullName);
        }

        if (!string.IsNullOrWhiteSpace(user.Email))
        {
            Preferences.Set(nameof(MyEmail), user.Email);
        }

        if (!string.IsNullOrWhiteSpace(user.Id.ToString()))
        {
            Preferences.Set(nameof(MyUserId), user.Id);
        }

        if (!string.IsNullOrWhiteSpace(user.ProfilePic))
        {
            Preferences.Set(nameof(MyProfilePic), user.ProfilePic);
        }

        if (!string.IsNullOrWhiteSpace(user.Points.ToString()))
        {
            Preferences.Set(nameof(MyPoints), user.Points);
        }

        if (!string.IsNullOrWhiteSpace(user.Balance.ToString()))
        {
            Preferences.Set(nameof(MyBalance), user.Balance);
        }

        if (user.QrCode != null && !string.IsNullOrWhiteSpace(user.QrCode.ToString()))
        {
            Preferences.Set(nameof(MyQrCode), user.QrCode);
        }

        WeakReferenceMessenger.Default.Send(new UserDetailsUpdatedMessage(new UserContext
        {
            Email       = MyEmail,
            ProfilePic  = MyProfilePic,
            Name        = MyName,
            IsStaff     = IsStaff
        }));
    }

    public async Task<IEnumerable<Achievement>> GetAchievementsAsync()
    {
        return await GetAchievementsForUserAsync(MyUserId);
    }

    public async Task<IEnumerable<Achievement>> GetAchievementsAsync(int userId)
    {
        return await GetAchievementsForUserAsync(userId);
    }

    public async Task<IEnumerable<Achievement>> GetProfileAchievementsAsync()
    {
        return await GetProfileAchievementsAsync(MyUserId);
    }

    public async Task<IEnumerable<Achievement>> GetProfileAchievementsAsync(int userId)
    {
        List<Achievement> achievements = new List<Achievement>();

        var achievementsList = await _userClient.ProfileAchievementsAsync(userId);

        foreach (UserAchievementDto achievement in achievementsList.UserAchievements)
        {
            achievements.Add(new Achievement
            {
                Complete = achievement.Complete,
                Name = achievement.AchievementName,
                Value = achievement.AchievementValue,
                Type = achievement.AchievementType,
                AwardedAt = achievement.AwardedAt?.DateTime,
                AchievementIcon = achievement.AchievementIcon,
                IconIsBranded = achievement.AchievementIconIsBranded,
                Id = achievement.AchievementId
            });
        }

        return achievements;
    }

    private async Task<IEnumerable<Achievement>> GetAchievementsForUserAsync(int userId)
    {
        List<Achievement> achievements = new List<Achievement>();

        var achievementsList = await _userClient.AchievementsAsync(userId);

        foreach (UserAchievementDto achievement in achievementsList.UserAchievements)
        {
            achievements.Add(new Achievement
            {
                Complete = achievement.Complete,
                Name = achievement.AchievementName,
                Value = achievement.AchievementValue,
                Type = achievement.AchievementType,
                AwardedAt = achievement.AwardedAt?.DateTime
            });
        }

        return achievements;
    }

    public async Task<IEnumerable<Reward>> GetRewardsAsync()
    {
        return await GetRewardsForUserAsync(MyUserId);
    }

    public async Task<IEnumerable<Reward>> GetRewardsAsync(int userId)
    {
        return await GetRewardsForUserAsync(userId);
    }

    private async Task<IEnumerable<Reward>> GetRewardsForUserAsync(int userId)
    {
        List<Reward> rewards = new List<Reward>();

        var rewardsList = await _userClient.RewardsAsync(userId);

        foreach (var userReward in rewardsList.UserRewards)
        {
            rewards.Add(new Reward
            {
                Awarded = userReward.Awarded,
                Name = userReward.RewardName,
                Cost = userReward.RewardCost,
                AwardedAt = userReward.AwardedAt?.DateTime
            });
        }

        return rewards;
    }

    public Task<ImageSource> GetProfilePicAsync(string url)
    {
        throw new NotImplementedException();
    }

    public Task<ImageSource> GetAvatarAsync(string url)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SaveSocialMediaId(int achievementId, string userId)
    {
        var achieved = await _userClient.UpsertUserSocialMediaIdAsync(new UpsertUserSocialMediaId
        {
            AchievementId = achievementId,
            SocialMediaPlatformUserId = userId
        });

        if (achieved == 0)
        {
            return false;
        }

        WeakReferenceMessenger.Default.Send(new PointsAwardedMessage());
        return true;
    }

    #endregion
}
