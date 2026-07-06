# Hanging on the Line — Dev Essentials

> **Format:** 2.5D — top-down X/Y movement plane plus a real height (Z) axis for rods, shelves, coat hooks, and jump arcs; angled/perspective camera instead of flat orthographic. Scripts named `*2D` (e.g. `PlayerController2D`, `CameraFollow2D`) predate this change and will need a height/Z component added — not a rewrite of the movement plane.

## 1. Core Scripts

### Player
| Script | Responsibility |
|--------|---------------|
| `PlayerController.cs` | WASD movement, speed tracking |
| `NoiseEmitter.cs` | Emit noise radius based on movement speed |
| `HookSystem.cs` | E key — attach to valid anchor points, swing |
| `SnapSpring.cs` | SPACE hold/release — charge → physics impulse |
| `QuietHang.cs` | Detect zero velocity → toggle camouflage state |

### Enemies
| Script | Responsibility |
|--------|---------------|
| `EnemyFSM.cs` | Base FSM: Patrol → Suspicious → Chase → Reset |
| `PuddingAI.cs` | Noise threshold wake; erratic move; 4s sleep reset |
| `MochiAI.cs` | Audio radius detection; ch2 patrol oval; ch4 stationary wait |
| `DustyHazard.cs` | Proximity contact trigger; fixed patrol path; no FSM |
| `ShinAI.cs` | Vision cone + reach zone; 8s crouch cycle; phone distraction |

### Systems
| Script | Responsibility |
|--------|---------------|
| `InventorySystem.cs` | Tier 2 consumables list; Tier 3 bool flags |
| `CraftingSystem.cs` | Check hasBattery + hasChip + hasWire → grant HackingItem |
| `MomoController.cs` | Glow colour state machine; animation trigger |
| `QTEController.cs` | 3-beam laser sequence; SPACE timing window; death-count check |
| `ChapterManager.cs` | Scene load/unload; inventory persistence across chapters |
| `DeathTracker.cs` | PlayerPrefs TotalDeaths; QTE window auto-extend at 10+ |

---

## 2. PlayerPrefs Keys

```
TotalDeaths       int   — cumulative death count (QTE accessibility trigger at 10)
hasBattery        bool  — Secret Part Ch2
hasChip           bool  — Secret Part Ch3
hasWire           bool  — Secret Part Ch4
hasHackingItem    bool  — crafted from 3 parts
hasPaperclip      bool  — consumable; false after use
hasMaskingTape    bool  — consumable; false after use
hasModelingClay   bool  — consumable; false after use
```

---

## 3. Numbers to Lock Before Coding

| Parameter | Value | Notes |
|-----------|-------|-------|
| Walk noise radius | TBD | Below Mochi detection threshold |
| Run noise radius | TBD | Above Mochi detection threshold |
| Mochi detection radius | TBD | Triggers Suspicious state |
| Pudding noise threshold | TBD | Single float value |
| Snap Spring force min | TBD | Tap = small hop |
| Snap Spring force max | TBD | 5s charge = long leap |
| Dusty disable duration | 3–4s | Paperclip jam |
| Shin crouch cycle | 8s | Reach zone activates during crouch |
| Shin phone distraction | 6s | Hacking Item window |
| Shin vision cone angle | TBD | Degrees; short-medium range |
| QTE beam speed (1/2/3) | TBD | Increasing per beam |
| QTE timing window normal | TBD | Seconds |
| QTE timing window extended | TBD | Auto at 10+ deaths |
| Pudding sleep reset | 4s | After last noise event |
| Enemy chase lost timer | 3s | Returns to Patrol |

---

## 4. Inventory Tiers

