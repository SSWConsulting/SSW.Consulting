﻿using System;

namespace SSW.Rewards.Application.Users.Commands.UpsertUser
{
    public class AlreadyAwardedException : Exception
    {
        public AlreadyAwardedException(int userId, string achievementName)
            : base($"User \"{userId}\" has already been awarded {achievementName}.")
        {
        }
    }
}
