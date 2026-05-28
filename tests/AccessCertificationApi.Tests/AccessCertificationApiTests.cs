using AccessCertificationApi.Api;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AccessCertificationApi.Tests;

public sealed class AccessCertificationApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AccessCertificationApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Overview_route_renders_access_certification_shell()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/");
        var html = await response.Content.ReadAsStringAsync();

        Assert.True(response.IsSuccessStatusCode);
        Assert.Contains("Access Certification API", html);
        Assert.Contains("privileged access", html);
    }

    [Fact]
    public async Task Api_summary_returns_expected_counts()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/dashboard/summary");
        var json = await response.Content.ReadAsStringAsync();

        Assert.True(response.IsSuccessStatusCode);
        Assert.Contains("\"campaigns\":2", json);
        Assert.Contains("\"blockingGaps\":4", json);
    }

    [Fact]
    public void Analysis_flags_high_risk_access_certification_gaps()
    {
        var report = AnalysisService.Analyze(SampleData.Payload);

        Assert.Equal(2, report.Campaigns);
        Assert.Equal(6, report.Gaps);
        Assert.Contains(report.Findings, finding => finding.Code == "privileged-access-gap");
        Assert.Contains(report.Findings, finding => finding.Code == "packet-signoff-gap");
    }
}
