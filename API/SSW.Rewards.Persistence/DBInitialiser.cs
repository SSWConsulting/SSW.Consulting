﻿using Microsoft.EntityFrameworkCore;
using SSW.Rewards.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SSW.Rewards.Persistence
{
    public class DBInitialiser
    {
        private readonly SSWRewardsDbContext _context;

        public DBInitialiser(SSWRewardsDbContext context)
        {
            _context = context;
        }

        public void Run()
        {
            _context.Database.Migrate();
            EnsureDefaultRolesCreated();
        }

        private void EnsureDefaultRolesCreated()
        {
            if(!_context.Roles.Any())
            {
                _context.Roles.AddRange(DefaultRoles);
                _context.SaveChanges();
            }
        }

        private List<Role> DefaultRoles = new List<Role>()
        {
            new Role
            {
                Name = "User"
            },
            new Role
            {
                Name = "Staff"
            },
            new Role
            {
                Name = "Admin"
            }
        };

        public async Task EnsureStaffAndAdminRolesSeeded()
        {
            var users = await _context.Users.Where(u => !u.Roles.Any()).ToListAsync();

            var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
            var staffRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Staff");

            // TODO: move to config
            var staffSMTPDomain = "ssw.com.au";

            foreach(var user in users)
            {
                user.Roles.Add(new UserRole
                {
                    Role = userRole
                });

                var emailAddress = new MailAddress(user.Email);

                if (emailAddress.Host == staffSMTPDomain)
                {
                    user.Roles.Add(new UserRole
                    {
                        Role = staffRole
                    });
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
