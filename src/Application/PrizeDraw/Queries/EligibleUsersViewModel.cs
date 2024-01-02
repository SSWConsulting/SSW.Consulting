﻿namespace SSW.Rewards.Application.PrizeDraw.Queries;
public class EligibleUsersViewModel
{
    public int AchievementId { get; set; }
    public string AchievementName { get; set; } = string.Empty;
    public IEnumerable<EligibleUserDto> EligibleUsers { get; set; } = new List<EligibleUserDto>();
}
