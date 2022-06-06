using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CheckBikeTheftAPI.Tests.Helpers;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace CheckBikeTheftAPI.Tests.IntegrationTests;

public class BikeTheftControllerIntegrationTests: IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public BikeTheftControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public async Task CountOfStolenBikes_WhenRequestIsCorrect_ReturnsOk()
    {
        // Act
        var response = await _factory.CreateClient().GetAsync("/CountOfStolenBikes?Distance=10&StolenLocationAddress=amsterdam&Stolenness=proximity");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        
        // Deserializing json data to object
        var deserializedObject = JsonConvert.DeserializeObject<StolenBikeCountResponse>(responseString);

        //assert
        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            deserializedObject.Value.Non.Should().BeGreaterThan(0);
            deserializedObject.Value.Proximity.Should().BeGreaterThan(0);
            deserializedObject.Value.Stolen.Should().BeGreaterThan(0);
        }
    }
    
    [Fact]
    public async Task CountOfStolenBikes_WhenRequestContainsLatLon_ReturnsOk()
    {
        // Act
        var response = await _factory.CreateClient().GetAsync("/CountOfStolenBikes?Distance=10&StolenLocationLatLon.Latitude=45.521728&StolenLocationLatLon.Longitude=-122.67326&Stolenness=proximity");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        
        // Deserializing json data to object
        var deserializedObject = JsonConvert.DeserializeObject<StolenBikeCountResponse>(responseString);

        //assert
        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            deserializedObject.Value.Non.Should().BeGreaterThan(0);
            deserializedObject.Value.Proximity.Should().BeGreaterThan(0);
            deserializedObject.Value.Stolen.Should().BeGreaterThan(0);
        }
    }
    
    [Fact]
    public async Task CountOfStolenBikes_WhenLocationIsNull_ReturnsNotFound()
    {
        // Act
        var response = await _factory.CreateClient().GetAsync("/countOfStolenBikes?Distance=10&Stolenness=proximity");

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task SearchStolenBikes_WhenLocationIsNull_ReturnsNotFound()
    {
        // Act
        var response = await _factory.CreateClient().GetAsync("/searchStolenBikes?Page=1&PerPage=25&Distance=10&Stolenness=proximity");

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task SearchStolenBikes_WhenCityProvidedAsLocation_ReturnsOkWithStolenLocationContainsCity()
    {
        // Act
        var stolenLocationAddress = "amsterdam";
        var response = await _factory.CreateClient().GetAsync($"/searchStolenBikes?Page=1&PerPage=25&Distance=10&StolenLocationAddress={stolenLocationAddress}&Stolenness=proximity");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        
        // Deserializing json data to object
        var deserializedObject = JsonConvert.DeserializeObject<StolenBikeSearchResponse>(responseString);

        //assert
        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            deserializedObject.Value.Bikes.Count.Should().BeGreaterThan(0);
            deserializedObject.Value.Bikes.Where(x=>x.StolenLocation != null)
                              .Select(x=> x.StolenLocation?.ToLower())
                              .Should().Match(p=>p.Contains("nl") || p.Contains(stolenLocationAddress));
        }
    }
}