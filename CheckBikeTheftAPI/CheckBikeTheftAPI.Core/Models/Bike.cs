using Newtonsoft.Json;

namespace CheckBikeTheftAPI.CheckBikeTheftAPI.Core.Models;

public class Bike
{
    [JsonProperty("date_stolen")]
    public ulong? StolenDateTime  { get; set; }
    
    [JsonProperty("description")]
    public string Description { get; set; }
    
    [JsonProperty("frame_colors")]
    public List<string> FrameColors { get; set; }
    
    [JsonProperty("frame_model")]
    public string FrameModel { get; set; }
    
    [JsonProperty("id")]
    public ulong Id { get; set; }

    [JsonProperty("is_stock_img")]
    public bool IsStockImage { get; set; }
    
    [JsonProperty("large_img")]
    public string LargeImageUrl { get; set; }
    
    [JsonProperty("location_found")]
    public string LocationFound { get; set; }
    
    [JsonProperty("manufacturer_name")]
    public string ManufacturerName { get; set; }
    
    [JsonProperty("external_id")]
    public int? ExternalId { get; set; }
    
    [JsonProperty("registry_name")]
    public string RegistryName { get; set; }
    
    [JsonProperty("registry_url")]
    public string RegistryUrl { get; set; }
    
    [JsonProperty("serial")]
    public string Serial { get; set; }
    
    [JsonProperty("status")]
    public string Status { get; set; }
    
    [JsonProperty("stolen")]
    public bool Stolen { get; set; }
    
    [JsonProperty("stolen_coordinates")]
    public List<double> StolenCoordinates { get; set; }
    
    [JsonProperty("stolen_location")]
    public string StolenLocation { get; set; }
    
    [JsonProperty("thumb")]
    public string Thumb { get; set; }
    
    [JsonProperty("title")]
    public string Title { get; set; }
    
    [JsonProperty("url")]
    public string Url { get; set; }
    
    [JsonProperty("year")]
    public int? Year { get; set; }
}