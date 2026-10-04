namespace HarbourApi.Harbours;

/// <summary>Which berths are free, and how full a harbour is.</summary>
public sealed class AvailabilityService(HarbourDirectory directory)
{
    /// <summary>The berths that take a boat of <paramref name="boatLength"/> metres for every night of the stay.</summary>
    public IReadOnlyList<Berth> Available(Harbour harbour, DateOnly arrival, int nights, decimal boatLength)
    {
        var departure = arrival.AddDays(nights);
        return
        [
            .. harbour.Berths
                .Where(berth => berth.MaxLength >= boatLength)
                .Where(berth => !directory.ReservationsFor(berth).Any(reservation => reservation.Overlaps(arrival, departure)))
                .OrderBy(berth => berth.MaxLength),
        ];
    }

    /// <summary>The share of the harbour's berths taken on the night of <paramref name="date"/>, in whole percent.</summary>
    public Occupancy OccupancyOn(Harbour harbour, DateOnly date)
    {
        var occupied = harbour.Berths.Count(berth => directory.ReservationsFor(berth).Any(reservation => reservation.Covers(date)));
        var percent = occupied * 100 / harbour.Berths.Count;
        return new Occupancy(harbour.Code, date, occupied, harbour.Berths.Count, percent);
    }
}
