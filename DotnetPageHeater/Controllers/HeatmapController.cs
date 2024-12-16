using DotnetPageHeater.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotnetPageHeater.Controllers
{
    public record ClickData(string Url, double X, double Y, string ElementPath, int ViewportWidth, int ViewportHeight);

    public record EventData(string Url, string EventName, string ElementPath, int ViewportWidth, int ViewportHeight);

    [ApiController]
    [Route("api/[controller]")]
    public class HeatmapController(HeatmapService heatmapService) : ControllerBase
    {
        [HttpGet("data")]
        public IActionResult LoadHeatmap()
            => Ok(heatmapService.HeatmapData());

        [HttpGet("data/url")]
        public IActionResult LoadHeatmapByPage([FromQuery] string url = "/")
         => Ok(heatmapService.GetHeatmapData(url));

        [HttpGet("events")]
        public IActionResult LoadEvents()
         => Ok(heatmapService.EventsData());


        [HttpGet("events/url")]
        public IActionResult LoadEventsByPage([FromQuery] string url = "/")
         => Ok(heatmapService.GetEventData(url));

        [HttpPost("record")]
        public IActionResult RecordHeatmap([FromBody] ClickData clickData)
        {
            heatmapService.RecordHeatmap(clickData.Url, clickData.X, clickData.Y, clickData.ElementPath, clickData.ViewportWidth, clickData.ViewportHeight);
            return Ok();
        }

        [HttpPost("event")]
        public IActionResult RecordEvent([FromBody] EventData eventData)
        {
            heatmapService.RecordEvent(eventData.Url, eventData.EventName, eventData.ElementPath, eventData.ViewportWidth, eventData.ViewportHeight);
            return Ok();
        }
    }
}
