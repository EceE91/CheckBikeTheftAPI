using System.Collections;
using System.Collections.Generic;
using CheckBikeTheftAPI.CheckBikeTheftAPI.Api.Controllers;

namespace CheckBikeTheftAPI.Tests.Helpers;

public class LocationTestData: IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "Amsterdam, The Netherlands", null };
        yield return new object[] { "Milan, Italy", new Location {Latitude = 45.521728, Longitude = -122.67326} };
        yield return new object[] { "210 NW 11th Ave, Portland, OR", null };
        yield return new object[] { null, new Location {Latitude = 45.521728, Longitude = -122.67326} };
        yield return new object[] { "60647", null };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}