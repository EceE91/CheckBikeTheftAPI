using System.Net.Http.Headers;
using CheckBikeTheftAPI.CheckBikeTheftAPI.Core.Interfaces;
using CheckBikeTheftAPI.CheckBikeTheftAPI.Core.Models;
using CheckBikeTheftAPI.CheckBikeTheftAPI.Data.Exceptions;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace CheckBikeTheftAPI.CheckBikeTheftAPI.Data.Repositories;

public class StolenBikeRepository: IStolenBikeRepository
{
    private static string _baseUrl = "https://bikeindex.org/api/v3/search";
    private readonly IHttpClientFactory _httpClientFactory;

    public StolenBikeRepository(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<StolenBike> GetStolenBikes(Dictionary<string,string> bikeTheftSearchParameters)
    {
        StolenBike stolenBike;
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        
        var uri = QueryHelpers.AddQueryString(_baseUrl, bikeTheftSearchParameters);
        
        using var response = await httpClient.GetAsync(new Uri(uri, UriKind.Absolute), HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();
        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            stolenBike = JsonConvert.DeserializeObject<StolenBike>(jsonString) ?? throw new JsonSerializationException();
        }else
            throw new HttpErrorStatusCodeException(response.StatusCode);

        return stolenBike;
    }
    
    public async Task<StolenBikeCount> GetCountOfStolenBikes(Dictionary<string,string> bikeTheftCountParameters)
    {
        StolenBikeCount stolenBikeCount;
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        
        var uri = QueryHelpers.AddQueryString(_baseUrl+"/count", bikeTheftCountParameters);
        
        using var response = await httpClient.GetAsync(new Uri(uri, UriKind.Absolute), HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();
        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            stolenBikeCount = JsonConvert.DeserializeObject<StolenBikeCount>(jsonString) ?? throw new JsonSerializationException();
        }else
            throw new HttpErrorStatusCodeException(response.StatusCode);

        return stolenBikeCount;
    }
}