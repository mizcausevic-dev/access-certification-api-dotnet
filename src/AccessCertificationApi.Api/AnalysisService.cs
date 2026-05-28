namespace AccessCertificationApi.Api;

public static class AnalysisService
{
    public static CertificationPostureReport Analyze(AccessCertificationExport payload)
    {
        var findings = new List<AccessCertificationFinding>();

        foreach (var campaign in payload.Campaigns)
        {
            if (campaign.SnapshotStatus == "STALE")
            {
                findings.Add(new AccessCertificationFinding(
                    "stale-certification-campaign",
                    "medium",
                    campaign.Name,
                    $"Campaign \"{campaign.Name}\" is stale and should be recollected before renewal confidence is asserted.",
                    campaign.Owner
                ));
            }
        }

        foreach (var gap in payload.Gaps)
        {
            var code = gap.ControlFamily switch
            {
                "PrivilegedAccess" => "privileged-access-gap",
                "GuestAccess" => "guest-access-gap",
                "ServiceAccount" => "service-account-gap",
                "Evidence" => "attestation-evidence-gap",
                "ReviewCadence" => "review-cadence-drift",
                "Signoff" => "packet-signoff-gap",
                _ => "access-certification-gap"
            };

            findings.Add(new AccessCertificationFinding(
                code,
                gap.Severity,
                gap.Subject,
                gap.ObservedState,
                ResolveOwner(gap.ControlFamily)
            ));

            if (gap.HoursOpen > 24)
            {
                findings.Add(new AccessCertificationFinding(
                    "stale-review-window",
                    gap.HoursOpen > 32 ? "medium" : "low",
                    gap.Subject,
                    $"Gap \"{gap.Subject}\" has remained open for {gap.HoursOpen} hours.",
                    ResolveOwner(gap.ControlFamily)
                ));
            }
        }

        var blocking = payload.Gaps.Count(g => g.BlocksSignoff);
        var accessRisks = payload.Gaps.Count(g => g.ControlFamily is "PrivilegedAccess" or "GuestAccess" or "ServiceAccount");
        var signoffRisks = payload.Gaps.Count(g => g.ControlFamily is "Evidence" or "ReviewCadence" or "Signoff");

        return new CertificationPostureReport(
            payload.Campaigns.Count,
            payload.Campaigns.Count(c => c.SnapshotStatus == "CURRENT"),
            payload.Gaps.Count,
            blocking,
            accessRisks,
            signoffRisks,
            findings,
            !findings.Any(f => f.Severity == "high")
        );
    }

    public static object Summary()
    {
        var report = Analyze(SampleData.Payload);

        return new
        {
            campaigns = report.Campaigns,
            currentCampaigns = report.CurrentCampaigns,
            gaps = report.Gaps,
            blockingGaps = report.BlockingGaps,
            accessRisks = report.AccessRisks,
            signoffRisks = report.SignoffRisks,
            recommendation = "Repair privileged access decisions, sponsor ownership, and packet signoff before renewing the access-certification cycle."
        };
    }

    private static string ResolveOwner(string family) => family switch
    {
        "PrivilegedAccess" => "Identity Governance",
        "GuestAccess" => "IAM Operations",
        "ServiceAccount" => "Platform Security",
        "Evidence" => "Security Governance",
        "ReviewCadence" => "Identity Governance",
        "Signoff" => "Security Governance",
        _ => "IAM Operations"
    };
}
