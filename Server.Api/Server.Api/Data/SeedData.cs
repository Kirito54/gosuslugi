namespace GovServices.Server.Data;

public static class SeedData
{
    public static class Departments
    {
        public const int IT = 1;
        public const int Legal = 2;
        public const int HR = 3;
    }

    public static class Roles
    {
        public const string Specialist = "Специалист";
        public const string DepartmentHead = "Начальник отдела";
        public const string ManagementHead = "Начальник управления";
    }

    public static class Permissions
    {
        public const int AccessApplications = 1;
        public const int EditContracts = 2;
    }

    public const int DefaultServiceId = 1;
}
