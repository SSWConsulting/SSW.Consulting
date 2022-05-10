﻿namespace SSW.Rewards.Services
{
    /// <summary>Centralize the handling of notification actions</summary>
    public interface INotificationActionService
    {
        void TriggerAction(string action);
    }
}
