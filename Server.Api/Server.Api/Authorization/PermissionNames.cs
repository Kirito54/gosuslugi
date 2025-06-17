namespace GovServices.Server.Authorization;

public static class PermissionNames
{
    public const string AccessRDZ = "Доступ к РДЗ";
    public const string AccessRDI = "Доступ к РДИ";
    public const string RequestEGRN = "Запросы в ЕГРН";
    public const string RequestVIS = "Запросы в ВИС";
    public const string RequestZAGS = "Запросы в ЗАГС";
    public const string UploadDocuments = "Доступ к загрузке документов";
    public const string SignDocuments = "Доступ к подписанию документов";
    public const string ViewSpecificServices = "Доступ только к определённым госуслугам";
    public const string ViewClosedServices = "Доступ только к закрытым услугам";
}
