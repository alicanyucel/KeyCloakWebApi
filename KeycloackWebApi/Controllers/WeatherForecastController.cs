using KeycloackWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace KeycloackWebApi.Controllers;

[ApiController] 
[Route("api/[controller]/[action]")] 
[Authorize] 
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")] 
    public IActionResult Get()
    {
        var currentUser = HttpContext.User;

     
        var realmAccessRoles = currentUser.FindFirst("realm_access")?.Value;

       
        var roles = realmAccessRoles != null ? GetRolesFromRealmAccess(realmAccessRoles) : Enumerable.Empty<string>();

      
        var isAdmin = roles.Contains("Admin");

        var rng = new Random();
        var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            TemperatureC = rng.Next(-20, 55),
            Summary = Summaries[rng.Next(Summaries.Length)]
        }).ToArray();

      
        if (isAdmin)
        {
            return Ok(forecasts); 
        }
        else
        {
            return Unauthorized(); 
        }
    }
    public IEnumerable<string> GetRolesFromRealmAccess(string realmAccessRoles)
    {
       
        var json = JObject.Parse(realmAccessRoles);
        var rolesArray = json["roles"] as JArray;
        if (rolesArray != null)
        {
            return rolesArray.Select(r => r.ToString());
        }
        return Enumerable.Empty<string>();
    }
}
