namespace CheckBikeTheftAPI.CheckBikeTheftAPI.Data.Exceptions;

public class LocationIsNotPresentException: Exception
{
    public LocationIsNotPresentException(String message)
        : base(message)
    {
    }
}