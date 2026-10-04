namespace HarbourApi.Harbours;

/// <summary>The harbours and their reservations. The data is held in memory and seeded at start-up.</summary>
public sealed class HarbourDirectory
{
    private readonly Dictionary<string, Harbour> _harbours = new(StringComparer.OrdinalIgnoreCase)
    {
        ["BGO"] = new("BGO", "Vågen guest harbour", "Bergen",
        [
            new("BGO-A1", 10m, true),
            new("BGO-A2", 10m, true),
            new("BGO-A3", 12m, true),
            new("BGO-A4", 12m, false),
            new("BGO-A5", 15m, true),
            new("BGO-A6", 18m, true),
        ]),
        ["SVG"] = new("SVG", "Vågen marina", "Stavanger",
        [
            new("SVG-B1", 9m, false),
            new("SVG-B2", 11m, true),
            new("SVG-B3", 14m, true),
            new("SVG-B4", 20m, true),
        ]),
        // The Brosundet pontoons are being rebuilt; the harbour is listed with its berths to come.
        ["AES"] = new("AES", "Brosundet guest harbour", "Ålesund", []),
    };

    private readonly List<Reservation> _reservations =
    [
        new("BGO-A1", new DateOnly(2026, 7, 1), new DateOnly(2026, 7, 4)),
        new("BGO-A3", new DateOnly(2026, 7, 2), new DateOnly(2026, 7, 3)),
        new("BGO-A6", new DateOnly(2026, 6, 28), new DateOnly(2026, 7, 6)),
        new("SVG-B2", new DateOnly(2026, 7, 1), new DateOnly(2026, 7, 2)),
    ];

    public IEnumerable<Harbour> All => _harbours.Values;

    public Harbour? Find(string code) => _harbours.GetValueOrDefault(code);

    public IEnumerable<Reservation> ReservationsFor(Berth berth) => _reservations.Where(reservation => reservation.BerthId == berth.Id);
}
