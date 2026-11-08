---
name: unity-performance
description: Investigate Unity frame-time, allocation, physics, and URP rendering problems using measured performance evidence.
---

# Unity performance

Read root `AGENTS.md`. Establish the target device, workload, scene, and desired frame-time or memory budget from the request or existing project documentation. Do not invent targets or assume mobile deployment from the renderer asset names.

- Inspect existing profiler captures or collect an authorized baseline using representative gameplay. Distinguish Editor overhead from player behavior, and check the repository write boundary before launching capture tools.
- Identify whether the limiting work is CPU, GPU, garbage collection, memory pressure, loading, or physics before choosing a change. If capture tooling is unavailable, label code findings as hypotheses rather than measured bottlenecks.
- For CPU and allocation issues, inspect per-frame allocations, repeated object searches, excessive callbacks, and collection growth. Introduce pooling only when object churn and lifecycle complexity justify it.
- For rendering, inspect the active URP asset, draw calls, overdraw, shadows, additional lights, and post-processing. Preserve visual intent and evaluate quality changes on the relevant platform configuration.
- For physics, inspect fixed timestep, collision layers, collider complexity, and unnecessary simulation work before reducing accuracy or changing gameplay behavior.
- Make a focused change and compare the same workload and capture conditions. Record the metric and any visual, responsiveness, or memory tradeoff; avoid claiming a speedup from code inspection alone.

Keep captures and reports inside the repository and out of commits unless requested. Report measured before-and-after results when available, or explain precisely what still needs profiling.
