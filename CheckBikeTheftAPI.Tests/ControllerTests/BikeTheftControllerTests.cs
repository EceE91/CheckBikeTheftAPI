using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using CheckBikeTheftAPI.CheckBikeTheftAPI.Api.Controllers;
using CheckBikeTheftAPI.CheckBikeTheftAPI.Api.Middleware;
using CheckBikeTheftAPI.CheckBikeTheftAPI.Api.ViewModels;
using CheckBikeTheftAPI.CheckBikeTheftAPI.Core.Interfaces;
using CheckBikeTheftAPI.CheckBikeTheftAPI.Core.Models;
using CheckBikeTheftAPI.CheckBikeTheftAPI.Data.Exceptions;
using CheckBikeTheftAPI.Tests.Helpers;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CheckBikeTheftAPI.Tests.ControllerTests;

public class BikeTheftControllerTests
{
    private static readonly Fixture _fixture = new();
    private readonly Mock<ILogger<BikeTheftController>> _mockLogger = new();
    private readonly Mock<IStolenBikeRepository> _mockRepository = new();
    private readonly StolenBike _stolenBike = _fixture.Create<StolenBike>();
    private readonly StolenBikeCount _stolenBikeCount = _fixture.Create<StolenBikeCount>();

    public BikeTheftControllerTests()
    {
        // Arrange
        _mockRepository.Setup(x => x.GetStolenBikes(It.IsAny<Dictionary<string, string>>()))
                       .Returns(Task.FromResult(_stolenBike));

        _mockRepository.Setup(x => x.GetCountOfStolenBikes(It.IsAny<Dictionary<string, string>>()))
                       .Returns(Task.FromResult(_stolenBikeCount));
    }

    private BikeTheftController CreateSut() => new(_mockLogger.Object, _mockRepository.Object);

    [Theory]
    [ClassData(typeof(LocationTestData))]
    public async void SearchStolenBikes_ReturnsStolenBikesInSpecifiedLocation(string? stolenLocationAddress, Location? stolenLocationLatLon)
    {
        // Act
        var result = await CreateSut().SearchStolenBikes(PrepareTestRequestForStolenBikeSearch(stolenLocationAddress, stolenLocationLatLon));

        // assert
        using (new AssertionScope())
        {
            result.Result.Should().BeOfType<OkObjectResult>();
            var resultValue = ((OkObjectResult)result.Result!).Value;
            resultValue.Should().BeOfType<ActionResult<StolenBikeViewModel>>();
            ((ActionResult<StolenBikeViewModel>)resultValue!).Value?.Bikes.Should().NotBeEmpty();
            ((ActionResult<StolenBikeViewModel>)resultValue!).Value?.Bikes.Count.Should().Be(_stolenBike.Bikes.Count);
        }
    }

    [Fact]
    public async void SearchStolenBikes_WhenLocationInfoNotProvided_ReturnsNotFound()
    {
        try
        {
            // Act
            await CreateSut().SearchStolenBikes(PrepareTestRequestForStolenBikeSearch(stolenLocationAddress: null, stolenLocationLatLon: null));
        }
        catch (Exception e)
        {
            // assert
            using (new AssertionScope())
            {
                e.Should().BeOfType<LocationIsNotPresentException>();
                e.Message.Should().Be("Either StolenLocationLatLon or StolenLocationAddress must be entered");
            }
        }
    }

    [Theory]
    [ClassData(typeof(LocationTestData))]
    public async void CountOfStolenBikes_ReturnsStolenBikesCountInSpecifiedLocation(string? stolenLocationAddress, Location? stolenLocationLatLon)
    {
        // Act
        var result = await CreateSut().CountOfStolenBikes(PrepareTestRequestForStolenBikeCount(stolenLocationAddress, stolenLocationLatLon));

        // assert
        using (new AssertionScope())
        {
            result.Result.Should().BeOfType<OkObjectResult>();
            var resultValue = ((OkObjectResult)result.Result!).Value;
            resultValue.Should().BeOfType<ActionResult<StolenBikeCountViewModel>>();
            ((ActionResult<StolenBikeCountViewModel>)resultValue!).Value?.Stolen.Should().Be(_stolenBikeCount.Stolen);
            ((ActionResult<StolenBikeCountViewModel>)resultValue).Value?.Proximity.Should().Be(_stolenBikeCount.Proximity);
            ((ActionResult<StolenBikeCountViewModel>)resultValue).Value?.Non.Should().Be(_stolenBikeCount.Non);
        }
    }

