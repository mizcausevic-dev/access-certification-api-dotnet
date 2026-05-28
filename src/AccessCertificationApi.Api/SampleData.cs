namespace AccessCertificationApi.Api;

public static class SampleData
{
    public static readonly AccessCertificationExport Payload = new(
        Campaigns:
        [
            new(
                "cert-admin",
                "Privileged admin recertification",
                "Entra admin roles and break-glass assignments",
                "WATCH",
                "CURRENT",
                "Identity Governance",
                18,
                6,
                DateTimeOffset.Parse("2026-05-28T15:00:00Z")
            ),
            new(
                "cert-vendor",
                "Third-party access recertification",
                "External guests, service accounts, and privileged vendors",
                "CRITICAL",
                "STALE",
                "IAM Operations",
                11,
                5,
                DateTimeOffset.Parse("2026-05-25T09:30:00Z")
            )
        ],
        Gaps:
        [
            new(
                "gap-break-glass",
                "cert-admin",
                "PrivilegedAccess",
                "high",
                "Break-glass role certification",
                "Emergency roles keep active certification evidence and owner signoff.",
                "One emergency role still lacks current reviewer attestation after the latest cycle.",
                19,
                true
            ),
            new(
                "gap-vendor-guest",
                "cert-vendor",
                "GuestAccess",
                "high",
                "Vendor guest assignment set",
                "External guest access is reviewed against sponsor ownership and business need.",
                "Three vendor guest accounts still retain privileged app access without completed sponsor review.",
                33,
                true
            ),
            new(
                "gap-service-owner",
                "cert-vendor",
                "ServiceAccount",
                "high",
                "Legacy service account owner",
                "Service accounts maintain accountable owners and current certification notes.",
                "A legacy service principal remains active without a current accountable owner in the campaign packet.",
                29,
                true
            ),
            new(
                "gap-evidence-chain",
                "cert-admin",
                "Evidence",
                "medium",
                "Attestation evidence chain",
                "Reviewer decisions retain linked ticket, sponsor, and application evidence.",
                "Two completed decisions are missing the linked ticket thread that explains continued access.",
                14,
                false
            ),
            new(
                "gap-review-sla",
                "cert-admin",
                "ReviewCadence",
                "medium",
                "Quarterly reviewer cadence",
                "High-risk entitlements close inside the quarterly review SLA.",
                "Reviewer backlog is slipping beyond the expected window for one privileged app set.",
                21,
                false
            ),
            new(
                "gap-packet-signoff",
                "cert-vendor",
                "Signoff",
                "high",
                "Campaign attestation packet",
                "Certification packets close with reviewer, owner, and security signoff before renewal.",
                "The current packet is missing one reviewer closure and one security note.",
                17,
                true
            )
        ]
    );

    public static readonly IReadOnlyList<ReviewerLane> ReviewLanes =
    [
        new(
            "privileged-lane",
            "Privileged access lane",
            "Identity Governance",
            "red",
            "Break-glass roles, privileged apps, and emergency elevation reviews",
            "Close the break-glass attestation gap before renewing the admin campaign packet.",
            "Privileged roles are still carrying one unresolved review exception."
        ),
        new(
            "guest-lane",
            "Guest access lane",
            "IAM Operations",
            "red",
            "Vendor guests, sponsor ownership, and third-party entitlement proof",
            "Finish sponsor decisions for the remaining vendor guest assignments.",
            "External access remains over-entitled until sponsor evidence is complete."
        ),
        new(
            "service-lane",
            "Service account lane",
            "Platform Security",
            "yellow",
            "Legacy service principals, accountable owners, and non-human access review",
            "Attach the missing accountable owner record and reroute the legacy principal for decision.",
            "The service-account lane is recoverable once owner evidence is repaired."
        ),
        new(
            "packet-lane",
            "Packet signoff lane",
            "Security Governance",
            "red",
            "Attestation completeness, reviewer closure, and renewal confidence",
            "Close the missing reviewer and security signoff notes before packet renewal.",
            "The campaign packet is not yet safe for final renewal."
        )
    ];

    public static readonly IReadOnlyList<AttestationPacket> AttestationPackets =
    [
        new(
            "AC-12",
            "Privileged admin packet",
            "Security Governance",
            "red",
            57,
            "Break-glass evidence and one reviewer note remain open.",
            "Do not renew until privileged access decisions and supporting evidence are complete.",
            8
        ),
        new(
            "AC-18",
            "Third-party access packet",
            "IAM Operations",
            "red",
            63,
            "Vendor guest and service-account decisions are still incomplete.",
            "Hold renewal and push the packet back through guest and non-human access review.",
            10
        ),
        new(
            "AC-21",
            "Service-account exception packet",
            "Platform Security",
            "yellow",
            76,
            "Owner evidence is incomplete for one legacy principal.",
            "Packet can recover once owner mapping closes inside the next cycle.",
            16
        ),
        new(
            "AC-27",
            "Campaign evidence replay",
            "Identity Governance",
            "yellow",
            73,
            "Two decision records need linked ticket evidence before closure.",
            "Signoff posture is recoverable if evidence replay finishes before renewal.",
            18
        )
    ];
}
