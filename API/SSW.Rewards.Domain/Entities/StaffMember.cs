﻿using System.Collections.Generic;

namespace SSW.Rewards.Domain.Entities
{
    public class StaffMember : Entity
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public string Email { get; set; }

        public string Profile { get; set; }

        public string TwitterUsername { get; set; }
        public string GitHubUsername { get; set; }
        public string LinkedInUrl { get; set; }


        public bool IsExternal { get; set; }

        public Achievement StaffAchievement { get; set; }

        public ICollection<StaffMemberSkill> StaffMemberSkills { get; set; } = new HashSet<StaffMemberSkill>();

        public string ProfilePhoto { get; set; }
        public bool IsDeleted { get; set; }
    }
}


