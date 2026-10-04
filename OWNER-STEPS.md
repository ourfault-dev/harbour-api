# Owner steps

How to demonstrate "Set it up for me" with this repository, which already exists as `ourfault-dev/harbour-api`.

1. Check that the ourfault GitHub App can see `harbour-api`. On 2026-10-04 the installations on `ourfault-dev` were on **All repositories**, so it does. If one is moved to selected repositories: GitHub, `ourfault-dev` organisation, **Settings**, **GitHub Apps**, the App, **Configure**, **Repository access**, add `harbour-api`, **Save**. The installation also needs Contents and Pull requests read and write, which it had that day.
2. On https://app.ourfault.dev, in the workspace that holds the installation, open **Setup** and choose **Set it up for me**. Choose `ourfault-dev/harbour-api`. Detection finds one service, `src/HarbourApi/HarbourApi.csproj` with `src/HarbourApi/Program.cs`, and suggests the service name `harbour-api`. Optionally name a token, then start it.
3. ourfault opens the draft pull request `[proposed] Send telemetry to ourfault` on the branch `ourfault/instrument`, adding the packages to `HarbourApi.csproj`, the setup to `Program.cs` and the environment lines to `README.md`. Review it in GitHub.
4. For the later proposed-fix demonstration: merge it, run the API with `OURFAULT_TOKEN` set and the README's environment, and call `GET /harbours/AES/occupancy?date=2026-07-02` (README, "Known bug"). With **Propose fixes as draft pull requests** on in the workspace's settings, the filed issue gets a draft fix.
