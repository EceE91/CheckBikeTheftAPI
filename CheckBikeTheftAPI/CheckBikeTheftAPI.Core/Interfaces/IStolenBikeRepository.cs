using CheckBikeTheftAPI.CheckBikeTheftAPI.Core.Models;

namespace CheckBikeTheftAPI.CheckBikeTheftAPI.Core.Interfaces;

public interface IStolenBikeRepository
{
    /// <summary>
    /// Fetches stolen bikes of a given location
    /// </summary>
    /// <returns></returns>
    Task<StolenBike> GetStolenBikes(Dictionary<string, string?> bikeTheftSearchParameters);
    
    /// <summary>
    /// Fetches count of stolen bikes of a given location
    /// </summary>
    /// <returns></returns>
    Task<StolenBikeCount> GetCountOfStolenBikes(Dictionary<string, string?> bikeTheftCountParameters);
}