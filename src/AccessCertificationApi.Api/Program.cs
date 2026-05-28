using System.Text.Json;
using AccessCertificationApi.Api;

var app = AccessCertificationApplication.BuildApp(args);

if (args.Contains("--prerender"))
{
    await SiteBuilder.WriteAsync();
    return;
}

if (args.Contains("--demo"))
{
    Console.WriteLine(JsonSerializer.Serialize(AnalysisService.Summary(), new JsonSerializerOptions { WriteIndented = true }));
    Console.WriteLine(JsonSerializer.Serialize(SampleData.ReviewLanes, new JsonSerializerOptions { WriteIndented = true }));
    return;
}

app.Run();

public partial class Program;

public static class AccessCertificationApplication
{
    public static WebApplication BuildApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapGet("/", () => Results.Content(RenderService.Overview(), "text/html"));
        app.MapGet("/campaign-lane", () => Results.Content(RenderService.CampaignLane(), "text/html"));
        app.MapGet("/review-exceptions", () => Results.Content(RenderService.ReviewExceptions(), "text/html"));
        app.MapGet("/attestation-posture", () => Results.Content(RenderService.AttestationPosture(), "text/html"));
        app.MapGet("/verification", () => Results.Content(RenderService.Verification(), "text/html"));
        app.MapGet("/docs", () => Results.Content(RenderService.Docs(), "text/html"));

        app.MapGet("/api/dashboard/summary", () => Results.Json(AnalysisService.Summary()));
        app.MapGet("/api/campaign-lane", () => Results.Json(SampleData.ReviewLanes));
        app.MapGet("/api/review-exceptions", () => Results.Json(SampleData.Payload.Gaps));
        app.MapGet("/api/attestation-posture", () => Results.Json(SampleData.AttestationPackets));
        app.MapGet("/api/verification", () => Results.Json(new[]
        {
            "Synthetic access certification and entitlement evidence only; no tenant, user, or privileged production data is published.",
            "Reviewer cadence, sponsor ownership, service-account accountability, and packet signoff are modeled as operator surfaces.",
            "This repo demonstrates IAM and security workflow depth, not compliance-overclaim marketing."
        }));
        app.MapGet("/api/sample", () => Results.Text(RenderService.Sample(), "application/json"));

        return app;
    }
}

public static class SiteBuilder
{
    public static async Task WriteAsync()
    {
        var root = FindRepoRoot();
        var siteDir = Path.Combine(root, "site");
        Directory.CreateDirectory(siteDir);

        var pages = new Dictionary<string, string>
        {
            ["index.html"] = RenderService.Overview(),
            [Path.Combine("campaign-lane", "index.html")] = RenderService.CampaignLane(),
            [Path.Combine("review-exceptions", "index.html")] = RenderService.ReviewExceptions(),
            [Path.Combine("attestation-posture", "index.html")] = RenderService.AttestationPosture(),
            [Path.Combine("verification", "index.html")] = RenderService.Verification(),
            [Path.Combine("docs", "index.html")] = RenderService.Docs()
        };

        foreach (var (relative, html) in pages)
        {
            var target = Path.Combine(siteDir, relative);
            Directory.CreateDirectory(Path.GetDirectoryName(target)!);
            await File.WriteAllTextAsync(target, html);
        }

        var apiDir = Path.Combine(siteDir, "api");
        Directory.CreateDirectory(Path.Combine(apiDir, "dashboard"));
        await File.WriteAllTextAsync(Path.Combine(apiDir, "dashboard", "summary.json"), JsonSerializer.Serialize(AnalysisService.Summary(), new JsonSerializerOptions { WriteIndented = true }));
        await File.WriteAllTextAsync(Path.Combine(apiDir, "campaign-lane.json"), JsonSerializer.Serialize(SampleData.ReviewLanes, new JsonSerializerOptions { WriteIndented = true }));
        await File.WriteAllTextAsync(Path.Combine(apiDir, "review-exceptions.json"), JsonSerializer.Serialize(SampleData.Payload.Gaps, new JsonSerializerOptions { WriteIndented = true }));
        await File.WriteAllTextAsync(Path.Combine(apiDir, "attestation-posture.json"), JsonSerializer.Serialize(SampleData.AttestationPackets, new JsonSerializerOptions { WriteIndented = true }));
        await File.WriteAllTextAsync(Path.Combine(apiDir, "verification.json"), JsonSerializer.Serialize(new[]
        {
            "Synthetic access certification and entitlement evidence only; no tenant, user, or privileged production data is published.",
            "Reviewer cadence, sponsor ownership, service-account accountability, and packet signoff are modeled as operator surfaces.",
            "This repo demonstrates IAM and security workflow depth, not compliance-overclaim marketing."
        }, new JsonSerializerOptions { WriteIndented = true }));
        await File.WriteAllTextAsync(Path.Combine(apiDir, "sample.json"), RenderService.Sample());

        const string domain = "certs.kineticgain.com";
        await File.WriteAllTextAsync(Path.Combine(siteDir, "robots.txt"), $"User-agent: *{Environment.NewLine}Allow: /{Environment.NewLine}Sitemap: https://{domain}/sitemap.xml{Environment.NewLine}");
        await File.WriteAllTextAsync(Path.Combine(siteDir, "sitemap.xml"), """
<?xml version="1.0" encoding="UTF-8"?>
<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
  <url><loc>https://certs.kineticgain.com/</loc></url>
  <url><loc>https://certs.kineticgain.com/campaign-lane/</loc></url>
  <url><loc>https://certs.kineticgain.com/review-exceptions/</loc></url>
  <url><loc>https://certs.kineticgain.com/attestation-posture/</loc></url>
  <url><loc>https://certs.kineticgain.com/verification/</loc></url>
  <url><loc>https://certs.kineticgain.com/docs/</loc></url>
</urlset>
""");
        await File.WriteAllTextAsync(Path.Combine(siteDir, "CNAME"), domain + Environment.NewLine);
    }

    private static string FindRepoRoot()
    {
        var current = AppContext.BaseDirectory;
        for (var i = 0; i < 8; i++)
        {
            if (File.Exists(Path.Combine(current, "access-certification-api-dotnet.sln")))
            {
                return current;
            }

            current = Directory.GetParent(current)?.FullName
                ?? throw new DirectoryNotFoundException("Unable to resolve repo root.");
        }

        throw new DirectoryNotFoundException("Unable to resolve repo root.");
    }
}
