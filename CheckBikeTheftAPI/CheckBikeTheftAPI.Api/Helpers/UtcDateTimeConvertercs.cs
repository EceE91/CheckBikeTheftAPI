namespace CheckBikeTheftAPI.CheckBikeTheftAPI.Api.Helpers;

public static class UtcDateTimeConverter
{
    public static DateTime ToUtcDateTime(this ulong stolenDateTime)
    {
        return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds((long)stolenDateTime);
    }   
}