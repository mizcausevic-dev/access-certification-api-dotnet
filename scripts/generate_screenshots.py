from pathlib import Path
import textwrap


ROOT = Path(__file__).resolve().parents[1]
SHOT_DIR = ROOT / "screenshots"
SHOT_DIR.mkdir(exist_ok=True)


def wrap(text: str, width: int):
    return textwrap.wrap(text, width=width) or [text]


def draw_lines(lines, x, y, font_size=22, color="#e9f3ff", weight="400", family="Inter,Segoe UI,Arial"):
    parts = [f'<text x="{x}" y="{y}" fill="{color}" font-size="{font_size}" font-weight="{weight}" font-family="{family}">']
    dy = 0
    for line in lines:
        parts.append(f'<tspan x="{x}" dy="{dy}">{line}</tspan>')
        dy = int(font_size * 1.25)
    parts.append("</text>")
    return "\n".join(parts)


def card(x, y, w, h, title, body, accent="#19c7ff"):
    body_lines = wrap(body, 34)
    return f"""
    <rect x="{x}" y="{y}" width="{w}" height="{h}" rx="20" fill="#0b1220" stroke="rgba(120,255,170,.18)" />
    {draw_lines([title.upper()], x + 28, y + 36, 12, accent, "700", "Consolas, monospace")}
    {draw_lines(wrap(title, 20), x + 28, y + 82, 30, "#f5f7ff", "700", "Georgia, serif")}
    {draw_lines(body_lines, x + 28, y + 132, 16, "#b8c6db", "400")}
    """


def shell(title, subtitle, inner):
    return f"""<svg xmlns="http://www.w3.org/2000/svg" width="1400" height="860" viewBox="0 0 1400 860">
    <rect width="1400" height="860" fill="#070a0f"/>
    <rect x="28" y="28" width="1344" height="804" rx="30" fill="#0a1426" stroke="rgba(120,255,170,.18)"/>
    <rect x="58" y="58" width="1284" height="110" rx="24" fill="#0b1220" stroke="rgba(120,255,170,.12)"/>
    {draw_lines([title.upper()], 94, 96, 14, "#37ff8b", "700", "Consolas, monospace")}
    {draw_lines(wrap(title, 26), 94, 140, 38, "#f5f7ff", "700", "Georgia, serif")}
    {draw_lines(wrap(subtitle, 92), 94, 188, 18, "#b8c6db", "400")}
    {inner}
    </svg>"""


overview = shell(
    "Access Certification API",
    "C# control plane for privileged access recertification, guest review pressure, service-account ownership, and packet signoff posture.",
    f"""
    <rect x="58" y="206" width="306" height="154" rx="22" fill="#0b1220" stroke="rgba(120,255,170,.12)"/>
    {draw_lines(["2"], 88, 274, 50, "#19c7ff", "700")}
    {draw_lines(["active campaigns", "one current renewal packet"], 88, 316, 16, "#b8c6db")}

    <rect x="382" y="206" width="306" height="154" rx="22" fill="#0b1220" stroke="rgba(120,255,170,.12)"/>
    {draw_lines(["6"], 412, 274, 50, "#b88cff", "700")}
    {draw_lines(["review gaps", "4 blocking signoff issues"], 412, 316, 16, "#b8c6db")}

    <rect x="706" y="206" width="306" height="154" rx="22" fill="#0b1220" stroke="rgba(120,255,170,.12)"/>
    {draw_lines(["3"], 736, 274, 50, "#ffcc66", "700")}
    {draw_lines(["access risks", "privileged and guest exposure"], 736, 316, 16, "#b8c6db")}

    <rect x="1030" y="206" width="312" height="154" rx="22" fill="#0b1220" stroke="rgba(120,255,170,.12)"/>
    {draw_lines(["Embedded"], 1060, 274, 40, "#37ff8b", "700")}
    {draw_lines(["hosted preview planned", "delivery by engagement"], 1060, 316, 16, "#b8c6db")}

    {card(58, 392, 620, 390, "Certification pressure stays visible before renewal", "Privileged role reviews, vendor guest entitlement decisions, service-account owners, and packet signoff remain readable together so access renewal does not become a blind administrative exercise.", "#19c7ff")}
    {card(708, 392, 634, 390, "Blocking exceptions are routed by lane and owner", "Identity Governance, IAM Operations, Platform Security, and Security Governance each get the same public proof frame: what is over-entitled, what lacks evidence, and what must close before access-certification renewal can be trusted.", "#37ff8b")}
    """,
)

lane = shell(
    "Campaign Lane",
    "Named lanes for privileged roles, guest accounts, service accounts, and packet signoff ownership.",
    f"""
    {card(58, 206, 620, 250, "Privileged access lane", "Identity Governance owns break-glass role certification, admin app recertification, and renewal blockers tied to elevated access.", "#19c7ff")}
    {card(708, 206, 634, 250, "Guest access lane", "IAM Operations keeps sponsor ownership, vendor guests, and third-party access decisions visible before renewal windows close.", "#ffcc66")}
    {card(58, 488, 620, 294, "Service account lane", "Platform Security tracks non-human access, legacy principals, and accountable-owner gaps so hidden exposure does not pass through certification silently.", "#b88cff")}
    {card(708, 488, 634, 294, "Packet signoff lane", "Security Governance keeps the campaign packet honest: reviewer closure, evidence links, and final signoff posture before renewal.", "#37ff8b")}
    """,
)

attestation = shell(
    "Attestation Posture",
    "Packet readiness, signoff blockers, and the next review window stay readable for IAM and security operators.",
    f"""
    {card(58, 206, 402, 256, "AC-12 · privileged admin packet", "57 percent complete. Renewal stays blocked until break-glass evidence and reviewer notes are closed.", "#ff5c7a")}
    {card(498, 206, 402, 256, "AC-18 · third-party access packet", "63 percent complete. Vendor guests and service-account decisions are still incomplete.", "#ffcc66")}
    {card(938, 206, 404, 256, "AC-27 · evidence replay packet", "73 percent complete. Renewal posture is recoverable if linked ticket evidence finishes on time.", "#37ff8b")}
    {card(58, 494, 1284, 288, "Why this monetizes cleanly", "Hosted preview planned gives a public control-plane hook. Paid templates can adapt campaign fields and review lanes. Embedded delivery fits enterprises that need access-certification routing inside Entra, Okta, or adjacent identity stacks.", "#19c7ff")}
    """,
)

(SHOT_DIR / "01-overview.svg").write_text(overview, encoding="utf-8")
(SHOT_DIR / "02-qc-lane.svg").write_text(lane, encoding="utf-8")
(SHOT_DIR / "03-release-posture.svg").write_text(attestation, encoding="utf-8")
