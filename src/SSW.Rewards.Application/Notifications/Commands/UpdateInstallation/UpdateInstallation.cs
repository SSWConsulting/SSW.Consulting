﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SSW.Rewards.Application.Common.Interfaces;
using SSW.Rewards.Application.Common.Models;

namespace SSW.Rewards.Application.Notifications.Commands.UpdateInstallation;
public class UpdateInstallation : IRequest<Unit>
{
    public string InstallationId { get; set; } = string.Empty;
    public string Platform { get; set; } = string.Empty;
    public string PushChannel { get; set; } = string.Empty;
    public IList<string> Tags { get; set; } = new List<string>();

    public UpdateInstallation(string installationId, string platform, string pushChannel, IList<string> tags)
    {
        InstallationId  = installationId;
        Platform        = platform;
        PushChannel     = pushChannel;
        Tags            = tags;
    }
}

public class UpdateInstallationHandler : IRequestHandler<UpdateInstallation, Unit>
{
    private readonly INotificationService _notificationService;

    public UpdateInstallationHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    public async Task<Unit> Handle(UpdateInstallation request, CancellationToken cancellationToken)
    {
        var deviceInstallation = new DeviceInstall
        {
            InstallationId  = request.InstallationId,
            Platform        = request.Platform,
            PushChannel     = request.PushChannel,
            Tags            = request.Tags
        };

        var success = await _notificationService
            .CreateOrUpdateInstallationAsync(deviceInstallation, cancellationToken);

        if (!success)
            throw new Exception("Bad Request");

        return Unit.Value;
    }
}