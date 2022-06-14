using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SSW.Rewards.Admin;
using MudBlazor.Services;
using SSW.Rewards;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// MessageHandler for adding the JWT to outbound requests to the API
builder.Services.AddTransient<CustomAuthorizationMessageHandler>();

string? apiBaseUrl = builder.Configuration["RewardsApiUrl"];
if (apiBaseUrl == null)
{
    throw new NullReferenceException("No API base URL provided");
}

// register services from nswag
const string generatedClientName = "generatedClient";
string baseUrl = apiBaseUrl.Replace("/api", string.Empty);
builder.Services.AddHttpClient(generatedClientName)
    .AddHttpMessageHandler(sp => sp.GetRequiredService<CustomAuthorizationMessageHandler>());

var generatedClients = typeof(Program).Assembly
    .GetTypes()
    .Where(t => t.IsAssignableTo(typeof(GeneratedClientBase)))
    .Select(s => new
    {
        Implementation = s,
        Interface = s.GetInterface($"I{s.Name}"), // nswag follows this convention
    })
    .Where(x => x.Interface != null);

foreach (var client in generatedClients)
{
    builder.Services.AddScoped(client.Interface!, sp =>
    {
        var ctor = client.Implementation.GetConstructor(new[] { typeof(string), typeof(HttpClient) })!;
        return ctor.Invoke(new object?[] { baseUrl, sp.GetService<IHttpClientFactory>()!.CreateClient(generatedClientName) });
    });
}

builder.Services.AddMudServices();

string[] authScopes = builder.Configuration.GetSection("Local:Scopes").Get<string[]>();

builder.Services.AddOidcAuthentication(options =>
{

    builder.Configuration.Bind("Local", options.ProviderOptions);

    foreach (var scope in authScopes)
    {
        options.ProviderOptions.DefaultScopes.Add(scope);
    }
    options.ProviderOptions.ResponseType = "code";
});

await builder.Build().RunAsync();
