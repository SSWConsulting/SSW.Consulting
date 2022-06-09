﻿using SSW.Rewards.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SSW.Rewards.Services
{
    public class LeaderService : BaseService, ILeaderService
    {
        private LeaderboardClient _leaderBoardClient;

        public LeaderService()
        {
            _leaderBoardClient = new LeaderboardClient(BaseUrl, AuthenticatedClient);
        }

        public async Task<IEnumerable<LeaderSummary>> GetLeadersAsync(bool forceRefresh)
        {
            List<LeaderSummary> summaries = new List<LeaderSummary>();

            try
            {
                var apiLeaderList = await _leaderBoardClient.GetAsync();

                foreach (var Leader in apiLeaderList.Users)
                {
                    LeaderSummary leaderSummary = new LeaderSummary
                    {
                        BaseScore = Leader.Points,
                        id = Leader.UserId,
                        Name = Leader.Name,
                        Rank = Leader.Rank,
                        ProfilePic = string.IsNullOrWhiteSpace(Leader.ProfilePic?.ToString()) ? "v2sophie" : Leader.ProfilePic.ToString()
                    };

                    summaries.Add(leaderSummary);
                }
            }
            catch(ApiException e)
            {
                if(e.StatusCode == 401)
                {
                    await App.Current.MainPage.DisplayAlert("Authentication Failure", "Looks like your session has expired. Choose OK to go back to the login screen.", "OK");
                    Application.Current.MainPage = new SSW.Rewards.Views.LoginPage();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Oops...", "There seems to be a problem loading the leaderboard. Please try again soon.", "OK");
                }
            }

            return summaries;
        }
    }
}
