using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CheckBikeTheftAPI.CheckBikeTheftAPI.Api.Controllers;

public class BikeTheftSearchRequest: BaseRequest
{
    [Required]
    [DefaultValue(1)]
    public int Page { get; set; } = 1;

    [Required]
    [Range(1, 100)]
    [DefaultValue(25)]
    public int PerPage { get; set; } = 25;

    public Dictionary<string, string> ToBikeTheftParameters()
    {
        return new Dictionary<string, string>
               {
                   ["page"] = Page.ToString(),
                   ["per_page"] = PerPage.ToString(),
                   ["distance"] = Distance.ToString(),
                   ["stolenness"] = Stolenness.ToString(),
                   ["location"] = ToLocationString()
               };
    }
}