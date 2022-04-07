﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSW.Rewards.Application.Common.Interfaces;
using SSW.Rewards.Application.Staff.Queries.GetStaffList;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SSW.Rewards.Application.Staff.Queries.GetStaffMemberProfile
{
    public class GetStaffMemberProfileQuery: IRequest<StaffDto>
    {
        public int Id { get; set; }

        public string email { get; set; }

        public bool GetByEmail { get; set; } = false;

        public sealed class Handler : IRequestHandler<GetStaffMemberProfileQuery, StaffDto>
        {
            private readonly IMapper _mapper;
            private readonly ISSWRewardsDbContext _dbContext;
            private readonly IProfileStorageProvider _storage;

            public Handler(
                IMapper mapper,
                ISSWRewardsDbContext dbContext,
                IProfileStorageProvider storage)
            {
                _mapper = mapper;
                _dbContext = dbContext;
                _storage = storage;
            }
            
            public async Task<StaffDto> Handle(GetStaffMemberProfileQuery request, CancellationToken cancellationToken)
            {
                var staffMember = await _dbContext.StaffMembers
                    .Include(s => s.StaffMemberSkills)
                    .ThenInclude(sms => sms.Skill)
                    .ProjectTo<StaffDto>(_mapper.ConfigurationProvider)
                    .Where(member => member.Id == request.Id)
                    .FirstOrDefaultAsync();

                return staffMember;
            }
        }
    }
}
