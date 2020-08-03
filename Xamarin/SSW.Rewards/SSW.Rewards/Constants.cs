﻿using System;
using System.Collections.Generic;
using System.Text;
using SSW.Rewards.Helpers;

namespace SSW.Rewards
{
    class Constants
    {
#if DEBUG
        public const string ApiBaseUrl = "https://sswconsulting-dev.azurewebsites.net";
        public const string AppCenterAndroidId = "bfe53aa1-a7df-499d-900f-725a5222fc23";

#elif QA
        public const string ApiBaseUrl = "https://sswconsulting-dev.azurewebsites.net";
#else
        public const string ApiBaseUrl = "https://sswconsulting-prod.azurewebsites.net";
        public const string AppCenterAndroidId = "60b96e0a-c6dd-4320-855f-ed58e44ffd00";
#endif
        public const string MaxApiSupportedVersion = "1.0";

        public const string AADB2CClientId = Secrets.b2cClientId;
        public const string IOSKeychainSecurityGroups = "com.ssw.rewards";
        public const string AADB2CPolicySignin = Secrets.b2cSigninPolicy;
        public const string AADB2CTenantId = Secrets.b2cDomain;
        public const string ADDB2CTenantName = Secrets.b2cTenantName;
        public const string AADB2CPolicyReset = Secrets.b2cResetPolicy;
        
    }
}
