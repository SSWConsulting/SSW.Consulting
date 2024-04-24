﻿
using SSW.Rewards.Enums;
using SSW.Rewards.Shared.DTOs.Rewards;
using IApiRewardService = SSW.Rewards.ApiClient.Services.IRewardService;

namespace SSW.Rewards.Mobile.Services;

public class RewardService : IRewardService
{
    private readonly IApiRewardService _rewardClient;
    private readonly IUserService _userService;

    public RewardService(IApiRewardService rewardClient, IUserService userService)
    {
        _rewardClient = rewardClient;
        _userService = userService;
    }

    public async Task<List<Reward>> GetRewards()
    {
        var rewardList = new List<Reward>();

        try
        {
            var rewards = await _rewardClient.GetRewards(CancellationToken.None);

            foreach (var reward in rewards.Rewards)
            {
                rewardList.Add(new Reward
                {
                    Cost = reward.Cost,
                    Id = reward.Id,
                    ImageUri = reward.ImageUri,
                    Name = reward.Name,
                    Description = reward.Description,
                    CarouselImageUri = reward.CarouselImageUri,
                    IsCarousel = reward.IsCarousel,
                    IsHidden = reward.IsHidden
                });
            }

            return rewardList;
        }
        catch (Exception e)
        {
            if (!await ExceptionHandler.HandleApiException(e))
            {
                await App.Current.MainPage.DisplayAlert("Oops...", "There seems to be a problem loading the leaderboard. Please try again soon.", "OK");
            }
        }

        return rewardList;
    }

    public async Task<ClaimRewardResult> ClaimReward(ClaimRewardDto claim)
    {
        var result = new ClaimRewardResult() { status = RewardStatus.Error };

        try
        {
            result = await _rewardClient.RedeemReward(claim, CancellationToken.None);
        }
        catch (Exception e)
        {
            // TODO: Handle errors
            if (! await ExceptionHandler.HandleApiException(e))
            {
            }
        }

        await _userService.UpdateMyDetailsAsync();

        return result;
    }
    
    public async Task<CreatePendingRedemptionResult> CreatePendingRedemption(ClaimRewardDto claim)
    {
        var result = new CreatePendingRedemptionResult { status = RewardStatus.Error };

        try
        {
            result = await _rewardClient.CreatePendingRedemption(claim, CancellationToken.None);
        }
        catch (Exception e)
        {
            // TODO: Handle errors
            if (! await ExceptionHandler.HandleApiException(e))
            {
            }
        }

        await _userService.UpdateMyDetailsAsync();

        return result;
    }
}
