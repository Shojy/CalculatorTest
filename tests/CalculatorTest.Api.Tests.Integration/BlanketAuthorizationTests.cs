using System.Reflection;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Xunit.Abstractions;

namespace CalculatorTest.Api.Tests.Integration;

public class BlanketAuthorizationTests
{
    private readonly ITestOutputHelper _output;
    private const string Controller = nameof(Controller);
    private const string RootNamespace = nameof(CalculatorTest);
    private static readonly Type ControllerType = typeof(Controller);
    private static readonly string[] ExecutableExtensions = { ".exe", ".dll" };
    private static readonly Type PageType = typeof(PageModel);

    public BlanketAuthorizationTests(ITestOutputHelper output)
    {
        _output = output;
        LoadAllAssemblies();
    }

    [Fact]
    public void AllActionsOrParentControllerHaveAuthorizationAttributeTest()
    {
        var allControllers = GetAllControllerTypes();

        _output.WriteLine($"Found {allControllers.Count} controllers/pages");

        var unauthorizedControllers =
            GetControllerTypesThatAreMissing<AuthorizeAttribute>(allControllers);

        unauthorizedControllers =
            GetControllerTypesThatAreMissing<AllowAnonymousAttribute>(unauthorizedControllers);

        var notAuthorizedEndpoints = 0;

        foreach (var controller in unauthorizedControllers)
        {
            var actions =
                controller.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(m => m.MemberType == MemberTypes.Method && !m.IsSpecialName)
                    .ToList();

            var unauthorizedActions =
                actions.Where(
                        action =>
                            action.GetCustomAttribute<AuthorizeAttribute>() == null &&
                            action.GetCustomAttribute<AllowAnonymousAttribute>() == null)
                    .ToList();

            if (unauthorizedActions is null || unauthorizedActions.Count == 0)
            {
                continue;
            }

            unauthorizedActions.ForEach(
                action => _output.WriteLine($"{controller.FullName}.{action.Name} is unauthorized!"));
            notAuthorizedEndpoints += unauthorizedActions.Count;
        }
        notAuthorizedEndpoints.Should().Be(0, $"all endpoints should have [Authorize] or [AllowAnonymous]");
    }

    private static List<Type> GetAllControllerTypes()
        => AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a => a.FullName!.StartsWith(RootNamespace))
            .SelectMany(a => a.GetTypes()
                .Where(t => t.FullName!.EndsWith(Controller) ||
                            t.BaseType == ControllerType ||
                            t.DeclaringType == ControllerType ||
                            t.BaseType == PageType ||
                            t.DeclaringType == PageType))
            .ToList();

    private static List<Type> GetControllerTypesThatAreMissing<TAttribute>(
        IEnumerable<Type> types)
        where TAttribute : Attribute
        => types.Where(t => t.GetCustomAttribute<TAttribute>() == null)
            .ToList();

    private static bool IsExeOrDll(string path)
        => ExecutableExtensions.Any(
            extension =>
                extension.Equals(
                    Path.GetExtension(path),
                    StringComparison.OrdinalIgnoreCase));

    private static T TryCatchIgnore<T>(Func<T> func)
    {
        try
        {
            return func();
        }
        catch
        {
            return default!;
        }
    }

    private static void LoadAllAssemblies()
    {
        var assemblies =
            Directory.EnumerateFiles(
                    Path.GetDirectoryName(
                        Assembly.GetExecutingAssembly().Location) ?? string.Empty,
                    $"{RootNamespace}.*")
                .Where(IsExeOrDll)
                .Select(_ =>
                    TryCatchIgnore(
                        () => AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(_))))
                .ToList();

        assemblies.Should().NotBeNullOrEmpty();
    }
}