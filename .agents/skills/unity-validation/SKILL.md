---
name: unity-validation
description: Diagnose Unity failures and validate gameplay changes using focused tests, Editor checks, and build evidence.
---

# Unity validation

Read root `AGENTS.md`. Establish the failing behavior, reproduction steps, and expected result. Inspect relevant scripts, scene wiring, package versions, and existing tests before choosing a check.

- Separate compiler errors, missing serialized references, lifecycle failures, and build-only failures. Find the first meaningful error rather than patching downstream symptoms.
- Check for an existing Unity process using this project before considering batch mode. Never run a competing Editor instance against the same project.
- Respect the repository-only write rule when selecting tooling. Unity can write user-level logs, preferences, licensing data, and caches even when `-logFile` points inside the project. Do not run it under that rule unless its write behavior is contained or the user explicitly authorizes an exception.
- For suitable logic, use Edit Mode tests. For object lifecycle, coroutine timing, physics, or scene behavior, use Play Mode tests. Reuse test assemblies if present; inspect assembly boundaries before adding an asmdef, since test assemblies cannot simply reference predefined `Assembly-CSharp` as a normal asmdef dependency.
- Keep tests deterministic: own and clean up test objects, restore modified global state, and avoid arbitrary wall-clock sleeps. Test meaningful behavior and edge cases, not private implementation details.
- If batch testing is available and permitted, select the appropriate `-runTests` and `-testPlatform` options and place test results and logs under the repository. Inspect the XML results and logs; a process exit alone is insufficient evidence that tests ran and passed.
- Validate builds only for an identified target with installed platform support. Do not switch platforms, install modules, or upgrade packages merely to complete a routine check.

Report the commands or Editor checks actually performed, results, and remaining gaps. If Unity cannot be run, use applicable static checks but label compilation, playtesting, and build checks as unverified.
