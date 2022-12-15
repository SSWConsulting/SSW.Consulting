﻿namespace SSW.Rewards.Services;

public class BaseService
{
    protected HttpClient AuthenticatedClient => AuthenticatedClientFactory.GetClient();

    protected string BaseUrl = Constants.ApiBaseUrl;
}
