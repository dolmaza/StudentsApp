namespace StudentsApp.Reusables
{
    public struct ControllerActionRouteNames
    {
        public struct Home
        {
            public const string Dashboard = "Dashboard";
        }

        public struct Shared
        {
            public const string NotFound = "NotFound";
        }

        public struct Student
        {
            public const string Students = "Students";
            public const string StudentsGetFiltered = "StudentsGetFiltered";
            public const string StudentsCreate = "StudentsCreate";
            public const string StudentsUpdate = "StudentsUpdate";
            public const string StudentsDelete = "StudentsDelete";
        }
    }
}