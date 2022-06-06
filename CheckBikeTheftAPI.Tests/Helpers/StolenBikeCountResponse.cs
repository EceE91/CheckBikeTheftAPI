using CheckBikeTheftAPI.CheckBikeTheftAPI.Api.ViewModels;

namespace CheckBikeTheftAPI.Tests.Helpers;

public class StolenBikeCountResponse
{
    public string Result { get; set; }
    public StolenBikeCountViewModel Value { get; set; }
}