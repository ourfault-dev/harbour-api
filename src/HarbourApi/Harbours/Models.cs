namespace HarbourApi.Harbours;

/// <summary>A guest harbour and its berths.</summary>
public sealed record Harbour(string Code, string Name, string Town, IReadOnlyList<Berth> Berths);

/// <summary>A berth and the longest boat it takes, in metres.</summary>
public sealed record Berth(string Id, decimal MaxLength, bool ShorePower);

/// <summary>A boat's stay on a berth, from the arrival day up to but not including the departure day.</summary>
public sealed record Reservation(string BerthId, DateOnly Arrival, DateOnly Departure)
{
    public bool Covers(DateOnly night) => night >= Arrival && night < Departure;

    public bool Overlaps(DateOnly arrival, DateOnly departure) => arrival < Departure && Arrival < departure;
}

/// <summary>How full a harbour is on a night.</summary>
public sealed record Occupancy(string Harbour, DateOnly Date, int Occupied, int Berths, int Percent);

/// <summary>A harbour as the list shows it.</summary>
public sealed record HarbourSummary(string Code, string Name, string Town, int Berths);
