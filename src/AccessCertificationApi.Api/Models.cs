namespace AccessCertificationApi.Api;

public sealed record CertificationCampaign(
    string Id,
    string Name,
    string Scope,
    string Status,
    string SnapshotStatus,
    string Owner,
    int ReviewerCount,
    int DecisionBacklog,
    DateTimeOffset CollectedAt
);

public sealed record CertificationGap(
    string Id,
    string CampaignId,
    string ControlFamily,
    string Severity,
    string Subject,
    string ExpectedState,
    string ObservedState,
    int HoursOpen,
    bool BlocksSignoff
);

public sealed record ReviewerLane(
    string Id,
    string Lane,
    string Owner,
    string Status,
    string Focus,
    string NextAction,
    string Note
);

public sealed record AttestationPacket(
    string PacketId,
    string Lane,
    string Owner,
    string Status,
    int CompletenessScore,
    string Blocker,
    string DecisionNote,
    int ReviewWindowHours
);

public sealed record AccessCertificationExport(
    IReadOnlyList<CertificationCampaign> Campaigns,
    IReadOnlyList<CertificationGap> Gaps
);

public sealed record AccessCertificationFinding(
    string Code,
    string Severity,
    string Subject,
    string Message,
    string Owner
);

public sealed record CertificationPostureReport(
    int Campaigns,
    int CurrentCampaigns,
    int Gaps,
    int BlockingGaps,
    int AccessRisks,
    int SignoffRisks,
    IReadOnlyList<AccessCertificationFinding> Findings,
    bool Ok
);
