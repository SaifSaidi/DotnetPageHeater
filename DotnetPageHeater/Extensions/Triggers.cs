using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DotnetPageHeater.Extensions
{
    public static class Triggers
    {
        public static async Task TriggerEvent(this ElementReference elementRef,
   string eventName, IJSRuntime js)
        {
            await js.InvokeVoidAsync("recordEvent", eventName, elementRef);
             
        }
        public static async Task TriggerHeatMapEvent(this ElementReference elementRef, IJSRuntime js)
        {
            await js.InvokeVoidAsync("recordHeatmap", elementRef);

        }
    }
}
