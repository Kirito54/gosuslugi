using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Client.Wasm.Services;

public class MenuService
{
    public List<MenuGroup> Groups { get; } = new();

    public MenuService()
    {
        BuildMenu();
    }

    private void BuildMenu()
    {
        var pages = typeof(Program).Assembly
            .GetTypes()
            .Where(t => typeof(ComponentBase).IsAssignableFrom(t))
            .SelectMany(t => t.GetCustomAttributes<RouteAttribute>(), (t, attr) => (Route: attr.Template, Type: t))
            .Distinct()
            .ToList();

        Console.WriteLine($"Found pages: {string.Join(", ", pages.Select(p => p.Route))}");

        var groupStatements = new MenuGroup { Title = "📄 Заявления" };
        AddIfExists(pages, groupStatements.Items, "/applications", "Заявления");
        AddIfExists(pages, groupStatements.Items, "/registry/applications", "Реестр заявлений");
        AddIfExists(pages, groupStatements.Items, "/registry/rdz-orders", "Распоряжения РДЗ");
        AddIfExists(pages, groupStatements.Items, "/registry/rdi-orders", "Распоряжения РДИ");
        if (groupStatements.Items.Count > 0) Groups.Add(groupStatements);

        var groupDocs = new MenuGroup { Title = "📑 Документы" };
        AddIfExists(pages, groupDocs.Items, "/registry/contracts", "Договоры");
        AddIfExists(pages, groupDocs.Items, "/registry/acts", "Акты");
        AddIfExists(pages, groupDocs.Items, "/registry/agreements", "Соглашения");
        AddIfExists(pages, groupDocs.Items, "/registry/answers", "Ответы");
        if (groupDocs.Items.Count > 0) Groups.Add(groupDocs);

        var groupUsers = new MenuGroup { Title = "👤 Пользователи" };
        AddIfExists(pages, groupUsers.Items, "/users", "Пользователи");
        if (groupUsers.Items.Count > 0) Groups.Add(groupUsers);

        var groupStructure = new MenuGroup { Title = "🏢 Структура" };
        AddIfExists(pages, groupStructure.Items, "/departments", "Подразделения");
        AddIfExists(pages, groupStructure.Items, "/positions", "Должности");
        if (groupStructure.Items.Count > 0) Groups.Add(groupStructure);

        var groupRights = new MenuGroup { Title = "🔐 Права" };
        AddIfExists(pages, groupRights.Items, "/permissions", "Права доступа");
        AddIfExists(pages, groupRights.Items, "/permission-groups", "Группы прав");
        if (groupRights.Items.Count > 0) Groups.Add(groupRights);

        var groupSettings = new MenuGroup { Title = "⚙️ Настройки" };
        AddIfExists(pages, groupSettings.Items, "/services", "Сервисы");
        AddIfExists(pages, groupSettings.Items, "/service-templates", "Шаблоны услуг");
        AddIfExists(pages, groupSettings.Items, "/workflows", "Рабочие процессы");
        AddIfExists(pages, groupSettings.Items, "/document-templates", "Шаблоны документов");
        AddIfExists(pages, groupSettings.Items, "/number-templates", "Шаблоны номеров");
        AddIfExists(pages, groupSettings.Items, "/dictionaries", "Справочники");
        if (groupSettings.Items.Count > 0) Groups.Add(groupSettings);

        var groupReports = new MenuGroup { Title = "📊 Отчёты" };
        AddIfExists(pages, groupReports.Items, "/dashboard", "Общий дашборд");
        AddIfExists(pages, groupReports.Items, "/dashboard/specialist", "Специалист");
        AddIfExists(pages, groupReports.Items, "/dashboard/department", "Отдел");
        AddIfExists(pages, groupReports.Items, "/dashboard/management", "Управление");
        AddIfExists(pages, groupReports.Items, "/dashboard/director", "Директор");
        if (groupReports.Items.Count > 0) Groups.Add(groupReports);

        var groupAi = new MenuGroup { Title = "AI" };
        AddIfExists(pages, groupAi.Items, "/agent", "AI Agent");
        if (groupAi.Items.Count > 0) Groups.Add(groupAi);

        Console.WriteLine($"Created menu: {string.Join(" | ", Groups.Select(g => g.Title + ":" + string.Join(',', g.Items.Select(i => i.Title)) ))}");

        var missing = pages.Where(p => !Groups.SelectMany(g => g.Items).Any(i => i.Url == p.Route) && !p.Route.Contains("{")).Select(p => p.Route).ToList();
        if (missing.Any())
        {
            Console.WriteLine($"Warning: pages not added to menu: {string.Join(", ", missing)}");
        }

        if (Groups.Count == 0)
        {
            Groups.Add(new MenuGroup
            {
                Title = "Заявления",
                Items = new List<MenuItem>
                {
                    new() { Title = "Список заявлений", Url = "/applications" }
                }
            });
            Groups.Add(new MenuGroup
            {
                Title = "Документы",
                Items = new List<MenuItem>
                {
                    new() { Title = "Список документов", Url = "/documents" }
                }
            });
            Groups.Add(new MenuGroup
            {
                Title = "Настройки",
                Items = new List<MenuItem>
                {
                    new() { Title = "Общие", Url = "/settings" }
                }
            });
        }
    }

    private static void AddIfExists(List<(string Route, Type Type)> pages, List<MenuItem> items, string route, string title)
    {
        if (pages.Any(p => p.Route == route))
        {
            items.Add(new MenuItem { Title = title, Url = route });
        }
        else
        {
            Console.WriteLine($"Warning: menu link '{title}' -> '{route}' not found");
        }
    }

    public class MenuGroup
    {
        public string Title { get; set; } = string.Empty;
        public List<MenuItem> Items { get; set; } = new();
    }

    public class MenuItem
    {
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
