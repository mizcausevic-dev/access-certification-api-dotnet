# access-certification-api-dotnet

C# / ASP.NET operator surface for routing privileged access reviews, guest entitlement renewals, service-account accountability, and attestation packet signoff into one readable control plane.

## Why this matters

Identity teams do not need another vague governance landing page. They need a board that keeps privileged roles, third-party guests, service accounts, reviewer cadence, and renewal signoff visible together before weak access decisions become silent entitlement debt.

This repo is the public proof surface for that pattern:

- `Hosted preview planned` for a browser-based access certification control plane
- `Embedded by engagement` for teams that need the routing model inside Entra, Okta, or adjacent identity-governance workflows

## What it includes

- ASP.NET Core minimal API in C#
- synthetic access-certification campaigns, review gaps, and attestation packets
- operator surfaces for:
  - `/campaign-lane`
  - `/review-exceptions`
  - `/attestation-posture`
  - `/verification`
  - `/docs`
- structured JSON endpoints under `/api/*`
- static Pages export with `robots.txt`, `sitemap.xml`, and `CNAME`

## Product depth

Access Certification API turns entitlement review sprawl into a board-readable IAM control surface. It is built for security leaders, identity-governance teams, platform owners, audit stakeholders, and operating partners who need to see whether privileged roles, guest access, service accounts, and campaign signoff are safe enough to renew.

For non-technical readers, it answers: which access lanes are exposed, who owns the remediation, what evidence is missing, and whether the renewal story is defensible. For technical reviewers, it exposes a C# / ASP.NET API, synthetic review packets, JSON endpoints, static Pages output, screenshots, tests, and smoke checks that make the public claim inspectable.

## What these repos have in common

This repo follows the Kinetic Gain control-plane pattern:

- name the operational ambiguity instead of hiding it inside screenshots or generic landing-page copy
- expose the decision surface as UI, JSON payloads, docs, screenshots, and validation commands
- connect GTM value, product narrative, technical proof, and executive review into the same public artifact
- keep public demos synthetic and safe while preserving enough structure to show how a real deployment would work

## Operating workflow

1. Load a synthetic access-certification packet covering campaigns, review exceptions, attestation posture, owners, and blockers.
2. Route privileged access, guest access, service-account, cadence, evidence, and signoff issues into one operator-readable surface.
3. Generate campaign-lane, exception, attestation, verification, and API views from the same evidence model.
4. Ship static public proof without exposing tenant, user, privileged-access, or production identity data.

## Screenshots

![Overview](./screenshots/01-overview.svg)
![Campaign lane](./screenshots/02-qc-lane.svg)
![Attestation posture](./screenshots/03-release-posture.svg)

## Verification

- synthetic access-certification and entitlement evidence only
- no tenant, user, or privileged production secrets
- no claim of SOC 2, ISO 27001, FedRAMP, or compliance certification
- this is a control-plane proof surface for IAM workflow depth, not a compliance certification claim

## Local run

```powershell
dotnet test
dotnet run --project src/AccessCertificationApi.Api -- --demo
dotnet run --project src/AccessCertificationApi.Api
```

Then open:

- `http://127.0.0.1:5087/`
- `http://127.0.0.1:5087/campaign-lane`
- `http://127.0.0.1:5087/review-exceptions`
- `http://127.0.0.1:5087/attestation-posture`

## Render static site

```powershell
dotnet run --project src/AccessCertificationApi.Api -- --prerender
```

## Related docs

- [Embedded framing](./docs/KINETIC_GAIN_EMBEDDED.md)
- [Origin story](./docs/ORIGIN.md)
