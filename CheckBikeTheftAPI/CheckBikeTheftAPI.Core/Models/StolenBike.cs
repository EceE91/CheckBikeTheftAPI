using Newtonsoft.Json;

namespace CheckBikeTheftAPI.CheckBikeTheftAPI.Core.Models;

[Serializable]
public class StolenBike
{
    [JsonProperty("bikes")]
    public List<Bike> Bikes { get; set; } = new();
}