using System.Net;
using System.Net.Http.Json;
using HarbourApi.Harbours;
using Microsoft.AspNetCore.Mvc.Testing;

namespace HarbourApi.Tests;

public sealed class ApiTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    private static CancellationToken Cancel => TestContext.Current.CancellationToken;

    [Fact]
    public async Task Lists_the_harbours_with_their_berth_counts()
    {
        var harbours = await _client.GetFromJsonAsync<HarbourSummary[]>("/harbours", Cancel);

        Assert.NotNull(harbours);
        Assert.Equal(["BGO", "SVG", "AES"], harbours.Select(harbour => harbour.Code));
        Assert.Equal(6, harbours[0].Berths);
    }

    [Fact]
    public async Task Available_berths_fit_the_boat_and_are_free_every_night()
    {
        var berths = await _client.GetFromJsonAsync<Berth[]>("/harbours/BGO/berths/available?arrival=2026-07-02&nights=2&length=11", Cancel);

        // A3 is taken on the 2nd and A6 until the 6th; A1 and A2 are too short.
        Assert.Equal(["BGO-A4", "BGO-A5"], berths!.Select(berth => berth.Id));
    }

    [Fact]
    public async Task A_berth_is_free_again_on_the_departure_day()
    {
        var berths = await _client.GetFromJsonAsync<Berth[]>("/harbours/SVG/berths/available?arrival=2026-07-02&nights=1&length=10", Cancel);

        Assert.Contains("SVG-B2", berths!.Select(berth => berth.Id));
    }

    [Theory]
    [InlineData("/harbours/BGO/berths/available?arrival=2026-07-02&nights=0&length=11")]
    [InlineData("/harbours/BGO/berths/available?arrival=2026-07-02&nights=2&length=0")]
    public async Task A_stay_out_of_range_is_400(string path)
    {
        var response = await _client.GetAsync(path, Cancel);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("/harbours/OSL/berths/available?arrival=2026-07-02&nights=2&length=11")]
    [InlineData("/harbours/OSL/occupancy?date=2026-07-02")]
    public async Task An_unknown_harbour_is_404(string path)
    {
        var response = await _client.GetAsync(path, Cancel);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Occupancy_counts_the_berths_taken_that_night()
    {
        var occupancy = await _client.GetFromJsonAsync<Occupancy>("/harbours/BGO/occupancy?date=2026-07-02", Cancel);

        Assert.Equal(new Occupancy("BGO", new DateOnly(2026, 7, 2), 3, 6, 50), occupancy);
    }

    [Fact]
    public async Task Health_check_answers_200()
    {
        var response = await _client.GetAsync("/healthz", Cancel);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
