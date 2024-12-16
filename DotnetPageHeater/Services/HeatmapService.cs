using DotnetPageHeater.Models;
using System.Collections.Concurrent;

namespace DotnetPageHeater.Services
{
    public interface IHeatmapService
    {

        ConcurrentDictionary<string, ConcurrentDictionary<string, HeatMapData>> HeatmapData();
        ConcurrentDictionary<string, ConcurrentDictionary<string, EventData>> EventsData();
        Dictionary<string, EventData> GetEventData(string pageUrl);
        Dictionary<string, HeatMapData> GetHeatmapData(string pageUrl);
        void RecordHeatmap(string pageUrl, double x, double y, string elementPath, int viewportWidth, int viewportHeight);
        void RecordEvent(string pageUrl, string eventName, string elementPath, int viewportWidth, int viewportHeight);
    }
    public class HeatmapService: IHeatmapService
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, HeatMapData>> _heatmapData = new();
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, EventData>> _eventData = new();

        public ConcurrentDictionary<string, ConcurrentDictionary<string, HeatMapData>>
            HeatmapData() => _heatmapData;

        public ConcurrentDictionary<string, ConcurrentDictionary<string, EventData>>
            EventsData() => _eventData;


        public void RecordHeatmap(string pageUrl, double x, double y, string elementPath, int viewportWidth, int viewportHeight)
        {
            var key = $"{x:F2},{y:F2}";
            _heatmapData.GetOrAdd(pageUrl, _ => new ConcurrentDictionary<string, HeatMapData>())
                        .AddOrUpdate(key,
                            new HeatMapData { Url = pageUrl, Count = 1, ElementPath = elementPath, ViewportWidth = viewportWidth, ViewportHeight = viewportHeight, X = x, Y = y },
                            (_, oldValue) =>
                            {
                                oldValue.Count++;
                                return oldValue;
                            });
        }

        public void RecordEvent(string pageUrl, string eventName, string elementPath, int viewportWidth, int viewportHeight)
        {
            _eventData.GetOrAdd(pageUrl, _ => new ConcurrentDictionary<string, EventData>())
                      .AddOrUpdate(eventName,
                          new EventData { Url = pageUrl, Count = 1, ElementPath = elementPath, ViewportWidth = viewportWidth, ViewportHeight = viewportHeight },
                          (_, oldValue) =>
                          {
                              oldValue.Count++;
                              return oldValue;
                          });
        }

        public Dictionary<string, HeatMapData> GetHeatmapData(string pageUrl)
        {
            return _heatmapData.TryGetValue(pageUrl, out var data)
                ? new Dictionary<string, HeatMapData>(data)
                : new Dictionary<string, HeatMapData>();
        }

        public Dictionary<string, EventData> GetEventData(string pageUrl)
        {
            return _eventData.TryGetValue(pageUrl, out var data)
                ? new Dictionary<string, EventData>(data)
                : new Dictionary<string, EventData>();
        }
    }

    
}
