var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/health", () => Results.Ok(new
{
    status = "ok"
}))
.WithName("Health");

app.MapGet("/oauth/{provider}/callback", (string provider, string? error) =>
{
    if (!IsSupportedProvider(provider))
    {
        return Results.NotFound();
    }

    var title = error is null
        ? "OAuth callback received"
        : "OAuth authorization failed";
    var message = error is null
        ? "The callback was received. OAuth session processing is not configured yet."
        : "The OAuth provider reported an authorization error. You may close this window.";

    return Results.Content(
        $$"""
        <!doctype html>
        <html lang="en">
        <head><meta charset="utf-8"><title>{{title}}</title></head>
        <body><h1>{{title}}</h1><p>{{message}}</p></body>
        </html>
        """,
        "text/html; charset=utf-8");
})
.WithName("OAuthCallback");

app.Run();

static bool IsSupportedProvider(string provider) =>
    provider.Equals("billit", StringComparison.OrdinalIgnoreCase) ||
    provider.Equals("exact", StringComparison.OrdinalIgnoreCase);
