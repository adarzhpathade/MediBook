# Memory — Deployment & Post-Launch Fixes

Last updated: 2026-07-11

## What was built

- Connected the project to a GitHub repository (`adarzhpathade/MediBook`) and pushed all code.
- Reverted the `.NET 8` downgrade back to `.NET 10` to ensure local development compatibility.
- Fixed a 500 Internal Server Error crashing the app locally.
- Fixed a CSS bug where the mobile menu bar icons and shadows were cut off on smaller screens.
- Created a `Dockerfile` and `.dockerignore` to allow 1-click cloud deployment.
- Updated `README.md` to include deployment instructions, .NET 10 requirements, and default seed credentials.

## Decisions made

- **Cookie Security:** Switched `CookieSecurePolicy.Always` to `CookieSecurePolicy.SameAsRequest`. This secures session/antiforgery tokens in production (HTTPS) without crashing local development (HTTP).
- **Deployment Platform:** Successfully deployed the Docker container to Render. It is live and functioning.
- **Docker Image:** Specifically added `libgssapi-krb5-2` to the `aspnet:10.0` runtime image to silence a known `Npgsql` Kerberos warning.

## Problems solved

- **File Lock Errors:** Fixed `dotnet watch run` build lock errors (`warning MSB3026`) by forcefully terminating dangling `MediBook.exe` processes (`taskkill`).
- **Postgres Linux Warnings:** Silenced the `libgssapi_krb5.so.2: cannot open shared object file` warning in cloud logs by installing the library in the Dockerfile.

## Current state

The application is 100% finished, fully containerized, pushed to GitHub, and successfully deployed to the internet via Render (`medibook-w3uw.onrender.com`). Local development runs flawlessly.

## Next session starts with

Exploring the live application, confirming there are no real-world deployment bugs, or adding any new post-launch features as requested. (Optional: Add a real application screenshot to the `assets/` folder and link it in the `README.md`).

## Open questions

None.