    [Fact]
    public async Task ExceptionHandlingMiddleware_Returns500StatusCode()
    {
        //arrange
        var expectedException = new ArgumentNullException();
        Task mockNextMiddleware(HttpContext _) => Task.FromException(expectedException);

        var logger = Mock.Of<ILogger<ExceptionHandlingMiddleware>>();
        var httpContext = new DefaultHttpContext();

        var exceptionHandlingMiddleware = new ExceptionHandlingMiddleware(mockNextMiddleware, logger);

        //act
        await exceptionHandlingMiddleware.InvokeAsync(httpContext);

        //assert
        httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
    
    [Fact]
    public async Task ExceptionHandlingMiddleware_WhenLocationIsNotPresentExceptionThrown_Returns404()
    {
        //arrange
        var expectedException = new LocationIsNotPresentException();
        Task mockNextMiddleware(HttpContext _) => Task.FromException(expectedException);

        var logger = Mock.Of<ILogger<ExceptionHandlingMiddleware>>();
        var httpContext = new DefaultHttpContext();

        var exceptionHandlingMiddleware = new ExceptionHandlingMiddleware(mockNextMiddleware, logger);

        //act
        await exceptionHandlingMiddleware.InvokeAsync(httpContext);

        //assert
        httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task ExceptionHandlingMiddleware_WhenLocationIsNotPresentExceptionThrown_WritesExceptionResponseJsonToBody()
    {
        //arrange
        var expectedContent = "{\"Success\":false,\"Message\":\"Either StolenLocationLatLon or StolenLocationAddress must be entered\"}";
        Task mockNextMiddleware(HttpContext _) => Task.FromException(new LocationIsNotPresentException());

        var httpContext = new DefaultHttpContext
                          {
                              Response =
                              {
                                  Body = new MemoryStream()
                              }
                          };

        var logger = Mock.Of<ILogger<ExceptionHandlingMiddleware>>();
      

        var exceptionHandlingMiddleware = new ExceptionHandlingMiddleware(mockNextMiddleware, logger);

        //act
        await exceptionHandlingMiddleware.InvokeAsync(httpContext);

        httpContext.Response.Body.Position = 0;
        string bodyContent;
        using (var sr = new StreamReader(httpContext.Response.Body))
            bodyContent = await sr.ReadToEndAsync();
        
        expectedContent.Should().BeEquivalentTo(bodyContent);
    }
    
    [Fact]
    public async Task ExceptionHandlingMiddleware_WritesExceptionResponseJsonToBody()
    {
        //arrange
        var expectedContent = "{\"Success\":false,\"Message\":\"Internal Server errors. Check Logs!\"}";
        Task mockNextMiddleware(HttpContext _) => Task.FromException(new JsonException());

        var httpContext = new DefaultHttpContext
                          {
                              Response =
                              {
                                  Body = new MemoryStream()
                              }
                          };

        var logger = Mock.Of<ILogger<ExceptionHandlingMiddleware>>();
      

        var exceptionHandlingMiddleware = new ExceptionHandlingMiddleware(mockNextMiddleware, logger);

        //act
        await exceptionHandlingMiddleware.InvokeAsync(httpContext);

        httpContext.Response.Body.Position = 0;
        string bodyContent;
        using (var sr = new StreamReader(httpContext.Response.Body))
            bodyContent = await sr.ReadToEndAsync();
        
        expectedContent.Should().BeEquivalentTo( bodyContent);
    }

    [Fact]
    public async void CountOfStolenBikes_WhenLocationInfoNotProvided_ReturnsNotFound()
    {
        try
        {
            // Act
            await CreateSut().CountOfStolenBikes(PrepareTestRequestForStolenBikeCount(stolenLocationAddress: null, stolenLocationLatLon: null));
        }
        catch (Exception e)
        {
            using (new AssertionScope())
            {
                e.Message.Should().Be("Either StolenLocationLatLon or StolenLocationAddress must be entered");
                e.Should().BeOfType<LocationIsNotPresentException>();
            }
        }
    }

    private BikeTheftCountRequest PrepareTestRequestForStolenBikeCount(string? stolenLocationAddress, Location? stolenLocationLatLon)
    {
        return new BikeTheftCountRequest
               {
                   StolenLocationAddress = stolenLocationAddress,
                   StolenLocationLatLon = stolenLocationLatLon,
                   Distance = 10,
                   Stolenness = Stolenness.proximity
               };
    }

    private BikeTheftSearchRequest PrepareTestRequestForStolenBikeSearch(string? stolenLocationAddress, Location? stolenLocationLatLon)
    {
        return new BikeTheftSearchRequest
               {
                   Page = 1,
                   PerPage = 25,
                   StolenLocationAddress = stolenLocationAddress,
                   StolenLocationLatLon = stolenLocationLatLon,
                   Distance = 10,
                   Stolenness = Stolenness.proximity
               };
    }
}