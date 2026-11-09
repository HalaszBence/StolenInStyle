---
name: unity-security
description: Threat-model and secure Unity features involving untrusted input, Steam identity, networking, saves, mods, native dependencies, or build credentials; review exploitable game behavior when requested.
---

# Unity game security

Read root `AGENTS.md` and the affected code, assets, package configuration, and
data flow. Consider security before choosing an implementation for a feature
that crosses a trust boundary. Do not assume this project has multiplayer,
servers, accounts, purchases, or mod support merely because packages exist.

## Establish the threat and boundary

- Identify what needs protection, who controls each input, and where a trusted
  decision occurs. Include local files, Steam callbacks, remote messages,
  command-line arguments, imported content, and developer tooling as applicable.
- Distinguish a player changing their own offline experience from an attack
  against other players, shared progression, accounts, purchases, or the machine.
  Prioritize concrete exploit paths and likely impact over hypothetical risks.
- Treat the game client as under the player's control. Client validation,
  obfuscation, and local encryption cannot establish authority over a shared
  economy or guarantee that a distributed executable is untampered.
- Suggest safer alternatives and explain usability, offline access, performance,
  and maintenance tradeoffs before changing those behaviors. Follow the user's
  existing choices and repository boundaries; a security review does not grant
  permission for unrelated configuration, external services, or account actions.

## Steam identity and secrets

- App IDs and Steam IDs are identifiers, not credentials. Never embed publisher
  Web API keys, signing secrets, upload passwords, or private keys in Unity
  scripts, assets, client builds, or source control. Keep secrets out of logs.
- Successful local Steam initialization is not server-side authentication. When
  a backend needs a trusted identity, validate Steam authentication tickets
  through the appropriate trusted Steam/server workflow and enforce ownership
  and authorization there. Do not trust a Steam ID claimed by a client.
- Keep privileged API calls and purchase or entitlement decisions on the trusted
  side when those features exist. Do not introduce a backend merely for an
  offline feature that has no shared trust requirement.
- If a real credential is exposed, avoid repeating it. Recommend revocation or
  rotation through the owning service; deleting it from one file is insufficient.
  Obtain authorization before performing external account changes.

## Untrusted data and game actions

- Validate structure, allowed values, lengths, sizes, counts, and numeric ranges
  at input boundaries. Reject non-finite numbers where they affect gameplay,
  coordinates, timing, or resource calculations. Bound expensive work and queued
  requests so malformed input cannot allocate or process without limit.
- For multiplayer features, validate authority, ownership, game state, frequency,
  and plausible action parameters on the authoritative peer or server. Define
  replay, ordering, and duplicate handling for operations that must happen once.
  A client reporting a reward or purchase is not evidence that it occurred.
- Parse saves and configuration as bounded data. Avoid unsafe object
  deserialization, evaluating file contents as code, or loading arbitrary types.
  Checksums detect accidental corruption; a key shipped with the client does not
  make local saves a trusted source for shared progression.
- Resolve paths against the intended data directory and check containment before
  access. For archives, account for traversal paths, links, excessive extracted
  size, and entry counts. Do not compose shell commands from untrusted values.
- Treat executable mods and native plugins as code with the game's permissions.
  Do not claim that an AssetBundle, scripting layer, or process is sandboxed
  without an actual isolation boundary. Respect the project's chosen mod policy.
- Preserve TLS certificate validation for network services. Avoid logging auth
  tickets, tokens, personal data, or entire untrusted payloads for debugging.

## Dependencies, tools, and builds

- Prefer maintained upstream packages and pin reviewed versions. Inspect new
  editor extensions, native libraries, and build scripts because they execute
  with developer privileges. Do not upgrade unrelated packages during a review.
- Use authoritative advisories and upstream documentation when checking a
  suspected vulnerability. A package version or a scanner warning alone is not
  proof that this game's usage is exploitable.
- Separate player-facing build content from local development files, upload
  credentials, and signing material. Exclude `steam_appid.txt` from Steam depots.
  Review file mappings before uploads; do not publish or change remote state
  without authorization.
- Keep agent-created tools, reports, downloads, and test artifacts inside this
  repository. Do not install system hooks, kernel anti-cheat, telemetry, or
  external scanning/upload services as an incidental security improvement.

## Verify and report

Use proportionate checks for the affected boundary: malformed and oversized
inputs, invalid identities, unauthorized actions, duplicate requests, or path
escapes where relevant. Use controlled local fixtures; do not attack live
services, scan unrelated systems, or execute suspicious content to inspect it.

Report concrete findings with the affected location, attacker-controlled input,
impact, and a practical mitigation. Separate verified vulnerabilities from
hypotheses and residual risks. State checks actually performed and limitations;
do not label the game secure, cheat-proof, or tamper-proof after a narrow check.

For Steam authentication and credential handling, consult the relevant official
documentation when implementing or reviewing those features:

- [Steam user authentication and ownership](https://partner.steamgames.com/doc/features/auth)
- [Steam Web API key authentication](https://partner.steamgames.com/doc/webapi_overview/auth)
- [Steam API initialization](https://partner.steamgames.com/doc/sdk/api)
