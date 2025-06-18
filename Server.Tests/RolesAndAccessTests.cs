using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Xunit;
using GovServices.Server.Authorization;
using GovServices.Server.Controllers;

namespace Server.Tests;

public class RolesAndAccessTests
{
    [Fact]
    public void Clerical_Endpoint_Requires_Chancery_Role()
    {
        var method = typeof(RegistriesController).GetMethod("Clerical");
        Assert.NotNull(method);
        var attr = method!.GetCustomAttributes(typeof(AuthorizeAttribute), false)
            .Cast<AuthorizeAttribute>()
            .FirstOrDefault();
        Assert.NotNull(attr);
        Assert.Equal(RoleNames.Chancery, attr!.Roles);
    }
}
