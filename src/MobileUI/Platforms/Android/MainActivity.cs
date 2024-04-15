﻿using Android.App;
using Android.Content.PM;
using Android.Gms.Extensions;
using Android.OS;
using Firebase.Messaging;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.PlatformConfiguration;
using SSW.Rewards.Mobile.Platforms.Android;
using Color = Microsoft.Maui.Graphics.Color;

namespace SSW.Rewards.Mobile;

[Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density, ScreenOrientation = ScreenOrientation.Portrait)]
public class MainActivity : MauiAppCompatActivity
{
    internal static readonly string Channel_ID = "General";
    internal static readonly int NotificationID = 101;

    protected async override void OnCreate(Bundle savedInstanceState)
    {
        var MainBackground = Color.FromArgb("#181818").ToAndroid();
        Window!.SetNavigationBarColor(MainBackground);
        base.OnCreate(savedInstanceState);
        // await AzureNotificationHubService.RegisterDevice(
        //     "ntfns-sswrewards-staging",
        //     "smnDd7R/cxaTHu5l/IgqHi50MZ+NpNBUYcFDDdcrKgI="
        // );
        CreateNotificationChannel();
        var token = await FirebaseMessaging.Instance.GetToken();
        // FirebaseMessaging.Instance.AutoInitEnabled = false;
    }

    private void CreateNotificationChannel()
    {
        if (OperatingSystem.IsOSPlatformVersionAtLeast("android", 26))
        {
            var channel = new NotificationChannel(Channel_ID, "General", NotificationImportance.Default);

            var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }
    }
}
