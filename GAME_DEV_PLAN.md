# Hanging on the Line — Game Dev Plan
## เจตจำนงสุดท้ายของไม้แขวนพลาสติก

> **Team:** Dev A (Systems/C#) · Dev B (Scenes/Unity Editor) · Artist (Photoshop / Illustrator)
> **Scope:** All 5 chapters + 3 endings (Normal, Bad, Secret)
> **Deadline:** None — work through phases in order
> **Format:** 2.5D — top-down movement plane (X/Y) plus a real height axis (Z) for rods, shelves, hooks, and jump arcs. Camera is angled/perspective, not flat orthographic. See `HangingOnTheLine_GDD_Full.md` §3 for the mechanics note.

---

## Quick Reference

| Role | Responsibilities |
|------|-----------------|
| **Dev A** | All C# scripts — systems, AI, mechanics, inventory, QTE |
| **Dev B** | Unity Editor — scene assembly, prefab wiring, audio, animation, UI |
| **Artist** | Photoshop/Illustrator → PNG at target PPU → place in `Assets/Sprites/` |

---

## Phase 0 — Foundation
> **Goal:** Core systems and project skeleton. Nothing can be built until this is done.

### Dev A
- [ ] Write `NoiseEmitter.cs` — reads `PlayerController2D` velocity → outputs `noiseRadius`
- [ ] Write `AudioRadiusDetector.cs` — `noiseRadius > detectionRadius` → suspicion (Mochi)
- [ ] Write `NoiseThresholdDetector.cs` — `noiseRadius > wakeThreshold` → suspicion (Pudding)
- [ ] Write `ProximityDetector.cs` — contact zone trigger → instant reset (Dusty)
- [ ] Add `IEnemyDetector` interface; refactor `VisionCone2D` to implement it
- [ ] Extend `SaveSystem.cs` with full inventory API (see Inventory section below)

### Dev B
- [ ] Create `Bootstrap.unity` scene — persistent managers, SaveSystem init
- [ ] Set up Physics2D layers: **Player, Enemy, Hookable, Trigger, Ground**
- [ ] Set up Tags: **Player, Enemy, HookPoint, ItemPickup, NoiseTrigger, ItemTier1, ItemTier2, ItemTier3**
- [ ] Configure URP 2D pipeline (needed for Momo glow via Light2D)
- [ ] Build folder structure (see Project Structure section below)
- [ ] Create base prefabs: Bendy, Momo (placeholder sprites OK for now)

### Artist
- [ ] **Priority:** Deliver Bendy (5 poses) + Momo (6 glow states × 2–3 frames each) — these unlock all playtesting
- [ ] Fill gaps in existing bedroom assets

**→ Gate: Ch1 assembly starts only after Phase 0 is complete.**

---

## Phase 1 — Chapter 1: Bedroom *(Tutorial)*
> **Enemy:** Pudding | **Teaches:** All 3 core skills (Quiet Hang, Hook, Snap Spring)

### Dev A
- [ ] Configure `EnemyFSM2D` + `NoiseThresholdDetector` for Pudding
- [ ] Verify `HookGrapple2D.cs` — hook-point on curtain rod above Bed
- [ ] Verify `SnapDash2D.cs` — full-charge leap clears Bed zone
- [ ] Verify `QuietHang2D.cs` — freeze + zeroes noise output
- [ ] Write `MomoController.cs` — 6-state glow FSM (Gold/GoldDimming/Orange/RedSteady/RedFlashing/White)

### Dev B
- [ ] Assemble Ch1_Bedroom scene: Wardrobe(A) → Desk(B) → Bed(C) → Windowsill(D)
- [ ] Place Pudding prefab (sleeping, centre of Bed zone) + noise wake trigger
- [ ] Place Push Pin (Zone B, Desk) and Masking Tape (Zone A, Wardrobe floor)
- [ ] Wire MomoController — follows Bendy, glow updates from EnemyFSM2D state
- [ ] Story trigger at Windowsill: sunlight shaft → Momo white glow → chapter exit

### Artist
- [ ] Pudding sprites: sleeping (1–2 frames) + awake/crying (3–4 frames)
- [ ] Bedroom environment: all furniture complete

**→ Gate: All 3 core mechanics (Quiet Hang, Hook, Snap Spring) must be playable before Ch2 starts.**

---

## Phase 2 — Chapter 2: Living Room
> **Enemy:** Mochi (audio-radius detection) | **New:** Paperclip strategic decision, Secret Part 1

### Dev A
- [ ] Configure `EnemyFSM2D` + `AudioRadiusDetector` for Mochi
- [ ] Distraction cup: Tier 1 environmental interaction — knock to distract Mochi
- [ ] Paperclip pickup: Tier 2 consumable, added to SaveSystem inventory
- [ ] Button Battery pickup: Tier 3 secret part — `SaveSystem.SetHasBattery(true)`
- [ ] Write `ChapterManager.cs` — Ch1→Ch2 inventory persistence on scene load/unload

### Dev B
- [ ] Assemble Ch2_LivingRoom: Entry(A) → Sofa+Table(B) → TV Cabinet(C)
- [ ] Mochi patrol oval waypoints (Sofa + Coffee Table zone)
- [ ] Place Paperclip (Zone B, Coffee Table), Button Battery (Zone C, behind TV)
- [ ] TV story trigger: scripted pause when Bendy reaches TV → Momo warm gold
- [ ] Source + assign `bgm_ch2` AudioSource (looping, free library — wonder/hopeful tone)

### Artist
- [ ] Mochi sprites: idle (2), patrol (4), ears-up suspicious (1), chase (4–6)
- [ ] Living Room environment

---

## Phase 3 — Chapter 3: Kitchen
> **Enemy:** Dusty (proximity contact) | **New:** Modeling Clay, ambient voice line, Secret Part 2

### Dev A
- [ ] Configure Dusty as hazard: `ProximityDetector` → instant chapter reset (no FSM states)
- [ ] Fridge Magnet: Tier 1 interaction — redirect Dusty direction sensor ~2s
- [ ] Modeling Clay: Tier 2 pickup — when active, multiplies NoiseEmitter output by 0 (fools Dusty + Mochi)
- [ ] Broken Toy Chip: Tier 3 secret — `SaveSystem.SetHasChip(true)`
- [ ] Write `CraftingSystem.cs` — after each Tier 3 pickup, check `HasBattery && HasChip && HasWire`

### Dev B
- [ ] Assemble Ch3_Kitchen: Doorway(A) → Counter+Sink(B) → Dining+BackDoor(C)
- [ ] Dusty tight patrol loop waypoints (Counter + Sink)
- [ ] Ch3 ambient voice trigger: AudioSource near Fridge, plays on Bendy entering Zone B
  - Voice line: *"...things that aren't useful anymore... you just throw them away."*
  - Source: ElevenLabs TTS → processed in Audacity (slight reverb + low-pass = phone/radio feel)
- [ ] Momo glow dim on voice trigger: not danger — quieter gold, sadness cue
- [ ] Back door exit: stiff open animation (first effortful exit)

### Artist
- [ ] Dusty sprites: patrol (2–4 rim-light frames) + disabled (1)
- [ ] Kitchen environment + Fridge prop

---

## Phase 4 — Chapter 4: Laundry Room
> **Enemies:** Mochi + Dusty simultaneously | **New:** Dual-enemy, Secret Part 3, crushed hanger story beat

### Dev A
- [ ] Mochi: stationary `AudioRadiusDetector` in Zone C (no patrol — waiting and listening)
- [ ] Dusty: wider patrol loop (`ProximityDetector`, Washing Machine + Dryer area)
- [ ] Copper Wire pickup: Tier 3 secret — `SaveSystem.SetHasWire(true)` → triggers `CraftingSystem.CheckCraft()`
- [ ] CraftingSystem fires: all 3 flags true → `MomoController.SetState(White, 2f)` → `SetHasHackingItem(true)`
- [ ] Optional story moment: 2s Quiet Hang over crushed hanger → Momo brief white flicker

### Dev B
- [ ] Assemble Ch4_LaundryRoom: Entry(A) → Washing Machine(B) → Linen Shelf(C)
- [ ] Crushed hanger prop: Zone B, visible from Zone A doorway (emotional beat)
- [ ] Copper Wire: Zone B, behind Washing Machine (inside Dusty loop)
- [ ] Exit vent: above Linen Shelf — height placement requires small hop or hook
- [ ] Source + assign `bgm_ch4` (heavy, damp, defiant tone)

### Artist
- [ ] Laundry Room environment
- [ ] Crushed hanger prop (flat, old, half-buried)
- [ ] Copper Wire item sprite

---

## Phase 5 — Chapter 5: Hallway + All 3 Endings
> **Enemy:** Shin (vision cone + reach zone) | **New:** QTE, all 3 endings

### Dev A
- [ ] Configure `EnemyFSM2D` + `VisionCone2D` for Shin (VisionCone2D already exists)
- [ ] Shin reach zone: separate `ProximityDetector` at floor level — activates on crouch (every 8s)
- [ ] Shin phone distraction: Hacking Item terminal interaction → 6s walk-away
- [ ] Coat hook camouflage: if Bendy is `isQuiet` + near coat hook tag → Shin's suspicion stays 0 (AI furniture check)
- [ ] Write `QTEController.cs`: 3-beam laser, SPACE timing window, adaptive width (`TotalDeaths >= 10`)
- [ ] All 3 ending scene transitions: clear QTE → Normal, miss beam → Bad, HackingItem used → Secret

### Dev B
- [ ] Assemble Ch5_Hallway: Hallway(A) → Laser Corridor(B) → Garden(C)
- [ ] Shin patrol waypoints (slow, curious roaming; scripted crouch every 8s)
- [ ] Wall coat hooks: tagged `HookPoint` — Bendy can Quiet Hang here as camouflage
- [ ] Hacking terminal: Zone A, near coat rack, behind Shin's patrol path
- [ ] Assemble Ending_Normal, Ending_Bad, Ending_Secret cutscene scenes
- [ ] Secret Ending garden: warm afternoon light, clothesline composition

### Artist
- [ ] Shin sprites: walking (4–6), crouching (2), hand reaching (1)
- [ ] Hallway + Garden environment
- [ ] Clothesline + second hanger (Secret Ending)

---

## Phase 6 — Polish & Accessibility

### Dev A
- [ ] QTE adaptive timing: verify `TotalDeaths >= 10 → timing window *= 1.5f` (silent, no UI)
- [ ] `SaveSystem.ClearInventory()` called on New Game
- [ ] Dusty proximity audio: `AudioSource.pitch` driven by `ProximityDetector.distance`
- [ ] Snap Spring charge audio scales with hold duration
- [ ] Full playthrough smoke test: all 3 endings reachable from a clean save

### Dev B
- [ ] Source all remaining SFX (full list in PRE_DEV_ESSENTIALS.md §4)
- [ ] Source all 8 BGM tracks; assign looping AudioSources per scene
- [ ] Chapter BGM transitions: fade out → silence → fade in new chapter BGM
- [ ] Momo wing-flap animation speed scales with danger state (faster = more urgent)
- [ ] Ending cutscene timing tuning

### Artist
- [ ] Final consistency pass — all sprites match bedroom reference style
- [ ] Secret Ending garden warmth — ensure light/composition reads as "tender, magical"

---

## Systems Architecture

### Existing Scripts (brownfield — review before touching)

| Script | Status | Notes |
|--------|--------|-------|
| `PlayerController2D.cs` | ✅ Complete | WASD, SetQuiet(), detectionMultiplier exposed |
| `EnemyFSM2D.cs` | ✅ Complete | Generic FSM — reads `IEnemyDetector.suspicionLevel` after refactor |
| `VisionCone2D.cs` | ✅ Complete | Shin's detector — implement `IEnemyDetector` |
| `HookGrapple2D.cs` | ⚠️ Verify | Review for correctness |
| `SnapDash2D.cs` | ⚠️ Verify | Review for correctness |
| `QuietHang2D.cs` | ⚠️ Verify | Review for correctness |
| `SaveSystem.cs` | ⚠️ Extend | Add full inventory API (see below) |
| `GameManager.cs` | ⚠️ Review | Clarify scope overlap with ChapterManager |
| `HUD.cs` | ⚠️ Review | Momo replaces most UI — strip down what's left |
| `MainMenuController.cs` | ✅ Likely done | — |
| `CameraFollow2D.cs` | ✅ Done | Already in use |

### New Scripts (Dev A build order)

```
1. NoiseEmitter.cs              — player noise radius from velocity
2. AudioRadiusDetector.cs       — Mochi detection
3. NoiseThresholdDetector.cs    — Pudding detection
4. ProximityDetector.cs         — Dusty contact
5. MomoController.cs            — glow state machine (UI replacement)
6. ChapterManager.cs            — scene load + inventory persistence
7. CraftingSystem.cs            — 3-part crafting gate
8. QTEController.cs             — laser gauntlet, adaptive timing
```

### Detector Component Pattern

```csharp
public interface IEnemyDetector {
    float suspicionLevel { get; }   // 0.0 → 1.0
}

// Wiring per enemy:
// Shin   → VisionCone2D     (implements IEnemyDetector)
// Mochi  → AudioRadiusDetector
// Pudding→ NoiseThresholdDetector
// Dusty  → ProximityDetector (bypasses FSM — instant reset on contact)
```

---

## Inventory Architecture

### SaveSystem Extensions

```csharp
// Tier 2 — Consumables
KEY "has_paperclip"             bool  SetHasPaperclip(bool v)
KEY "used_paperclip_as_lockpick"bool  UsePaperclipAsLockpick()   ← consumes paperclip + logs use
KEY "used_paperclip_as_jam"     bool  UsePaperclipAsJam()        ← consumes paperclip + logs use
KEY "has_masking_tape"          bool  SetHasMaskingTape(bool v)
KEY "has_modeling_clay"         bool  SetHasModelingClay(bool v)

// Tier 3 — Secret Parts (permanent, never consumed)
KEY "has_battery"               bool  SetHasBattery(bool v)
KEY "has_chip"                  bool  SetHasChip(bool v)
KEY "has_wire"                  bool  SetHasWire(bool v)

// Crafted
KEY "has_hacking_item"          bool  SetHasHackingItem(bool v)

// Accessibility
KEY "total_deaths"              int   AddDeath() / GetTotalDeaths()
```

### Three-Tier Rules

| Tier | Items | Pickup? | Saved? | Consumed on use? |
|------|-------|---------|--------|-----------------|
| 1 — Environmental | Push Pin, Fridge Magnet, Cup | No | No | N/A (scene object) |
| 2 — Consumable | Masking Tape, Paperclip, Modeling Clay | Yes | PlayerPrefs bool | Yes — key → false |
| 3 — Secret Parts | Battery, Chip, Wire | Yes | PlayerPrefs bool | Never — permanent |
| Crafted | Hacking Item | Auto on craft | PlayerPrefs bool | No (single use in Ch5) |

### Paperclip Decision (Strategic Resource)
The Paperclip is the game's primary resource trade-off:
- **Use as lockpick in Ch2** → `UsePaperclipAsLockpick()` — opens an optional path in Ch2, gone for Ch3+
- **Save for Dusty jam in Ch3/Ch4** → `UsePaperclipAsJam()` — disables Dusty 3–4s, gone after use
- Both uses are tracked separately so the game can differentiate later (dialogue, analytics)

---

## Project Structure (Full Folder Tree)

```
Assets/
├── Scenes/
│   ├── Bootstrap.unity
│   ├── MainMenu.unity
│   ├── Ch1_Bedroom.unity
│   ├── Ch2_LivingRoom.unity
│   ├── Ch3_Kitchen.unity
│   ├── Ch4_LaundryRoom.unity
│   ├── Ch5_Hallway.unity
│   ├── Ending_Normal.unity
│   ├── Ending_Bad.unity
│   └── Ending_Secret.unity
│
├── Scripts/
│   ├── Player/
│   │   ├── PlayerController2D.cs  (EXISTS)
│   │   ├── NoiseEmitter.cs        (NEW)
│   │   ├── HookGrapple2D.cs       (EXISTS)
│   │   ├── SnapDash2D.cs          (EXISTS)
│   │   └── QuietHang2D.cs         (EXISTS)
│   ├── Enemies/
│   │   ├── EnemyFSM2D.cs          (EXISTS — shared FSM base)
│   │   ├── VisionCone2D.cs        (EXISTS — refactor to IEnemyDetector)
│   │   ├── AudioRadiusDetector.cs (NEW)
│   │   ├── NoiseThresholdDetector.cs (NEW)
│   │   └── ProximityDetector.cs   (NEW)
│   ├── Systems/
│   │   ├── SaveSystem.cs          (EXISTS — extend)
│   │   ├── GameManager.cs         (EXISTS — review)
│   │   ├── ChapterManager.cs      (NEW)
│   │   ├── CraftingSystem.cs      (NEW)
│   │   ├── QTEController.cs       (NEW)
│   │   └── MomoController.cs      (NEW)
│   └── UI/
│       ├── HUD.cs                 (EXISTS — review/strip)
│       └── MainMenuController.cs  (EXISTS)
│
├── Prefabs/
│   ├── Characters/    (Bendy, Momo, Pudding, Mochi, Dusty, Shin)
│   ├── Items/
│   │   ├── Tier1/     (Push Pin, Fridge Magnet, Distraction Cup)
│   │   ├── Tier2/     (Masking Tape, Paperclip, Modeling Clay)
│   │   └── Tier3/     (Battery, Chip, Wire)
│   └── UI/
│
├── Sprites/
│   ├── Characters/    (Bendy/, Momo/, Pudding/, Mochi/, Dusty/, Shin/)
│   ├── Bedroom/       (PARTIALLY EXISTS)
│   ├── LivingRoom/
│   ├── Kitchen/
│   ├── LaundryRoom/
│   ├── Hallway/
│   └── Items/
│
├── Audio/
│   ├── BGM/
│   ├── SFX/
│   └── Ambient/
│
├── Animations/        (Bendy/, Momo/, Pudding/, Mochi/, Dusty/, Shin/)
└── Settings/          (EXISTS — URP config)
```

---

## Art Asset Checklist

### Characters

| Asset | Frames | Status |
|-------|--------|--------|
| Bendy — idle | 1–2 | [ ] |
| Bendy — walk | 4 | [ ] |
| Bendy — hooked/hanging | 1 | [ ] |
| Bendy — snap charge | 2–3 | [ ] |
| Bendy — snap release | 2–3 | [ ] |
| Momo — gold (safe) | 2 | [ ] |
| Momo — gold dimming (caution) | 2 | [ ] |
| Momo — orange (suspicious) | 2 | [ ] |
| Momo — red steady (chase) | 2–3 | [ ] |
| Momo — red flashing + wing-flap (imminent) | 4–6 | [ ] |
| Momo — white (joy) | 2–3 | [ ] |
| Pudding — sleeping | 1–2 | [ ] |
| Pudding — awake/crying | 3–4 | [ ] |
| Mochi — idle | 2 | [ ] |
| Mochi — patrol | 4 | [ ] |
| Mochi — ears-up suspicious | 1 | [ ] |
| Mochi — chase | 4–6 | [ ] |
| Dusty — patrol (rim lights) | 2–4 | [ ] |
| Dusty — disabled | 1 | [ ] |
| Shin — walking | 4–6 | [ ] |
| Shin — crouching | 2 | [ ] |
| Shin — hand reaching | 1 | [ ] |

### Special Props

| Asset | Status |
|-------|--------|
| Crushed hanger (Ch4 story beat) | [ ] |
| Clothesline + second hanger (Secret Ending) | [ ] |

---

## Audio Checklist

### BGM (all from free libraries)

| Track | Tone | Status |
|-------|------|--------|
| Ch1 Bedroom | Curious, light | [ ] |
| Ch2 Living Room | Wonder, hopeful | [ ] |
| Ch3 Kitchen | Uneasy, off-kilter | [ ] |
| Ch4 Laundry Room | Heavy, defiant | [ ] |
| Ch5 Hallway | Tense, quiet dread | [ ] |
| Ending Normal | Bittersweet | [ ] |
| Ending Bad | Minimal / silence | [ ] |
| Ending Secret | Warm, tender | [ ] |

### SFX (all from free libraries)

| Sound | Status |
|-------|--------|
| footstep_soft (walk) | [ ] |
| footstep_loud (run) | [ ] |
| hook_attach | [ ] |
| hook_swing | [ ] |
| snap_charge (rising) | [ ] |
| snap_release | [ ] |
| pudding_cry | [ ] |
| mochi_meow_far | [ ] |
| mochi_meow_near | [ ] |
| dusty_hum_normal | [ ] |
| dusty_hum_close (higher pitch) | [ ] |
| dusty_disabled | [ ] |
| shin_stomp | [ ] |
| shin_hum | [ ] |
| laser_warning | [ ] |
| laser_hit | [ ] |
| item_pickup | [ ] |
| crafting_merge | [ ] |
| chapter_exit | [ ] |

### Ambient / Narrative

| Sound | How | Status |
|-------|-----|--------|
| Ch3 voice line — *"...things that aren't useful anymore..."* | ElevenLabs TTS → Audacity (reverb + low-pass) | [ ] |
| Washing machine hum (Ch4 BG) | Free library loop | [ ] |
| Outdoor ambience — flat/grey (Normal Ending) | Free library | [ ] |
| Outdoor ambience — warm afternoon (Secret Ending) | Free library | [ ] |

---

## Momo Glow Reference

| State | Colour | When |
|-------|--------|------|
| Safe | Warm gold | No enemy in range |
| Caution | Gold dimming → orange | Enemy nearby |
| Suspicious | Orange | Enemy in Suspicious state |
| Chase | Red steady | Enemy in Chase state |
| Imminent | Red flashing + frantic flap | Capture about to happen |
| Joy | White | Chapter clear / crafting event / story milestone |

**Design rule:** Momo IS the UI. No health bar. No detection meter. Players read danger from Momo only.

---

*Plan generated from 14-round deep interview · Spec: `.omc/specs/deep-interview-handon2d-devplan.md`*
