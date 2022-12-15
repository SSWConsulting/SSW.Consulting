﻿using Maui.Plugins.PageResolver;
using SSW.Rewards.Mobile.Pages;

namespace SSW.Rewards.Services;

public class LeaderService : BaseService, ILeaderService
{
    private LeaderboardClient _leaderBoardClient;

    public LeaderService()
    {
        _leaderBoardClient = new LeaderboardClient(BaseUrl, AuthenticatedClient);
    }

    public async Task<IEnumerable<LeaderboardUserDto>> GetLeadersAsync(bool forceRefresh)
    {
        List<LeaderboardUserDto> summaries = new List<LeaderboardUserDto>();

        try
        {
            var apiLeaderList = await _leaderBoardClient.GetAsync();

            foreach (var leader in apiLeaderList.Users)
            {
                if (string.IsNullOrWhiteSpace(leader.ProfilePic))
                {
                    leader.ProfilePic = "v2sophie";
                }

                summaries.Add(leader);
            }
        }
        catch(ApiException e)
        {
            if(e.StatusCode == 401)
            {
                await App.Current.MainPage.DisplayAlert("Authentication Failure", "Looks like your session has expired. Choose OK to go back to the login screen.", "OK");
                await Application.Current.MainPage.Navigation.PushModalAsync<LoginPage>();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Oops...", "There seems to be a problem loading the leaderboard. Please try again soon.", "OK");
            }
        }

        return summaries;
    }
}
