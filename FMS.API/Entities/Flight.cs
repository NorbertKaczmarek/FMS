using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FMS.API.Entities;

public enum PlaneType
{
    Embraer,
    Boeing,
    Airbus
}
public class Flight
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public int NumerLotu { get; set; }

    [Required]
    public DateTimeOffset DataWylotu { get; set; }

    [Required]
    public string MiejsceWylotu { get; set; }

    [Required]
    public string MiejscePrzylotu { get; set; }

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PlaneType TypSamolotu { get; set; }

}
