using System.Text.Json;

namespace AccessCertificationApi.Api;

public static class RenderService
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public static string Overview() => Layout(
        "Access Certification API",
        "/",
        $$"""
        <section class="section">
          <div class="sh"><h2>Operator Snapshot</h2><div class="note">iam / security / access certification</div></div>
          <div class="kpis">
            {{Metric("2", "campaigns", "Synthetic privileged and third-party recertification campaigns across active access lanes.", "cyan")}}
            {{Metric("1", "current campaigns", "Only one campaign snapshot is current enough to trust without replay.", "green")}}
            {{Metric("6", "gaps", "Review deviations across privileged access, guest access, evidence, cadence, and signoff.", "plum")}}
            {{Metric("4", "blocking gaps", "Renewal-blocking issues still need closure before campaign signoff.", "red")}}
            {{Metric("3", "access risks", "Privileged, guest, and service-account exposure still need repair.", "amber")}}
            {{Metric("3", "signoff risks", "Evidence, cadence, and final packet posture remain incomplete.", "red")}}
          </div>
        </section>
        <section class="section">
          <div class="sh"><h2>Why this lane matters</h2><div class="note">c# / dotnet / iam</div></div>
          <div class="stack">
            <div class="src"><div class="src-name">renewal confidence</div><div class="src-tit">Stop weak entitlement renewals before they become silent access debt</div><p>Identity teams need one board where privileged roles, vendor guests, service accounts, reviewer cadence, and signoff posture stay visible together.</p></div>
            <div class="src"><div class="src-name">founder edge</div><div class="src-tit">CyberArk and enterprise identity depth, not generic admin filler</div><p>This follows the Kinetic Gain pattern: routing, evidence, approvals, and operator-safe remediation posture for real access governance work.</p></div>
            <div class="src"><div class="src-name">monetization path</div><div class="src-tit">Hosted preview planned · Embedded by engagement</div><p>The free surface shows the operator model; the commercial path is an embedded access-certification module inside enterprise identity workflows.</p></div>
          </div>
        </section>
        """
    );

    public static string CampaignLane() => Layout(
        "Access Certification API — Campaign Lane",
        "/campaign-lane",
        $$"""
        <section class="section">
          <div class="sh"><h2>Campaign Lane</h2><div class="note">owner · focus · next action</div></div>
          <table class="ttbl">
            <thead><tr><th>Lane</th><th>Owner</th><th>Status</th><th>Focus</th><th>Next action</th></tr></thead>
            <tbody>
              {{string.Join("", SampleData.ReviewLanes.Select(lane => $$"""
                <tr>
                  <td><b>{{lane.Lane}}</b><br />{{lane.Note}}</td>
                  <td>{{lane.Owner}}</td>
                  <td><span class="st {{SeverityClass(lane.Status)}}">{{lane.Status}}</span></td>
                  <td>{{lane.Focus}}</td>
                  <td>{{lane.NextAction}}</td>
                </tr>
              """))}}
            </tbody>
          </table>
        </section>
        """
    );

    public static string ReviewExceptions() => Layout(
        "Access Certification API — Review Exceptions",
        "/review-exceptions",
        $$"""
        <section class="section">
          <div class="sh"><h2>Review Exceptions</h2><div class="note">severity · owner · subject</div></div>
          <table class="ttbl">
            <thead><tr><th>Risk</th><th>Owner</th><th>Subject</th><th>Observed state</th></tr></thead>
            <tbody>
              {{string.Join("", SampleData.Payload.Gaps.Select(gap => $$"""
                <tr>
                  <td><span class="st {{SeverityClass(gap.Severity)}}">{{gap.Severity}}</span><br /><b>{{gap.ControlFamily}}</b></td>
                  <td>{{OwnerForGap(gap.ControlFamily)}}</td>
                  <td>{{gap.Subject}}</td>
                  <td>{{gap.ObservedState}}</td>
                </tr>
              """))}}
            </tbody>
          </table>
        </section>
        """
    );

    public static string AttestationPosture() => Layout(
        "Access Certification API — Attestation Posture",
        "/attestation-posture",
        $$"""
        <section class="section">
          <div class="sh"><h2>Attestation Posture</h2><div class="note">packet readiness · blocker · timing</div></div>
          <div class="board">
            {{string.Join("", SampleData.AttestationPackets.Select(packet => $$"""
              <article class="pcard">
                <div class="ptop">
                  <div class="pnum">{{packet.CompletenessScore}}%</div>
                  <div class="ppri">{{packet.Owner}}</div>
                </div>
                <h3>{{packet.Lane}}</h3>
                <p class="pdesc">{{packet.DecisionNote}}</p>
                <ul class="check">
                  <li>{{packet.Blocker}}</li>
                  <li>{{packet.ReviewWindowHours}} hours to the next renewal checkpoint</li>
                  <li>Status: <span class="st {{SeverityClass(packet.Status)}}">{{packet.Status}}</span></li>
                </ul>
                <div class="pfoot"><code>{{packet.PacketId}}</code></div>
              </article>
            """))}}
          </div>
        </section>
        """
    );

    public static string Verification() => Layout(
        "Access Certification API — Verification",
        "/verification",
        $$"""
        <section class="section">
          <div class="sh"><h2>Verification</h2><div class="note">operator-safe claims only</div></div>
          <div class="stack">
            {{VerificationCard("This repo uses synthetic access certification and entitlement evidence only; no tenant, user, or privileged production data is published.")}}
            {{VerificationCard("The control plane is rooted in reviewer cadence, sponsor ownership, service-account accountability, and packet signoff mechanics.")}}
            {{VerificationCard("This is an IAM and security operator surface, not a compliance-overclaim page.")}}
            {{VerificationCard("Hosted preview is planned; embedded module delivery is available by engagement.")}}
          </div>
        </section>
        """
    );

    public static string Docs() => Layout(
        "Access Certification API — Docs",
        "/docs",
        $$"""
        <section class="section">
          <div class="sh"><h2>Docs</h2><div class="note">routes · runbook · api</div></div>
          <div class="stack">
            <div class="src"><div class="src-name">routes</div><div class="src-tit">Public proof surface</div><p><code>/</code>, <code>/campaign-lane</code>, <code>/review-exceptions</code>, <code>/attestation-posture</code>, <code>/verification</code>, <code>/docs</code></p></div>
            <div class="src"><div class="src-name">api</div><div class="src-tit">Structured payloads</div><p><code>/api/dashboard/summary</code>, <code>/api/campaign-lane</code>, <code>/api/review-exceptions</code>, <code>/api/attestation-posture</code>, <code>/api/verification</code>, <code>/api/sample</code></p></div>
            <div class="src"><div class="src-name">runbook</div><div class="src-tit">Local execution</div><p><code>dotnet run --project src/AccessCertificationApi.Api -- --demo</code> prints the same campaign posture used by the public proof surface.</p></div>
          </div>
        </section>
        """
    );

    public static string Sample() => JsonSerializer.Serialize(new
    {
        summary = AnalysisService.Summary(),
        campaignLane = SampleData.ReviewLanes,
        reviewExceptions = SampleData.Payload.Gaps,
        attestationPosture = SampleData.AttestationPackets,
        sample = SampleData.Payload
    }, JsonOptions);

    private static string VerificationCard(string title) =>
        $$"""<div class="src"><div class="src-name">verification</div><div class="src-tit">{{title}}</div><p>This lane keeps renewal pressure, entitlement evidence, and commercial framing honest.</p></div>""";

    private static string Metric(string value, string label, string help, string tone) =>
        $$"""<div class="kpi {{tone}}"><div class="v">{{value}}</div><div class="lbl">{{label}}</div><div class="h">{{help}}</div></div>""";

    private static string OwnerForGap(string family) => family switch
    {
        "PrivilegedAccess" => "Identity Governance",
        "GuestAccess" => "IAM Operations",
        "ServiceAccount" => "Platform Security",
        "Evidence" => "Security Governance",
        "ReviewCadence" => "Identity Governance",
        "Signoff" => "Security Governance",
        _ => "IAM Operations"
    };

    private static string SeverityClass(string value) => value switch
    {
        "high" or "red" => "red",
        "medium" or "yellow" => "yellow",
        "green" or "low" => "green",
        _ => "info"
    };

    public static string Layout(string title, string active, string body)
    {
        var nav = new[]
        {
            Nav("/", "Overview"),
            Nav("/campaign-lane", "Campaign Lane"),
            Nav("/review-exceptions", "Review Exceptions"),
            Nav("/attestation-posture", "Attestation Posture"),
            Nav("/verification", "Verification"),
            Nav("/docs", "Docs")
        };

        var navHtml = string.Join("", nav.Select(item =>
            item.Href == active
                ? $"""<a class="navchip active" href="{item.Href}">{item.Label}</a>"""
                : $"""<a class="navchip" href="{item.Href}">{item.Label}</a>"""));

        return $$$"""
        <!DOCTYPE html>
        <html lang="en">
        <head>
          <meta charset="utf-8" />
          <meta name="viewport" content="width=device-width, initial-scale=1" />
          <title>{{{title}}}</title>
          <style>
            :root{--bg:#070a0f;--panel:#0b1220;--line:rgba(120,255,170,.18);--line2:rgba(120,255,170,.10);--text:#e9f3ff;--muted:rgba(233,243,255,.72);--muted2:rgba(233,243,255,.55);--bert:#37ff8b;--bert2:#19c7ff;--warn:#ffcc66;--bad:#ff5c7a;--shadow:0 18px 60px rgba(0,0,0,.55);--mono:ui-monospace,SFMono-Regular,Menlo,Monaco,Consolas,"Liberation Mono","Courier New",monospace;--sans:ui-sans-serif,system-ui,-apple-system,Segoe UI,Roboto,Helvetica,Arial}
            *{box-sizing:border-box} body{margin:0;font-family:var(--sans);color:var(--text);background:radial-gradient(1200px 600px at 20% -10%, rgba(55,255,139,.18), transparent 60%),radial-gradient(900px 520px at 90% 0%, rgba(25,199,255,.16), transparent 55%),linear-gradient(180deg,#05070c 0%,#070a0f 35%,#05070c 100%)}
            .wrap{max-width:1280px;margin:0 auto;padding:24px 22px 80px}.topbar{display:flex;justify-content:space-between;gap:14px;border-bottom:1px solid var(--line2);padding-bottom:14px;margin-bottom:22px;font-family:var(--mono);font-size:11px;letter-spacing:.16em;color:var(--muted);text-transform:uppercase}.topbar .left{color:var(--bert)}
            .herorow{display:grid;grid-template-columns:1.5fr .9fr;gap:18px}@media (max-width:1000px){.herorow{grid-template-columns:1fr}}
            .hero,.src,.pcard,.kpi,.bluf,.corr{background:linear-gradient(180deg, rgba(11,18,32,.95), rgba(8,14,26,.92));border:1px solid var(--line);box-shadow:var(--shadow)}
            .hero{border-radius:22px;padding:28px 28px 24px;border-top:2px solid var(--bert2)} .hero h1{font-size:60px;line-height:.95;margin:0 0 18px;font-weight:800}@media (max-width:700px){.hero h1{font-size:42px}} .hero p{color:var(--muted);font-size:15px;line-height:1.55;max-width:680px;margin:0 0 18px}
            .chiprow,.navrow{display:flex;flex-wrap:wrap;gap:8px}.navrow{margin-top:18px}.meta-chip,.navchip,.ppri,.st,code{font-family:var(--mono)} .meta-chip,.navchip{font-size:11px;color:var(--muted);padding:7px 12px;border-radius:999px;border:1px solid var(--line);background:rgba(6,10,18,.4);text-decoration:none}.navchip.active{color:#071017;background:linear-gradient(135deg,var(--bert),var(--bert2));font-weight:700}
            .side{display:flex;flex-direction:column;gap:14px}.bluf,.corr{border-radius:14px;padding:16px 18px}.bluf{border-left:4px solid var(--warn)}.corr{border-left:4px solid var(--bert)}.lbl{font-family:var(--mono);font-size:10px;letter-spacing:.18em;text-transform:uppercase}.bluf .lbl{color:var(--warn)} .corr .lbl{color:var(--bert)} .bluf p,.corr p,.src p,.pcard .pdesc,.kpi .h{color:var(--muted);line-height:1.55}
            .section{margin-top:34px}.sh{display:flex;justify-content:space-between;gap:14px;padding-bottom:10px;border-bottom:1px solid var(--line2);margin-bottom:14px}.sh h2{margin:0;font-size:24px;font-weight:600}.sh .note{font-family:var(--mono);font-size:11px;color:var(--muted2);letter-spacing:.16em;text-transform:uppercase}
            .kpis{display:grid;grid-template-columns:repeat(6,1fr);gap:12px}@media (max-width:1100px){.kpis{grid-template-columns:repeat(3,1fr)}}@media (max-width:640px){.kpis{grid-template-columns:repeat(2,1fr)}} .kpi{border-radius:14px;padding:14px 14px 12px}.kpi .v{font-size:26px;font-weight:600}.kpi .lbl{font-size:10px;letter-spacing:.18em;text-transform:uppercase;color:var(--muted);margin-top:6px}.cyan .v{color:var(--bert2)} .green .v{color:var(--bert)} .plum .v{color:#b88cff} .amber .v,.yellow{color:var(--warn)} .red .v,.red{color:var(--bad)}
            .stack{display:grid;grid-template-columns:repeat(3,1fr);gap:12px}@media (max-width:1100px){.stack{grid-template-columns:repeat(2,1fr)}}@media (max-width:640px){.stack{grid-template-columns:1fr}} .src{border-radius:16px;padding:16px}.src-name{font-family:var(--mono);font-size:11px;color:var(--bert);letter-spacing:.2em;text-transform:uppercase}.src-tit{margin:8px 0 6px;font-size:17px;font-weight:600}
            .ttbl{width:100%;border-collapse:separate;border-spacing:0;border:1px solid var(--line);border-radius:14px;overflow:hidden}.ttbl th,.ttbl td{padding:13px 14px;text-align:left;font-size:13.5px;vertical-align:top}.ttbl thead th{font-family:var(--mono);font-size:11px;letter-spacing:.16em;text-transform:uppercase;color:var(--muted2);border-bottom:1px solid var(--line);background:rgba(11,18,32,.5)}.ttbl td,.ttbl td *{color:var(--muted)}.ttbl b{color:var(--text)}
            .board{display:grid;grid-template-columns:repeat(3,1fr);gap:14px}@media (max-width:1000px){.board{grid-template-columns:1fr}} .pcard{border-radius:16px;padding:18px 20px;display:flex;flex-direction:column}.ptop{display:flex;justify-content:space-between;align-items:center;margin-bottom:8px}.pnum{font-family:var(--mono);font-size:22px;font-weight:600;color:var(--bert)}.ppri{font-size:10px;padding:5px 10px;border-radius:999px;border:1px solid var(--line);color:var(--bert);letter-spacing:.14em;background:rgba(55,255,139,.06)}.pcard h3{margin:6px 0 8px;font-size:19px}.check{list-style:none;padding:0;margin:0 0 14px}.check li{display:grid;grid-template-columns:18px 1fr;gap:10px;padding:6px 0;font-size:13.5px;color:var(--muted)}.check li:before{content:"";width:14px;height:14px;border:1px solid var(--line);border-radius:3px;background:rgba(6,10,18,.4);margin-top:3px}
            .st{font-size:10px;padding:4px 9px;border-radius:6px;letter-spacing:.1em;text-transform:uppercase;border:1px solid currentColor;display:inline-block}.st.green{color:var(--bert)}.st.yellow{color:var(--warn)}.st.red{color:var(--bad)}.st.info{color:var(--bert2)}
            .footer{margin-top:30px;padding-top:14px;border-top:1px dashed var(--line2);display:flex;justify-content:space-between;gap:10px;flex-wrap:wrap;font-family:var(--mono);font-size:11px;color:var(--muted2);letter-spacing:.08em} code{font-size:12px;color:var(--bert2);background:rgba(25,199,255,.08);padding:1px 6px;border-radius:5px;border:1px solid rgba(25,199,255,.18)}
          </style>
        </head>
        <body>
          <div class="wrap">
            <div class="topbar">
              <div class="left">Kinetic Gain · Access Certification API</div>
              <div>synthetic access certification snapshots · no tenant or privileged production secrets</div>
            </div>
            <div class="herorow">
              <section class="hero">
                <div class="chiprow">
                  <span class="meta-chip">Wave 21 · Polyglot Language and Vertical Expansion</span>
                  <span class="meta-chip">C# · IAM / Security operator proof</span>
                  <span class="meta-chip">Hosted preview planned · Embedded by engagement</span>
                </div>
                <h1>Privileged access reviews, guest renewals, and signoff posture that stay operator-readable.</h1>
                <p>This control plane turns synthetic access certification exports into one review surface: privileged-role renewals, sponsor-backed guest reviews, service-account ownership, evidence continuity, and attestation packet completeness before renewal confidence is asserted.</p>
                <div class="navrow">{{{navHtml}}}</div>
              </section>
              <aside class="side">
                <div class="bluf"><div class="lbl">Commercial front door</div><p><strong>Access certification routing, entitlement evidence posture, and renewal operations for enterprise IAM teams.</strong><br />The free surface is buyer-readable proof; the commercial path is an embedded certification module for identity governance workflows.</p></div>
                <div class="corr"><div class="lbl">Proof layer</div><p><strong>.NET API + static operator shell.</strong><br />The repo models reviewer cadence, sponsor ownership, service-account accountability, and packet signoff without pretending to be a live tenant integration.</p></div>
                <div class="corr"><div class="lbl">Why it matters</div><p>Identity teams need renewal pressure and entitlement evidence in one lane, not scattered exports, tickets, and stale review packets.</p></div>
              </aside>
            </div>
            {{{body}}}
            {{{ProductDepth()}}}
            {{{CommonPattern()}}}
            <div class="footer">
              <div>access-certification-api-dotnet · synthetic sample data only</div>
              <div>
                <a class="meta-chip" href="https://github.com/mizcausevic-dev/access-certification-api-dotnet">GitHub</a>
                <a class="meta-chip" href="https://portfolio.kineticgain.com/">Portfolio</a>
                <a class="meta-chip" href="https://suite.kineticgain.com/">Suite</a>
                <a class="meta-chip" href="https://www.linkedin.com/in/mirzacausevic/">LinkedIn</a>
                <a class="meta-chip" href="https://kineticgain.com/">Kinetic Gain</a>
              </div>
            </div>
          </div>
        </body>
        </html>
        """;
    }

    private static string ProductDepth() => """
        <section class="section">
          <div class="sh"><h2>Product depth</h2><div class="note">buyer value · technical proof · GTM story</div></div>
          <div class="stack">
            <div class="src">
              <div class="src-name">SaaS GTM analyst</div>
              <div class="src-tit">Sell access-certification clarity, not generic IAM posture.</div>
              <p>The product frames renewal readiness around exposed access lanes, accountable owners, missing evidence, and whether the certification story is safe enough for leadership review.</p>
            </div>
            <div class="src">
              <div class="src-name">Value architect</div>
              <div class="src-tit">Turn entitlement debt into prioritized remediation.</div>
              <p>Privileged roles, third-party guests, service accounts, reviewer cadence, evidence gaps, and signoff blockers resolve into one inspectable action surface.</p>
            </div>
            <div class="src">
              <div class="src-name">Technical proof</div>
              <div class="src-tit">Back the narrative with a working .NET implementation.</div>
              <p>The repo includes C# scoring, ASP.NET routes, JSON APIs, synthetic fixtures, static Pages output, screenshots, tests, smoke checks, and public-data safety boundaries.</p>
            </div>
          </div>
        </section>
        """;

    private static string CommonPattern() => """
        <section class="section">
          <div class="sh"><h2>What these repos have in common</h2><div class="note">risk · owner · proof · next action</div></div>
          <div class="board">
            <article class="pcard">
              <div class="ptop"><div class="pnum">01</div><div class="ppri">Risk</div></div>
              <h3>Each product names the ambiguity.</h3>
              <p class="pdesc">The Kinetic Gain pattern turns cost, governance, reliability, revenue, or compliance drag into a named decision surface instead of a screenshot dump.</p>
            </article>
            <article class="pcard">
              <div class="ptop"><div class="pnum">02</div><div class="ppri">Proof</div></div>
              <h3>The claim is backed by implementation evidence.</h3>
              <p class="pdesc">Routes, APIs, CLI output, fixtures, docs, screenshots, and validation commands make the surface reviewable for technical and non-technical readers.</p>
            </article>
            <article class="pcard">
              <div class="ptop"><div class="pnum">03</div><div class="ppri">Action</div></div>
              <h3>The next move is visible.</h3>
              <p class="pdesc">Every lane resolves into owner, severity, blocker, recommendation, and executive narrative so the surface supports a decision, not just discovery.</p>
            </article>
          </div>
        </section>
        """;

    private static (string Href, string Label) Nav(string href, string label) => (href, label);
}
