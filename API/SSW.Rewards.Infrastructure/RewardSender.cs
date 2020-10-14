﻿using Microsoft.Extensions.Logging;
using SSW.Rewards.Application.Common.Interfaces;
using SSW.Rewards.Application.Common.Models;
using SSW.Rewards.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SSW.Rewards.Infrastructure
{
    public class RewardSender : IRewardSender
    {
        private readonly ISSWRewardsDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ILogger<RewardSender> _logger;

        public RewardSender(ISSWRewardsDbContext context, IEmailService emailService, ILogger<RewardSender> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
        }

        public Task SendReward(User user, Reward reward)
        {
            return SendRewardAsync(user, reward, CancellationToken.None);
        }

        public async Task SendRewardAsync(User user, Reward reward, CancellationToken cancellationToken)
        {
            var entity = new UserReward
            {
                AwardedAt = DateTime.Now,
                Reward = reward,
                User = user
            };

            _context.UserRewards.Add(entity);

            string emailSubject = "SSW Rewards - Reward Notification";
            
            // TODO: These values are hard coded here but will be 
            // replaced with the user's details once we implement
            // physical address functionality and automatic sending
            // of rewards
            string recipientEmail = "marketing@ssw.com.au";
            string recipientName = "SSW Marketing";

            bool rewardSentSuccessully;

            if (reward.RewardType == RewardType.Physical)
            {
                PhysicalRewardEmail emailProps = new PhysicalRewardEmail
                {
                    RecipientAddress = "Please contact user for address", // TODO: when we implement addess capture, change to: user.Address.ToString(),
                    RecipientName = user.FullName,
                    RewardName = reward.Name
                };

                rewardSentSuccessully = await _emailService.SendPhysicalRewardEmail(recipientEmail, recipientName, emailSubject, emailProps, cancellationToken);
            }
            else
            {
                // TODO: Replace this with logic that:
                // 1. Determines if it is a free ticket
                // 2. If yes, generate with the appropriate API (currently eventbrite) and send to user
                // 3. If no, send notification to Marketing for appropriate action

                string voucherCode = "Please generate for user";

                //end TODO

                DigitalRewardEmail emailProps = new DigitalRewardEmail
                {
                    RecipientName = user.FullName,
                    RecipientEmail = user.Email,
                    RewardName = reward.Name,
                    VoucherCode = voucherCode
                };

                rewardSentSuccessully = await _emailService.SendDigitalRewardEmail(recipientEmail, recipientName, emailSubject, emailProps, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            if(!rewardSentSuccessully)
            {
                _logger.LogError("Error sending email reward notification.", reward, user);
            }
        }
    }
}
