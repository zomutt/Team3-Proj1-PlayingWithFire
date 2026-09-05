# Playing With Fire

Unity 6000.3.17f1 game project (team course project, multiple levels).

## Structure

- `Assets/1A_Scripts/` — gameplay C# scripts, namespace `_1A_Scripts` (subfolders get sub-namespaces, e.g. `_1A_Scripts.Level1Puzzle_Scripts`, `_1A_Scripts.Managers`).
- `Assets/1A_Scenes/Levels/` — level scenes (`LevelOne.unity`, `LevelTwo.unity`, ...).
- `Assets/1A_Prefabs/` — prefabs, grouped by feature (e.g. `Hint/`, `Keys/`).
- Third-party/imported asset packs live in their own top-level folders (e.g. `AllSkyFree`, `Matthew Guz`, `CaptainCatSparrow`) — don't restructure these.

## Conventions

- Key pickups are unified across levels: `Keys.cs` detects the active level's puzzle manager (`LevelOnePuzzleManager.Instance` / `LevelTwoPuzzleManager.Instance`) and hands off via the `IKeyCollector` interface (`CollectKey(string color)`), implemented by each level's puzzle manager. Don't reintroduce per-level duplicate key scripts.
- Key color is the `KeyColor` enum, not a string, at the point of pickup config.
- Prefer editing existing scripts/prefabs over adding new duplicated ones per level — this project had per-level duplication (e.g. old `KeysLevelTwo.cs`) that was deliberately consolidated.

## Branches

Team members work on personal branches (e.g. `Comer-*`, `Pedro-*`) and merge into `main` via PR.
