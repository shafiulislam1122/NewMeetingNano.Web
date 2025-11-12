namespace MeetingRoomNano.Client.Helpers
{
    public static class ApiEndpoints
    {
        public const string BaseUrl = "https://localhost:5001/api";

        // Account
        public const string AccountRegister = BaseUrl + "/Account/Register";
        public const string AccountLogin = BaseUrl + "/Account/Login";

        // Auth
        public const string AdminPanel = BaseUrl + "/Auth/Admin";
        public const string EmployeePanel = BaseUrl + "/Auth/Employee";

        // Employee actions
        public const string CreateMeeting = BaseUrl + "/Employee/Create-Meeting";
        public const string UpdateProfile = BaseUrl + "/Employee/Update-Profile";
        public const string DeleteProfile = BaseUrl + "/Employee/Delete-Profile";
        public const string EmployeeMeetingRoomList = BaseUrl + "/MeetingRoomList";

        // Admin actions
        public const string AdminMeetingRoomList = BaseUrl + "/MeetingRoomList";
        public const string CreateMeetingRoom = BaseUrl + "/Admin/Create-MeetingRoom";
        public const string MeetingRoomDetails = BaseUrl + "/MeetingRoom/{id}";
        public const string DeleteMeetingRoom = BaseUrl + "/MeetingRoom/{id}";
    }
}
