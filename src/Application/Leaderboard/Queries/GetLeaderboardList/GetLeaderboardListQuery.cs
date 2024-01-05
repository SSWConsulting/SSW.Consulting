﻿using AutoMapper.QueryableExtensions;
using SSW.Rewards.Shared.DTOs.Leaderboard;

namespace SSW.Rewards.Application.Leaderboard.Queries.GetLeaderboardList;

public class GetLeaderboardListQuery : IRequest<LeaderboardViewModel>
{

}

public class Handler : IRequestHandler<GetLeaderboardListQuery, LeaderboardViewModel>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;

    public Handler(
        IMapper mapper,
        IApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<LeaderboardViewModel> Handle(GetLeaderboardListQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.Users
            .Where(u => u.Activated)
            .Include(u => u.UserAchievements)
            .ThenInclude(ua => ua.Achievement)
            .ProjectTo<LeaderboardUserDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var model = new LeaderboardViewModel
        {
            // need to set rank outside of AutoMapper
            Users = users
                .Where(u => !string.IsNullOrWhiteSpace(u.Name))
                .OrderByDescending(u => u.TotalPoints)
                .Select((u, i) =>
                {
                    u.Rank = i + 1;
                    return u;
                }).ToList()
        };

        return model;
    }
}
