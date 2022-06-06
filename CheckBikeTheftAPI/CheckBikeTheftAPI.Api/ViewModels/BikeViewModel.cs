namespace CheckBikeTheftAPI.CheckBikeTheftAPI.Api.ViewModels;

public class BikeViewModel
{
    public DateTime? StolenDateTime  { get; set; }
    public string Description { get; set; }
    public List<string> FrameColors { get; set; }
    public string FrameModel { get; set; }
    public ulong Id { get; set; }
    public bool IsStockImage { get; set; }
    public string LargeImageUrl { get; set; }
    public string LocationFound { get; set; }
    public string ManufacturerName { get; set; }
    public int? ExternalId { get; set; }
    public string RegistryName { get; set; }
    public string RegistryUrl { get; set; }
    public string Serial { get; set; }
    public string Status { get; set; }
    public bool Stolen { get; set; }
    public List<double> StolenCoordinates { get; set; }
    public string? StolenLocation { get; set; }
    public string Thumb { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
    public int? Year { get; set; }
}