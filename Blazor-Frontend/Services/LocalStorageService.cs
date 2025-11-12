using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace MeetingRoomNano.Client.Services
{
    public class LocalStorageService
    {
        private readonly IJSRuntime _js;

        public LocalStorageService(IJSRuntime js)
        {
            _js = js;
        }

        public ValueTask SetItemAsync(string key, string value)
        {
            return _js.InvokeVoidAsync("localStorage.setItem", key, value);
        }

        public ValueTask<string> GetItemAsync(string key)
        {
            return _js.InvokeAsync<string>("localStorage.getItem", key);
        }

        public ValueTask RemoveItemAsync(string key)
        {
            return _js.InvokeVoidAsync("localStorage.removeItem", key);
        }
    }
}