```
Tier 1 — Environmental (no pickup, proximity use only)
  Push Pin        Ch1-B  Desk      Wall foothold
  Fridge Magnet   Ch3-B  Fridge    Redirect Dusty 2s
  Distraction Cup Ch2-B  Shelf     Knock to distract Mochi

Tier 2 — Carried Consumables (single-use, persist across chapters)
  Masking Tape    Ch1-A  Wardrobe  Silence a surface
  Paperclip       Ch2-B  Table     Jam Dusty 3-4s OR lockpick (choose one)
  Modeling Clay   Ch3-B  Counter   Reduce Bendy mass/vibration (fools Dusty+Mochi)

Tier 3 — Secret Parts (permanent, never consumed)
  Button Battery  Ch2-C  Behind TV
  Broken Toy Chip Ch3-B  Drawer inside Dusty loop
  Copper Wire     Ch4-B  Behind Washing Machine
  → All 3 collected: TriggerCraftingEvent() → add HackingItem to Tier 2
```

---

## 5. Momo Glow States

| State | Colour | Trigger |
|-------|--------|---------|
| Safe | Warm gold | No enemy in range |
| Caution | Gold → orange dim | Enemy nearby |
| Suspicious | Orange | Enemy in Suspicious state |
| Chase | Red steady | Enemy in Chase state |
| Imminent | Red flashing + wing flap | Capture about to happen |
| Joy | White | Chapter clear / story milestone / crafting event |

---

## 6. Enemy Detection Matrix

| Enemy | Type | Chapters | Bypassed by |
|-------|------|----------|-------------|
| Pudding | Noise threshold | 1 | Quiet Hang, Hook over bed, Snap Spring |
| Mochi | Audio radius | 2, 4 | Quiet Hang, distraction, perimeter creep |
| Dusty | Proximity contact | 3, 4 | Timing gap, Paperclip jam, Hook to shelf height, Modeling Clay |
| Shin | Vision cone + reach zone | 5 | Quiet Hang on coat hooks, Snap Spring over hand, Hacking Item |

---

## 7. Scene List

```
Bootstrap          — persistent managers, PlayerPrefs init
Ch1_Bedroom        — Pudding, 3 skills tutorial
Ch2_LivingRoom     — Mochi, Paperclip decision
Ch3_Kitchen        — Dusty, ambient audio trigger, Modeling Clay
Ch4_LaundryRoom    — Mochi + Dusty, crushed hanger moment
Ch5_Hallway        — Shin, QTE or Hacking bypass
Ending_Normal      — QTE cleared
Ending_Bad         — QTE failed
Ending_Secret      — All parts + Hacking Item used
```

---

## 8. Crafting Trigger (Ch4→Ch5 or whenever 3rd part collected)

```csharp
if (hasBattery && hasChip && hasWire) {
    TriggerCraftingEvent();     // brief animation on Bendy
    MomoController.SetState(MomoState.Joy);  // white glow 2s
    Inventory.Add(ItemID.HackingItem);
}
```

---

## 9. QTE Sequence (Ch5 Zone B)

```
Enter Zone B
  → Beam 1 sweeps (slowest) — SPACE to spring over
  → Beam 2 sweeps (faster)  — SPACE to spring over
  → Beam 3 sweeps (fastest) — SPACE to spring over
  → All cleared → Normal Ending
  → Any hit     → freeze → Shin appears → Bad Ending
```

Adaptive: if `TotalDeaths >= 10` → widen timing window silently, no UI.

---

## 10. Build Order (Recommended)

1. **Player movement + NoiseEmitter** — foundation of everything
2. **Ch1 Bedroom** — Pudding AI, Hook, Snap Spring, Quiet Hang
3. **Momo glow system** — replaces all UI; needed before playtesting
4. **Inventory + ChapterManager** — persistence across scenes
5. **Ch2** — Mochi FSM, Paperclip item
6. **Ch3** — Dusty hazard, Modeling Clay, ambient audio trigger
7. **Ch4** — dual enemy, Crafting system
8. **Ch5** — Shin AI, QTE controller, all 3 endings
9. **Polish** — accessibility (QTE adaptive), chapter transitions, audio mix
