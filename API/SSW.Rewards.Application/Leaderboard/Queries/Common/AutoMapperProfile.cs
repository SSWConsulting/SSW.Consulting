﻿using System.Data;
using AutoMapper;
using System;
using System.Linq;

namespace SSW.Rewards.Application.Leaderboard.Queries.Common
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Domain.Entities.User, LeaderboardUserDto>()
                .ForMember(dst => dst.UserId, opt => opt.Ignore())
                .ForMember(dst => dst.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dst => dst.ProfilePic, opt => opt.MapFrom(src => src.Avatar))
                .ForMember(dst => dst.TotalPoints, opt => opt.MapFrom(src => src.UserAchievements
                                                                                    .Sum(ua => ua.Achievement.Value)))
                .ForMember(dst => dst.Balance, opt => opt.MapFrom(src => src.UserAchievements.Sum(ua => ua.Achievement.Value)
                                                                       - src.UserRewards.Sum(ur =>ur.Reward.Cost)))
                .ForMember(dst => dst.PointsThisYear, opt => opt.MapFrom(src => src.UserAchievements
                                                                                    .Where(ua => ua.AwardedAt.Year == DateTime.Now.Year)
                                                                                    .Sum(ua => ua.Achievement.Value)))
                .ForMember(dst => dst.PointsThisMonth, opt => opt.MapFrom(src => src.UserAchievements
                                                                                    .Where(ua => ua.AwardedAt.Year == DateTime.Now.Year && ua.AwardedAt.Month == DateTime.Now.Month)
                                                                                    .Sum(ua => ua.Achievement.Value)));
        }
    }

}
