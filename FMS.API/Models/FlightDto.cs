using FMS.API.Entities;

namespace FMS.API.Models;

public class FlightDto
{
    public Guid Id { get; set; }
    public int NumerLotu { get; set; }
    public DateTimeOffset DataWylotu { get; set; }
    public string MiejsceWylotu { get; set; }
    public string MiejscePrzylotu { get; set; }
    public PlaneType TypSamolotu { get; set; }
}
