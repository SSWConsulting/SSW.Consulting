﻿namespace SSW.Rewards;

public static class Constants
{
#if DEBUG
    // public static string ApiBaseUrl = "https://sswconsulting-dev.azurewebsites.net";
    public static string ApiBaseUrl = "https://api.rewards.ssw.com.au";
    public static string AppCenterAndroidId = "285df68b-ea1b-4afb-94c3-2581613c6880";
    public static string AppCenterIOSId = "21efe682-dc49-4d39-8af8-ad05911be003";

#elif QA
    public string ApiBaseUrl = "https://sswconsulting-dev.azurewebsites.net";
#else
    public static string ApiBaseUrl = "https://api.rewards.ssw.com.au";
    public static string AppCenterAndroidId = "d6f591ec-8cef-44d7-96c0-08f31f91fb74";
    public static string AppCenterIOSId = "21efe682-dc49-4d39-8af8-ad05911be003";
#endif
    public static string MaxApiSupportedVersion = "1.0";

    public static string AuthRedirectUrl { get; } = "msauth.com.ssw.consulting://auth";

#if DEBUG
    public static string AuthorityUri { get; } = "https://identity.ssw.com.au"; // "https://sswidentity-stage.azurewebsites.net";

#else
    public static string AuthorityUri { get; } = "https://identity.ssw.com.au";
#endif
    public static string ClientId { get; } = "ssw-rewards-mobile-app";

    public static string Scope { get; } = "openid profile ssw-rewards-api email offline_access";

    public static int AnimationRepeatCount = 3;
}
