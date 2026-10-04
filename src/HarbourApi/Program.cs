using HarbourApi.Harbours;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<HarbourDirectory>();
builder.Services.AddSingleton<AvailabilityService>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

app.MapGet("/harbours", (HarbourDirectory directory) =>
    directory.All.Select(harbour => new HarbourSummary(harbour.Code, harbour.Name, harbour.Town, harbour.Berths.Count)));

app.MapGet("/harbours/{code}/berths/available", (string code, DateOnly arrival, int nights, decimal length, HarbourDirectory directory, AvailabilityService availability) =>
{
    if (directory.Find(code) is not { } harbour)
    {
        return Results.NotFound();
    }
    if (nights is < 1 or > 28 || length <= 0)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["nights"] = ["A stay is 1 to 28 nights."],
            ["length"] = ["The boat's length is in metres, above zero."],
        });
    }
    return Results.Ok(availability.Available(harbour, arrival, nights, length));
});

app.MapGet("/harbours/{code}/occupancy", (string code, DateOnly date, HarbourDirectory directory, AvailabilityService availability) =>
    directory.Find(code) is { } harbour ? Results.Ok(availability.OccupancyOn(harbour, date)) : Results.NotFound());

app.MapGet("/healthz", () => Results.Text("ok"));

app.Run();
