﻿using SSW.Rewards.Shared.DTOs.Rewards;

namespace SSW.Rewards.Mobile.Services;

public interface IRewardService
{
    Task<List<Reward>> GetRewards();
    Task <ClaimRewardResult> ClaimReward(ClaimRewardDto claim);
}
