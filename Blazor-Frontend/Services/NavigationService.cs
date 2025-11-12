using Microsoft.AspNetCore.Components;

namespace MeetingRoomNano.Client.Services
{
    public class NavigationService
    {
        private readonly NavigationManager _navigation;

        public NavigationService(NavigationManager navigation)
        {
            _navigation = navigation;
        }

        public void NavigateTo(string url, bool forceReload = false)
        {
            _navigation.NavigateTo(url, forceReload);
        }
    }
}
