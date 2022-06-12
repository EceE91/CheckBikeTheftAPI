namespace CheckBikeTheftAPI.CheckBikeTheftAPI.Data.Exceptions;

public class LocationIsNotPresentException: Exception
{
    public LocationIsNotPresentException()
        : base("Either StolenLocationLatLon or StolenLocationAddress must be entered")
    {
    }
}