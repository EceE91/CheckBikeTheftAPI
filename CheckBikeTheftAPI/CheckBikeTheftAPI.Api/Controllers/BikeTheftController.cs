using CheckBikeTheftAPI.CheckBikeTheftAPI.Api.Helpers;
using CheckBikeTheftAPI.CheckBikeTheftAPI.Api.ViewModels;
using CheckBikeTheftAPI.CheckBikeTheftAPI.Core.Interfaces;
using CheckBikeTheftAPI.CheckBikeTheftAPI.Core.Models;
using CheckBikeTheftAPI.CheckBikeTheftAPI.Data.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CheckBikeTheftAPI.CheckBikeTheftAPI.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BikeTheftController: ControllerBase
{
    private readonly ILogger<BikeTheftController> _logger;
    private readonly IStolenBikeRepository _stolenBikeRepository;

    public BikeTheftController(ILogger<BikeTheftController> logger, IStolenBikeRepository stolenBikeRepository)
    {
        _logger = logger;
        _stolenBikeRepository = stolenBikeRepository;
    }

    [ApiVersion("1.0")]
    [HttpGet("/SearchStolenBikes")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(StolenBikeViewModel))]
    [SwaggerResponse(StatusCodes.Status404NotFound, type: typeof(ErrorViewModel))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, type: typeof(ErrorViewModel))]
    public async Task<ActionResult<StolenBikeViewModel>> SearchStolenBikes([FromQuery] BikeTheftSearchRequest bikeTheftSearchRequest)
    {
        try
        {
            var queryParams = bikeTheftSearchRequest.ToBikeTheftParameters();
            var bikeThefts = await _stolenBikeRepository.GetStolenBikes(queryParams);

            _logger.LogInformation($"{bikeThefts.Bikes.Count} bike thefts returned for {queryParams["location"]}");

            return Ok(
                new ActionResult<StolenBikeViewModel>(
                    new StolenBikeViewModel
                    {
                        Bikes = MapToBikeViewModel(bikeThefts.Bikes)
                    }
                )
            );
        }
        catch (LocationIsNotPresentException e)
        {
            _logger.LogError(e.Message);
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    private List<BikeViewModel> MapToBikeViewModel(List<Bike> bikes)
    {
        var bikeViewModel = new List<BikeViewModel>();
        foreach (var bike in bikes)
        {
            bikeViewModel.Add(
                new BikeViewModel
                {
                    StolenDateTime = bike.StolenDateTime?.ToUtcDateTime(),
                    Description = bike.Description,
                    ExternalId = bike.ExternalId,
                    FrameColors = bike.FrameColors,
                    FrameModel = bike.FrameModel,
                    Id = bike.Id,
                    IsStockImage = bike.IsStockImage,
                    LargeImageUrl = bike.LargeImageUrl,
                    LocationFound = bike.LocationFound,
                    ManufacturerName = bike.ManufacturerName,
                    RegistryName = bike.RegistryName,
                    RegistryUrl = bike.RegistryUrl,
                    Serial = bike.Serial,
                    Status = bike.Status,
                    Stolen = bike.Stolen,
                    StolenCoordinates = bike.StolenCoordinates,
                    StolenLocation = bike.StolenLocation,
                    Thumb = bike.Thumb,
                    Title = bike.Title,
                    Url = bike.Url,
                    Year = bike.Year
                }
            );
        }

        return bikeViewModel;
    }

    [ApiVersion("1.0")]
    [HttpGet("/CountOfStolenBikes")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(StolenBikeCountViewModel))]
    [SwaggerResponse(StatusCodes.Status404NotFound, type: typeof(ErrorViewModel))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, type: typeof(ErrorViewModel))]
    public async Task<ActionResult<StolenBikeCountViewModel>> CountOfStolenBikes([FromQuery] BikeTheftCountRequest bikeTheftCountRequest)
    {
        try
        {
            var queryParams = bikeTheftCountRequest.ToBikeTheftCountParameters();
            var bikeTheftCount = await _stolenBikeRepository.GetCountOfStolenBikes(queryParams);

            _logger.LogInformation($"{bikeTheftCount.Stolen} bike thefts returned for {queryParams["location"]}");

            return Ok(
                new ActionResult<StolenBikeCountViewModel>(
                    new StolenBikeCountViewModel
                    {
                        Proximity = bikeTheftCount.Proximity,
                        Non = bikeTheftCount.Non,
                        Stolen = bikeTheftCount.Stolen
                    }
                )
            );
        }
        catch (LocationIsNotPresentException e)
        {
            _logger.LogError(e.Message);
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
}