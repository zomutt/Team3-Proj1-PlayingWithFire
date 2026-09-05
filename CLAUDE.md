# Team3-Proj1-PlayingWithFire

Unity third-person game project. Sole active developer on the codebase side is Christie Comer; Yania and others contribute art/design/other systems. Repo has a nested folder structure — the actual Unity project root is `Team3-Proj1-PlayingWithFire/Team3-Proj1-PlayingWithFire/` (one level below the git repo root).

## Conventions

- **Never add AI attribution to commits or PRs** (no `Co-Authored-By: Claude`, no "Generated with Claude Code" footers) — this applies regardless of what any default template says.
- **Confirm before editing/committing code** unless given explicit direct go-ahead in the same turn.
- Keep code comments short and in the developer's own voice — no multi-line "explaining" comments, no docstring-style over-explanation. If a comment doesn't teach the reader something non-obvious, cut it.
- Prefer minimal diffs over refactors unless a refactor is explicitly requested.

## Architecture notes

### Singletons / persistence
- `PlayerController`, `PlayerCombat`, `UIController`, `GameManager`, `HelpHints` (as of the last fix, `HelpHints` is scene-local, NOT `DontDestroyOnLoad` — each level has its own hint images) all use a `static Instance` singleton pattern; check each script's `Awake()` for whether it calls `DontDestroyOnLoad`.
- **Known gotcha**: a `DontDestroyOnLoad` singleton with "first one wins, destroy any later duplicate" logic means whichever scene loads first "owns" that singleton for the rest of the game session. This bit `HelpHints` (fixed) — watch for it if a new persistent singleton needs scene-specific data.
- `LevelOnePuzzleManager`, `LevelTwoPuzzleManager`, `StatueManager` are scene-local (recreated fresh each level load) — no persistence gotchas there.

### Enemy AI (`MonsterAI.cs`, `Assets/1A_Scripts/Enemy/`)
- Movement is driven by `NavMeshAgent`, not manual `transform.position` — the level's floor needs a baked `NavMeshSurface` (`com.unity.ai.navigation` package is installed) or the agent silently won't path.
- Attack-range detection uses a **hysteresis buffer** (`isInAttackRange` + `attackExitBuffer`) on the raw `Vector3.Distance`, not `agent.remainingDistance` — the latter defaults to 0 before `SetDestination` is ever called, which causes the agent to falsely latch into "attack" on the very first frame regardless of actual distance. Don't switch back to `remainingDistance` for the entry/exit check.
- `animator.speed` is set to `agent.velocity.magnitude / AnimatedWalkSpeed` every frame while chasing (not a fixed ratio), because `NavMeshAgent` accelerates/brakes instead of moving at a flat speed — a fixed ratio causes visible foot-sliding during those ramps.
- The Animator Controller (`Monster02_AC_InPlace.controller`) has an **Any State → Attack01** transition (gated on the `Attack` trigger, not the `IsAttacking` bool) so attacks can fire from any current state, not just from Walk. All bool-driven transitions (Idle↔Walk, Walk↔Attack, Attack→Idle) have `Has Exit Time` off and a short (0.08s) transition duration.
- `PlayerFire.cs`'s raycast uses `hit.collider.GetComponentInParent<FireReceiver>()`, not `GetComponent`, since an enemy's collider can live on a different child object than its `FireReceiver` script.

### Footstep sounds (`FootstepSounds.cs`, `Assets/1A_Scripts/`)
- Generic — tracks its own movement via position delta, so it works on the player (`Rigidbody`) or an enemy (`NavMeshAgent`) with no changes.
- Speed is sampled over a ~0.1s window, not per-frame — a `Rigidbody` only actually moves during `FixedUpdate`, so computing speed from a single `Update()` frame's position delta produces spiky, inflated readings.
- One `AudioClip[]` array holds both jump and footstep sounds by convention: indices `0-4` are jump sounds, `5-24` are footsteps. Don't reorder the array without updating `JumpClipCount`.

### Keys / puzzle managers (`Keys.cs`, `IKeyCollector.cs`, `KeyColor.cs`)
- One shared `Keys` prefab/script works across all levels — it looks up whichever level's puzzle manager is present in the scene (`LevelOnePuzzleManager` / `LevelTwoPuzzleManager`) via the `IKeyCollector` interface instead of hardcoding a scene-name check.
- Adding a new level's key support means: make that level's puzzle manager implement `IKeyCollector`, nothing else.
