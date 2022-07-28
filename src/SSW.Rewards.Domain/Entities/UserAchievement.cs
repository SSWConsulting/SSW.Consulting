﻿using System;

namespace SSW.Rewards.Domain.Entities
{
    public class UserAchievement : Entity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int AchievementId { get; set; }
        public Achievement Achievement { get; set; }
        public DateTime AwardedAt { get; set; }
    }
}


