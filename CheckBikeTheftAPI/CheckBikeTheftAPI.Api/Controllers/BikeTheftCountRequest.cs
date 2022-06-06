using System.Text;

namespace CheckBikeTheftAPI.CheckBikeTheftAPI.Api.Controllers;

public class BikeTheftCountRequest : BaseRequest
{
    public Dictionary<string, string> ToBikeTheftCountParameters()
    {
        return new Dictionary<string, string>
               {
                   ["distance"] = Distance.ToString(),
                   ["stolenness"] = Stolenness.ToString(),
                   ["location"] = ToLocationString()
               };
    }
}