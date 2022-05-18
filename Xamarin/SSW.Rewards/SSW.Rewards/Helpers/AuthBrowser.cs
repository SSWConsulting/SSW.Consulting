﻿using IdentityModel.OidcClient.Browser;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SSW.Rewards.Helpers
{
    public class AuthBrowser : IBrowser
    {
        public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
        {
            WebAuthenticatorResult authResult = await WebAuthenticator.AuthenticateAsync(new Uri(options.StartUrl), new Uri(App.Constants.AuthRedirectUrl));

            return new BrowserResult
            {
                Response = ParseAuthenticationResult(authResult)
            };
        }

        private string ParseAuthenticationResult(WebAuthenticatorResult result)
        {
            string code = result?.Properties["code"];
            string state = result?.Properties["state"];
            string scope = result?.Properties["scope"];
            string sessionState = result?.Properties["session_state"];

            return $"{App.Constants.AuthRedirectUrl}#code={code}&scope={scope}&state={state}&session_state={sessionState}";
        }
    }
}
