using Newtonsoft.Json;

namespace CheckBikeTheftAPI.CheckBikeTheftAPI.Core.Models;

[Serializable]
public class StolenBikeCount
{
    [JsonProperty("proximity")]
    public int Proximity { get; set; }
    
    [JsonProperty("stolen")]
    public int Stolen { get; set; }
    
    [JsonProperty("non")]
    public int Non { get; set; }
}