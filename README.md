# JoRPA OAuth Service

This repository contains the standalone OAuth service for JoRPA. The current
implementation is Phase 1: a health endpoint and provider callback routes are
available, but no provider credentials or token processing are configured yet.

## Run locally

```powershell
dotnet run --project .\WebApplication1\WebApplication1\WebApplication1.csproj
```

Endpoints:

- `GET /health` returns `{ "status": "ok" }`.
- `GET /oauth/billit/callback`
- `GET /oauth/exact/callback`

The callback routes intentionally do not display or log query-string values.
OAuth session validation and token exchange will be added in the Billit POC
phase after the provider's current documentation has been verified.

## Railway deployment

Railway can deploy the repository using the root `Dockerfile`. The container
listens on port `8080` through `ASPNETCORE_HTTP_PORTS`; Railway should route its
public HTTPS domain to that port.

For the first deployment:

1. Create a Railway service from this repository and select the Dockerfile
   deployment.
2. Generate a public domain in the Railway service settings.
3. Verify `https://<domain>/health`.
4. Register the final HTTPS callback URI with each provider only after the
   provider-specific endpoint and redirect requirements have been confirmed.

Do not add client secrets to this repository. Provider credentials will be
supplied through Railway environment variables in a later phase.
