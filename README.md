# Harbour API

A small HTTP API for guest harbour berths on the Norwegian west coast: which harbours there are, which berths are free for a boat and a stay, and how full a harbour is on a night. An ASP.NET Core minimal API on .NET 10, with the data held in memory.

It exists to demonstrate [ourfault](https://ourfault.dev). It sends no telemetry, so ourfault's "Set it up for me" can open a pull request that adds its OpenTelemetry setup; the repository holds exactly one service for it to find, `src/HarbourApi`.

## Run it

```bash
dotnet run --project src/HarbourApi     # http://localhost:5090
dotnet test
```

| Endpoint | Answers |
|---|---|
| `GET /harbours` | Each harbour's code, name, town and number of berths |
| `GET /harbours/{code}/berths/available?arrival=2026-07-02&nights=2&length=11` | The berths that take a boat of that length in metres for every night of the stay, smallest first; 400 for 0 or more than 28 nights or a length of 0 |
| `GET /harbours/{code}/occupancy?date=2026-07-02` | Berths taken that night, out of how many, and the percentage |
| `GET /healthz` | 200 |

An unknown harbour is 404. The harbours are `BGO` (Bergen), `SVG` (Stavanger) and `AES` (Ålesund), whose berths are not built yet.

## Known bug

`GET /harbours/AES/occupancy?date=2026-07-02` answers 500: `AvailabilityService.OccupancyOn` (`src/HarbourApi/Harbours/AvailabilityService.cs` line 23) divides by the harbour's berth count, which is zero for Ålesund, and throws `DivideByZeroException`. It is left in for a later demonstration of a fix ourfault proposes, once the API sends its errors.

## Structure

```
src/HarbourApi/           Program.cs, and Harbours/: the models, the in-memory directory and the availability service
tests/HarbourApi.Tests/   xunit v3 on Microsoft Testing Platform, against the API in memory
```

## Licence

MIT, see `LICENSE`.
