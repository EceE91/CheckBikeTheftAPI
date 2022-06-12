using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using CheckBikeTheftAPI.CheckBikeTheftAPI.Data.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CheckBikeTheftAPI.CheckBikeTheftAPI.Api.Controllers;

public class BaseRequest
{
    [Required]
    [DefaultValue(10)]
    public int Distance { get; set; } = 10;

    public string? StolenLocationAddress { get; set; }

    public Location? StolenLocationLatLon { get; set; }
    
    [Required]
    [EnumDataType(typeof(Stolenness))]
    [JsonConverter(typeof(StringEnumConverter))]
    [DefaultValue("proximity")]
    public Stolenness Stolenness { get; set; } = Stolenness.proximity;

    protected string ToLocationString()
    {
        // use lat lon as primary search param
        return (StolenLocationLatLon == null && StolenLocationAddress == null)
                           ? throw new LocationIsNotPresentException()
                           : StolenLocationLatLon != null
                               ? new StringBuilder().Append(StolenLocationLatLon.Latitude).Append(",").Append(StolenLocationLatLon.Longitude).ToString()
                               : StolenLocationAddress;
    }
}

public enum Stolenness
{
    //all,
    //non,
    stolen,
    proximity
}

public class Location
{
    public double Latitude { get;  set; }
    public double Longitude { get;  set; }
    
}