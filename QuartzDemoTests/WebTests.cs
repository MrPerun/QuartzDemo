using Microsoft.AspNetCore.Mvc.Testing;
using QuartzDemoWebApi;

namespace QuartzDemoTests;

public class WebTests
{
    private WebApplicationFactory<Program>? application;

    [TearDown]
    public async Task TearDown()
    {
        if (application != null)
        {
            await application.DisposeAsync();
            application = null;
        }
    }

    [SetUp]
    public void Setup()
    {
        TestContext.Out.WriteLine($"Starting test: {TestContext.CurrentContext.Test.FullName}");
        application = new WebApplicationFactory<Program>();
    }

    [Test]
    public async Task Test1()
    {
        await Test();
    }
    
    [Test]
    public async Task Test2()
    {
        await Test();
    }
    
    [Test]
    public async Task Test3()
    {
        await Test();
    }

    private async Task Test()
    {
        var response = await application.CreateClient().GetStringAsync("/WeatherForecast");
        Assert.That(response, Does.Contain("weather summary"));
    }
}
