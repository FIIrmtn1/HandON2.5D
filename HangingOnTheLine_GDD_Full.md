# Game Design Document (GDD) — Full Detail
## เจตจำนงสุดท้ายของไม้แขวนพลาสติก (Hanging on the Line)

> Version 2.0 — Generated from deep-interview validation session 2026-06-29
> Status: Design-complete. Ready for Unity implementation and art production.

---

## Table of Contents
1. [Project Overview](#1-project-overview)
2. [Core Theme & Narrative](#2-core-theme--narrative)
3. [Core Mechanics & Controls](#3-core-mechanics--controls)
4. [Characters](#4-characters)
5. [Stealth System & Enemy AI](#5-stealth-system--enemy-ai)
6. [Chapter Storyboard](#6-chapter-storyboard)
7. [Scene Design — Zone Layouts](#7-scene-design--zone-layouts)
8. [Puzzle Design — Multi-Path Solutions](#8-puzzle-design--multi-path-solutions)
9. [Item System](#9-item-system)
10. [Crafting System](#10-crafting-system)
11. [QTE: Laser Gauntlet](#11-qte-laser-gauntlet)
12. [Endings](#12-endings)
13. [Progression & Accessibility](#13-progression--accessibility)

---

## 1. Project Overview

| Field | Value |
|-------|-------|
| **Game Name** | Hanging on the Line |
| **Thai Title** | เจตจำนงสุดท้ายของไม้แขวนพลาสติก |
| **Genre** | 2.5D Top-Down Stealth Puzzle-Adventure |
| **Platform** | PC — Keyboard & Mouse |
| **Engine** | Unity (C#) |
| **Art Style** | 2D Kawaii / Soft Matte Minimalist (Pastel tones) |
| **Tone** | Bittersweet. Lighthearted surface, emotional depth beneath. |
| **Play Length** | ~45–90 minutes (single run) |

---

## 2. Core Theme & Narrative

### The Disillusionment Arc

**Hanging on the Line** is not a game about escaping. It is a game about wanting to escape — and what happens when you do.

Bendy, a plastic hanger brought to life by lightning, stares at sunlight through a window and decides the outside world must be magical. Over five chapters and five rooms, they journey toward that dream. But the further Bendy goes, the more the dream frays.

By the final chapter, Bendy has learned that the outside world is grey, indifferent, and loud. The adventure was the richest thing in Bendy's life — not the destination.

**The Thematic Climax:** In Chapter 5, Bendy defeats the final obstacle (Shin, an enormous boy who would "rescue" them) by doing exactly one thing: freezing on a wall hook and pretending to be a normal hanger. The best solution to the final boss is to be what Bendy started as. The journey leads home.

### Narrative Summary

| Chapter | Location | Story Moment |
|---------|----------|-------------|
| 1 | Bedroom | **Dream Ignites** — sunlight through the window. Bendy decides to escape. |
| 2 | Living Room | **Peak Hope** — Bendy sees a TV showing exciting places in the world. The dream is largest here. |
| 3 | Kitchen | **The Crack** — ambient audio: *"things that aren't useful anymore, you just throw them away."* Momo's glow dims. |
| 4 | Laundry Room | **Defiant Stubbornness** — Bendy finds a crushed hanger under the washing machine. Their own possible fate. They press on not from hope, but from stubbornness. |
| 5 | Front Door / Garden | **Confrontation & Resolution** — Shin guards the exit. Three endings wait beyond the front door. |

---

## 3. Core Mechanics & Controls

> **2.5D note:** Movement stays on a top-down plane (WASD = X/Y), but the world now carries a real height axis (Z) for surfaces like curtain rods, shelves, and coat hooks. Camera is angled/perspective rather than a flat orthographic top-down view, and characters/props read with depth (3D models or layered sprites) instead of flat 2D art. Hook, Snap Spring, and enemy reach/vision now reason about height, not just X/Y position.

### Movement
- **WASD** — standard top-down movement across the X/Y plane
- Movement speed affects noise output — faster movement = louder footsteps

### Quiet Hang (อยู่นิ่งๆ)
- **How:** Simply stop all movement (zero velocity)
- **Effect:** Bendy blends in as a normal hanging object. Sound-based enemies lose interest. Vision-based enemies may not notice if Bendy is near other hanging objects.
- **Key use:** Against Mochi (sound-based) and Shin (vision-based, especially near coat hooks)
- **Strategic depth:** This is the thematic mechanic — "be a hanger" is always a valid solution

### Hook (การเกี่ยวตะขอ)
- **Input:** E (contextual, near valid surfaces)
- **Effect:** Attaches Bendy's hook to edges, bars, curtain rods, shelf lips, coat racks
- **Uses:** Swing across gaps, traverse at shelf/rod height above floor level, bypass enemy patrol zones by moving up the height axis
- **Key use:** Over Pudding (Ch1), over Dusty's floor patrol (Ch3), over Shin's reach zone (Ch5)

### Snap Spring (การดีดตัว)
- **Input:** Hold SPACE to charge; release to launch
- **Charge duration:** 0–5 seconds (caps at 5s)
- **Effect:** Applies a physics impulse force proportional to charge duration — an arc across X/Y with a height (Z) component. Short tap = small hop. Full charge = long leap.
- **Key use:** Over the Bed in Ch1 (Path C), over Shin's hand in Ch5 (Path B), and critically — as the mechanic for the Ch5 laser QTE

---

## 4. Characters

### Bendy (Protagonist)

**Appearance:** A white plastic hanger with a rounded hook at the top. Small, lightweight, fragile-looking. Expressive eyes on the flat body.

**Personality:** Stubborn. Brave. Naïve at the start; weary by the end. Treats the escape as an adventure until reality makes that impossible.

**Arc:** Naïve dreamer → disillusioned idealist → contented simpleton.

**Voice:** Mute. All emotion conveyed through movement, Momo, and environmental storytelling.

**Core skill:** The "Quiet Hang" — Bendy's greatest power is looking exactly like what they are.

---

### Momo (Companion Moth)

**Appearance:** A small, softly glowing moth with translucent wings. Rests near Bendy when idle; flies around them when agitated.

**Personality:** Loyal beyond reason. Followed Bendy not from duty but from love. She is genuinely afraid for Bendy — not executing a warning script.

**Role:** Diegetic UI for danger. No health bar or detection meter appears on screen; Momo communicates everything.

**Glow Colour System:**

| Colour | Meaning |
|--------|---------|
| Warm gold | Safe. No enemy in detection range. |
| Gold dimming toward orange | Caution. Enemy nearby. |
| Orange | Enemy in Suspicious state — has heard or seen something. |
| Red (steady) | Enemy in Chase state. |
| Red (flashing + frantic wing-flap) | Capture imminent. |
| White | Joy. Chapter cleared, story milestone reached, or Secret Ending extended moment. |

**Key character note:** When Momo flaps frantically and glows red, she is scared *for Bendy*. This must come through in animation as erratic, urgent, and personal — not mechanical.

**Per-chapter presence:**
- Ch1: Gold → near-red near Pudding → white at Windowsill exit
- Ch2: Gold → orange near Mochi → white at TV moment + exit
- Ch3: Gold dimming slightly at "useless things" audio → orange near Dusty → white at exit
- Ch4: Dim gold → red (Dusty + Mochi) → very brief white flicker if Bendy pauses over crushed hanger
- Ch5: Gold → orange near Shin → white (extended hold) in Secret Ending

---

### Pudding (Baby — Chapter 1)

**Description:** Tiny. Asleep in the middle of the Bed zone. Innocent. Does not seek Bendy; reacts to noise.

**Detection type:** Sound (noise threshold). Wakes if player generates sound (running footsteps, falling objects, collision).

**Behaviour on wake:** Pudding cries, begins moving erratically with a widened detection radius. Not fast, but unpredictable direction.

**Reset:** Falls back asleep ~4 seconds after last noise event.

**Chapter role:** Tutorial enemy. Teaches the noise system and the value of silence. Low stakes.

**Emotional subtext:** A baby who sleeps — harmless if left undisturbed. A metaphor for the household Bendy is escaping: peaceful when not provoked.

---

### Mochi (Orange Cat — Chapters 2 & 4)

**Description:** A round, orange tabby cat. Playful expression even during the chase.

**Personality:** Playful cruelty. Mochi does not want to catch Bendy to end the game — Mochi wants to *play*. The chase is a game to her. This makes her simultaneously more charming and more horrifying.

**Detection type:** Hearing only. Mochi uses an audio radius — Bendy's movement speed determines footstep volume, which determines detection range.

**Audio signature:** Chirpy meows, soft at distance, increasing in frequency and volume as she closes in. The closing meows should feel almost endearing before they become alarming.

**Patrol (Chapter 2):** Loose oval around the Sofa + Coffee Table zone. Pauses occasionally to "listen" — brief idle state with ears up.

**Patrol (Chapter 4):** Stationed near the Exit Vent, Zone C. Does not patrol; waits and listens. Effectively guards the only exit.

**FSM:** Patrol → Suspicious (hears something; slows, rotates toward source) → Chase (confirmed sound; fast sprint)

---

### Dusty (Robot Vacuum — Chapters 3 & 4)

**Description:** A flat, disc-shaped robot vacuum. No face. Lights on the rim indicate status.

**Personality:** Merciless efficiency. No emotion. No adaptation. No negotiation. Cannot be fooled by behaviour — only by physical means (vent jam) or vertical escape.

**Detection type:** Proximity (contact/near-contact zone trigger). Does NOT use vision cones or sound detection.

**Audio signature:** Constant low motor hum. Pitch rises as it approaches Bendy (proximity-based pitch shift). The rising hum is the only warning.

**Patrol:** Fixed looping path. Consistent speed, no variation. Chapter 3: tight loop around Counter + Sink. Chapter 4: wider loop through Washing Machine + Dryer area.

**Interactions:**
- Paperclip into vent → 3–4 second disable (jam)
- Hook to upper shelf → Dusty cannot reach height; complete bypass
- Modeling Clay applied to Bendy → reduces Bendy's weight/vibration; Dusty proximity trigger not triggered

**Important:** Dusty has no chase state. If Bendy is within the contact zone, the round resets. There is no "seen and fleeing" phase with Dusty — it is a roaming hazard, not a reactive enemy.

---

### Shin (Young Boy — Chapter 5)

**Description:** A child, seen from Bendy's extremely low perspective as enormous. Giant hands, large feet that stomp. Curious, gentle face.

**Personality:** Oblivious power. Shin is not a villain. He loves the house and knows where everything belongs. He would cheerfully pick Bendy up and put them away — and to Bendy, that is indistinguishable from being caught.

**Detection type:** Vision cone (wide, short-medium range) + Reach zone (crouches to grab things on the floor; this is a separate proximity trigger at floor level).

**Patrol:** Slow, curious roaming sweeps through Zone A (Hallway). Crouches to examine floor items on a scripted cycle (~every 8 seconds). During crouch, his reach zone activates — this is the danger window AND the Snap Spring timing window.

**Audio:** Heavy, earthquake-like stomping (Bendy's scale). Occasional soft humming or singing. The contrast between the gentle sounds and the crushing scale is intentional.

**Key interaction with Hacking Item:** If Bendy activates the terminal hidden near the coat rack, Shin's phone buzzes. He straightens, turns away, and walks toward the kitchen to check it. Gap: ~6 seconds.

**Emotional subtext:** Shin is the system. He would "help" you right back into captivity. The only way to be free around him is to look like you were never leaving.

---

## 5. Stealth System & Enemy AI

### Detection Types Summary

| Enemy | Detection Method | Chapter |
|-------|-----------------|---------|
| Pudding | Sound (noise threshold) | 1 |
| Mochi | Hearing (audio radius) | 2, 4 |
| Dusty | Proximity (contact zone) | 3, 4 |
| Shin | Vision cone + reach zone | 5 |

### FSM States (Mochi & Shin)

| State | Trigger | Enemy Behaviour | Momo |
|-------|---------|----------------|------|
| **Patrol** | Default | Follows patrol path or waits | Gold |
| **Suspicious** | Detected Bendy ≤1 second | Slows, rotates toward source, searches briefly | Orange |
| **Chase** | Detected Bendy >1 second | Sprints toward last known position | Red + flap |
| **Reset** | Lost target ~3 seconds | Returns to Patrol; briefly searches origin | Orange fading to gold |

### Dusty Special Rules
Dusty operates as a **hazard object**, not an FSM enemy:
- No Patrol / Suspicious / Chase states
- Contact zone = instant game-over reset
- Can be temporarily disabled (Paperclip jam, 3–4 seconds)
- Can be permanently avoided (Hook to shelf height)

### Quiet Hang Detection Bypass
When Bendy is fully stationary:
- Mochi's audio radius shrinks to near-zero (Bendy makes no footstep sound)
- Shin's vision cone still active — but if Bendy is near coat hooks or hanging objects, Shin's AI treats Bendy as environmental furniture and does not trigger Suspicious state

---

## 6. Chapter Storyboard

### Chapter 1: Bedroom — *Dream Ignites*
**Enemy:** Pudding | **Tone:** Curiosity, wonder, pure beginning

**Opening:** Bendy hangs in the Wardrobe in darkness. Momo glows gold and rests on the clothes rail above. A thin beam of sunlight cuts across the floor from the Windowsill at the far end of the room.

**Story beat:** Bendy moves toward the light. As they reach the Windowsill, the sunlight falls warm on their hook. Outside: a garden, a street, clouds in a blue sky. Momo flutters around Bendy. Something becomes a decision. Bendy looks back once at the Wardrobe. Then forward. The adventure begins.

**Exit beat:** Bendy climbs through the window gap. Momo glows white. First chapter ends on optimism.

---

### Chapter 2: Living Room — *Peak Hope*
**Enemy:** Mochi | **Tone:** Wonder at its height, brief joy before the world begins to answer back

**Opening:** Bendy emerges onto the Living Room floor. Mochi is already circling the sofa, meowing softly to herself. The TV in the far corner is on — a travel programme showing distant coasts, busy markets, laughing faces.

**Story beat:** When Bendy reaches Zone C and the TV, there is a scripted pause — Bendy freezes in front of the screen. For a few seconds they watch. The places on screen are exactly what they imagined outside must be like. Momo glows warm gold. This is the peak of the dream.

**Transition:** The TV flickers. Mochi meows somewhere behind. Bendy climbs through the window gap beside the TV cabinet. The dream is still intact, but the world has started to respond.

---

### Chapter 3: Kitchen — *The Crack*
**Enemy:** Dusty | **Tone:** Unease, the first fracture, something heard that cannot be unheard

**Opening:** The Kitchen is functional and indifferent. Dusty whirs in its loop. The smell of cleaning fluid. Harsh overhead light.

**Story beat:** Somewhere near the Fridge — a phone on the counter, or a radio — ambient voices play: *"...things that aren't useful anymore... there's no point keeping them... you just throw them away."* The line is not about hangers. It was not meant for Bendy. But Bendy hears it. Momo's glow dims — still gold, but quieter.

**Visual note:** No dialogue box. No subtitle. The audio plays naturally as Bendy crosses Zone B. Players who are paying attention will hear it. Players who rush through may miss it entirely — and still feel that something is slightly wrong with the chapter's mood.

**Exit beat:** The back door is stiff. Bendy has to work to push through. For the first time, leaving the house feels effortful.

---

### Chapter 4: Laundry Room — *Defiant Stubbornness*
**Enemy:** Mochi + Dusty | **Tone:** Heaviness, dread, the decision to continue despite knowing

**Opening:** The Laundry Room is dim and damp. The smell of detergent. The machines are loud. The whole room vibrates slightly.

**Story beat:** In Zone B, visible immediately upon entering: a hanger. Old. Crushed flat. Half-buried under the Washing Machine, forgotten. Bendy stops. Momo's glow dims to a pale orange — not danger, but sadness. This is what Bendy could become. This is what this house does to things it no longer needs.

**The decision:** Bendy could turn back. The way to the Bedroom is clear. But Bendy doesn't turn back. They continue forward. Not because the dream is still alive. Because stubbornness is all that's left when hope has gone.

**Optional moment:** If Bendy stands still over the crushed hanger for ~2 seconds (Quiet Hang), Momo briefly glows white — flickering, uncertain. As if acknowledging that continuing anyway is its own kind of courage.

**Exit beat:** The exit vent requires a small jump or hook to reach. It feels like climbing out of something as much as climbing toward something.

---

### Chapter 5: Front Door / Garden — *Confrontation & Resolution*
**Enemy:** Shin | **Tone:** Finality, scale, the moment everything was for

**Opening:** The Hallway is quiet relative to the Laundry Room. Shin moves through it slowly, humming. The front door is visible at the far end of Zone B. Between Bendy and the door: Shin's presence, then a bank of laser sensors across the doorway.

**Story beat (diverges based on player path):**
- Players with the Hacking Item discover the terminal hidden near the coat rack behind Shin.
- Players without it must clear Shin using stealth, then face the laser QTE.

**Resolution:** See Endings section.

---

## 7. Scene Design — Zone Layouts

### Chapter 1: Bedroom

```
[WARDROBE ZONE A]  →  [DESK ZONE B]  →  [BED ZONE C]  →  [WINDOWSILL ZONE D]
  Start position       Push Pin          Pudding sleeping    Chapter exit
  Masking Tape         Foothold point    Curtain rod above   Sunlight beam
  (dark, safe)                           (Hook point)        (story trigger)
```

| Zone | Name | Key Objects | Hazard Level |
|------|------|------------|--------------|
| A | Wardrobe | Starting position, Masking Tape on floor | None |
| B | Desk | Push Pin on desk surface, desk lamp | Low |
| C | Bed | Pudding asleep on mattress, curtain rod above | High |
| D | Windowsill | Window gap exit, sunlight shaft | None |

**Traversal note:** Zone A to D flows linearly but not in a straight line — the room has furniture creating natural corridors and choke points.

---

### Chapter 2: Living Room

```
[ENTRY RUG ZONE A]  →  [SOFA ZONE B]  →  [TV CABINET ZONE C]
  Chapter entry          Mochi patrol       TV (story beat)
  Bookshelf (perimeter)  Coffee Table       Button Battery (Secret)
                         Paperclip          Window gap exit
```

| Zone | Name | Key Objects | Hazard Level |
|------|------|------------|--------------|
| A | Doorway / Entry Rug | Entry point, bookshelf along wall | None |
| B | Sofa + Coffee Table | Paperclip on table, Mochi patrol oval, distraction cup on shelf | High |
| C | TV Cabinet + Window | TV (story trigger), Button Battery behind TV, window exit | Low |

**Design note:** The TV is in the safest zone (C), but to reach it Bendy must cross Mochi's most active zone (B). The emotional high point of the game is placed behind its most frightening obstacle.

---

### Chapter 3: Kitchen

```
[KITCHEN DOORWAY ZONE A]  →  [COUNTER + SINK ZONE B]  →  [DINING TABLE ZONE C]
  Entry                         Dusty patrol loop              Back door exit
                                Fridge (dialogue trigger)
                                Fridge Magnet
                                Modeling Clay (counter)
                                Toy Chip in drawer (Secret)
```

| Zone | Name | Key Objects | Hazard Level |
|------|------|------------|--------------|
| A | Kitchen Doorway | Entry, no items | None |
| B | Counter + Sink | Dusty tight loop, Fridge with ambient audio, Fridge Magnet, Modeling Clay, Toy Chip in drawer | High |
| C | Dining Table + Back Door | Back door exit, dining chairs as cover | Low |

**Design note:** Zone B contains three items AND the narrative dialogue trigger AND Dusty's patrol. Bendy must navigate the most information-dense zone in the game while Dusty orbits around it.

---

### Chapter 4: Laundry Room

```
[LAUNDRY ENTRY ZONE A]  →  [WASHING MACHINE ZONE B]  →  [LINEN SHELF ZONE C]
  Entry (safe)               Dusty patrol                  Mochi stationed
  Crushed hanger under       Copper Wire behind machine     Exit Vent above shelf
  machine visible from       (Secret Part)
  Zone A doorway
```

| Zone | Name | Key Objects | Hazard Level |
|------|------|------------|--------------|
| A | Laundry Entry | Entry; crushed hanger visible in B from here | None |
| B | Washing Machine + Dryer | Dusty patrol, crushed hanger under machine, Copper Wire behind machine | High |
| C | Linen Shelf + Exit Vent | Mochi waiting (stationary, listening), exit vent above shelf | High |

**Design note:** This chapter has no new items to help the player — by design. Chapter 4 is a test of everything learned in Chapters 1–3. The dual-enemy layout demands mastery of stealth (Mochi) and timing/items (Dusty) simultaneously.

---

### Chapter 5: Front Door / Garden

```
[HALLWAY ZONE A]  →  [LASER CORRIDOR ZONE B]  →  [GARDEN ZONE C]
  Shin patrol           3 laser beams                Ending cutscene area
  Wall coat hooks       (QTE trigger)                Normal / Bad / Secret branch
  Hacking Terminal
  (behind Shin, near coat rack)
```

| Zone | Name | Key Objects | Hazard Level |
|------|------|------------|--------------|
| A | Hallway / Front Door Mat | Shin patrol, wall coat hooks, Hacking Terminal near coat rack | High |
| B | Laser Grid Corridor | 3 sweeping laser beams (QTE) | QTE |
| C | Garden Threshold | Front garden, ending-specific visuals | None |

**Design note:** The Hacking Terminal is in Zone A, behind Shin — the Secret Path requires sneaking past the final boss to access the bypass. There is no safe route to the terminal; all three Shin puzzle paths must be used to position near the coat rack.

---

## 8. Puzzle Design — Multi-Path Solutions

Every chapter offers three distinct solutions to its primary stealth challenge. Each path uses different core mechanics or items, allowing players with different playstyles to succeed.

### Chapter 1: Pudding (Bedroom)

| Path | Mechanic | Description |
|------|---------|-------------|
| **A — Quiet Hang** | Skill | Freeze mid-room when Pudding stirs. She hears nothing, settles back. Wait for deep sleep, proceed. |
| **B — Hook** | Skill | Hook onto the curtain rod above the Bed. Swing across Pudding's zone without touching the floor. |
| **C — Snap Spring** | Skill | Charge Snap Spring at the foot of the Bed. Release at full/near-full charge to leap over the Bed before Pudding reacts. Timing-intensive. |

*All three paths use core skills — no items required. Chapter 1 is the mechanics tutorial.*

---

### Chapter 2: Mochi (Living Room)

| Path | Mechanic | Description |
|------|---------|-------------|
| **A — Quiet Hang** | Skill | Stop moving when Mochi's ears flick toward Bendy. Hold still until she completes her patrol loop and moves away. Cross behind her. |
| **B — Distract** | Environment | Nudge the cup on the bookshelf near Zone A. Mochi hears it fall and investigates. Cross Zone B in the gap. (Environmental interaction — cup is not a carried item.) |
| **C — Size Stealth** | Positioning | Crawl along the base of the bookshelf on the room's far perimeter. At this distance and low speed, Bendy's footstep volume is below Mochi's detection threshold. Slowest path. |

---

### Chapter 3: Dusty (Kitchen)

| Path | Mechanic | Description |
|------|---------|-------------|
| **A — Blind Spot** | Timing | Dusty's fixed patrol loop has a predictable gap at the far Counter corner. Time entry into Zone B to the gap; cross and exit before Dusty completes its loop. |
| **B — Jam** | Item | Use Paperclip (carried from Ch2) in Dusty's intake vent. Dusty stutters and stops for 3–4 seconds. Consumes Paperclip. |
| **C — Aerial** | Skill | Hook onto the upper shelf above the Counter. Cross hand-over-hand along the shelf edge. Dusty sweeps below; cannot reach height. |

---

### Chapter 4: Mochi + Dusty (Laundry Room)

| Path | Mechanic | Description |
|------|---------|-------------|
| **A — Sequential Patience** | Skill | Quiet Hang during Dusty's patrol gap in Zone B. Cross to Zone C. Quiet Hang again while Mochi's listening window passes. Pure timing; no items. |
| **B — Physics Item** | Item | Apply Modeling Clay (carried from Ch3) to Bendy. Increased mass dampens footstep vibration below both Dusty's proximity threshold and Mochi's hearing range. Single item neutralises both enemies simultaneously. |
| **C — Sequential Disable** | Skill + Item | Jam Dusty with Paperclip (if saved from Ch3) → cross Zone B freely → Quiet Hang past Mochi in Zone C. Requires carrying Paperclip across two chapters. |

---

### Chapter 5: Shin (Hallway)

| Path | Mechanic | Description |
|------|---------|-------------|
| **A — Camouflage** | Skill | Quiet Hang on the row of coat hooks mounted to the wall. Shin walks past. From his perspective, Bendy is just a hanger where hangers belong. *The thematic climax: Bendy becomes what they are, and it works.* |
| **B — Snap Spring** | Skill | When Shin crouches to examine something on the floor (~every 8 seconds), his attention is down. Spring over his reaching hand in the gap. Narrow timing window. |
| **C — Hacking Distract** | Item | Use Hacking Item at the terminal near the coat rack (requires Secret Ending path). Shin's phone buzzes; he walks toward the kitchen. 6-second window to cross Zone A. Also opens the side bypass to Zone C (skipping Zone B/QTE). |

---

## 9. Item System

### Three-Tier Architecture

Items in this game belong to exactly one of three tiers with distinct rules:

**Tier 1 — Environmental Tools**
- Found in a specific zone
- Used on the spot; not picked up or carried
- Interacted with via proximity action

**Tier 2 — Carried Consumables**
- Picked up and added to inventory
- Single-use; removed from inventory on use
- Can be strategically saved across chapters

**Tier 3 — Secret Parts**
- Collected and held permanently
- Never consumed; not used in puzzles directly
- Three parts exist; collecting all three triggers crafting

---

### Item Reference Table

| Item | Tier | Chapter Found | Zone | Effect |
|------|------|-------------|------|--------|
| **Push Pin** | Environmental | Ch1 | B — Desk | Pin to wall surface to create a temporary foothold for climbing |
| **Masking Tape** | Carried Consumable | Ch1 | A — Wardrobe | Apply to a surface to silence it (squeaky floor, creaky object) |
| **Paperclip** | Carried Consumable | Ch2 | B — Coffee Table | Jam Dusty's vent intake (3–4 sec disable); also functions as lock pick |
| **Fridge Magnet** | Environmental | Ch3 | B — Fridge | Temporarily redirect Dusty's direction sensor (~2 sec) |
| **Modeling Clay** | Carried Consumable | Ch3 | B — Counter | Apply to Bendy's body; increases mass, reduces footstep vibration (fools Dusty proximity + Mochi hearing) |
| **Button Battery** | Secret Part | Ch2 | C — Behind TV | hasBattery = true |
| **Broken Toy Chip** | Secret Part | Ch3 | B — Inside drawer | hasChip = true |
| **Copper Wire** | Secret Part | Ch4 | B — Behind Washing Machine | hasWire = true |

**Note on Paperclip:** If used as a lock pick in Ch2, it is consumed there and unavailable for Ch3/Ch4. Players who save it for Dusty lose the Ch2 lock-pick option. This is the game's primary strategic resource decision.

---

## 10. Crafting System

### Logic

```
// Checked continuously after each item collection
if (hasBattery == true && hasChip == true && hasWire == true) {
    TriggerCraftingEvent();
    Inventory.Add(HackingItem);
}
```

### Crafting Event
- No UI prompt or menu
- A brief animation plays on Bendy (the three parts align and merge)
- Momo glows white for ~2 seconds
- The Hacking Item appears in inventory silently
- Players who notice Momo's white glow and check inventory discover it

### Hacking Item Use
- Only usable at one specific point: the terminal in Ch5 Zone A (near coat rack, behind Shin)
- Triggers Shin's phone notification distraction
- Also opens a bypass route that skips Zone B (laser corridor) entirely

### Secret Part Placement Note
All three Secret Parts are hidden inside enemy patrol/detection zones:
- Battery → Zone C of Ch2 (must cross Mochi's zone; she patrols Zone B)
- Chip → Zone B of Ch3 (inside Dusty's patrol loop)
- Wire → Zone B of Ch4 (behind the Washing Machine in Dusty's zone; next to the crushed hanger)

**Design intent:** Collecting all Secret Parts is not passive exploration — it requires players to go into danger zones they didn't need to enter. The Secret Ending rewards players who took unnecessary risks.

---

## 11. QTE: Laser Gauntlet

**Chapter:** 5 — Zone B (Laser Grid Corridor)
**Triggered by:** Crossing Zone A via Path A or Path B (any path that does NOT use the Hacking Item terminal)

### Sequence

1. Bendy enters Zone B. Laser sensors glow red on both walls.
2. **Beam 1:** A glowing horizontal bar sweeps left-to-right across the corridor at mid-height. A subtle audio cue (rising tone) precedes each beam.
3. Player presses **SPACE** at the right moment → Bendy Snap Springs over the beam.
4. Brief pause.
5. **Beam 2:** Same mechanic; slightly faster sweep.
6. **Beam 3:** Fastest sweep; narrowest timing window.
7. If all 3 cleared → Bendy reaches Zone C → Normal Ending triggers.
8. If any beam hits Bendy → Freeze → Shin appears → Bad Ending triggers.

### Design Notes
- The QTE mechanic IS the Snap Spring — no new input to learn
- The final chapter tests the very first mechanic taught in Chapter 1
- The three-beam sequence feels like a rhythm game "song" with a climax on Beam 3

### Accessibility Feature
- The game tracks cumulative death count via PlayerPrefs
- At **10+ total deaths**, the timing window for each beam auto-extends (silently, no UI notification)
- Players discover this by trying again after multiple failures
- This is not a difficulty mode or toggle — it applies automatically to anyone who struggles

---

## 12. Endings

### Normal Ending

**Trigger:** QTE cleared with no Hacking Item

**Cutscene:**
Bendy pushes open the front door. Outside: afternoon light, but flat. A residential street. Cars passing. People walking, not looking up. Wind that doesn't feel warm.

Bendy stands on the front step. Momo lands beside them.

The outside world is real. It is exactly as large as they imagined. And exactly as indifferent.

Bendy turns around. The front door is still open.

They walk back inside.

**Final image:** Bendy hanging on a coat hook near the front door. Small. Motionless. Alone.
Momo settles on the hook above. Her glow: warm gold.
Fade to black.

**Title card:** None. The silence is enough.

**Emotional tone:** Bittersweet. Freedom was found and found wanting. Simplicity is not defeat — it is wisdom earned through experience. Bendy is exactly where they belong, and they chose to be there.

---

### Bad Ending

**Trigger:** Any QTE laser beam is missed

**Cutscene:**
The beam catches Bendy. A flash of red. Everything stops.

Heavy footsteps. Shin appears at the end of the corridor, curious. He picks Bendy up with both hands — gently. The way you pick up something that doesn't belong on the floor.

He carries Bendy to the kitchen. Opens the bin. Places Bendy inside.

The lid closes.

**Final image:** Darkness. The inside of a bin. Bendy curled around their own hook. Very still.

**Text fades in:** *"The hanger's last wish went unheard."*

Momo is not visible. She is somewhere outside the bin.

**Emotional tone:** Tragic. The crushed hanger in Chapter 4 was a warning. This is that fate. The last wish — freedom — was never answered. The bin is dark and final.

---

### Secret Ending

**Trigger:** All 3 Secret Parts collected → Hacking Item in inventory → Hacking Terminal used in Ch5 Zone A

**Cutscene:**
The terminal glows faintly near the coat rack. Bendy activates it.

Shin's phone buzzes. He straightens, looks at it, and turns toward the kitchen, distracted and humming.

A small section of the wall near the terminal shifts — an old cat-flap, or a gap in the skirting board. A bypass. Bendy had the key.

Bendy slips through. The laser corridor is behind them, untouched.

The garden is different from the Normal Ending. The light is warm. Actual afternoon warmth.

There is a clothesline.

On the clothesline: another hanger. Old. A little bent. But standing.

Bendy stops.

The other hanger does not move. But they are facing the same direction. Looking at the same garden.

Bendy climbs onto the clothesline. Sits beside the other hanger.

Momo lands on the line between them. Her glow: white. Sustained. Not flickering.

**Final image:** Two hangers and a moth on a clothesline in the garden. Warm light. Still.
Fade to white.

**Who is the other hanger?** The game does not say. Old friend, past self, or a stranger who also found a way out long ago. The player carries the interpretation.

**Emotional tone:** Warm, tender, quietly magical. The game's harshest lesson — that the outside world is grey and indifferent — is answered not with a grander adventure, but with belonging. Someone else was already here. You found them because you paid attention.

---

## 13. Progression & Accessibility

### Difficulty Curve

| Chapter | Enemy(s) | New mechanic introduced | Puzzle complexity |
|---------|---------|------------------------|-----------------|
| 1 | Pudding | All 3 core skills (tutorial) | Low — 3 simple paths |
| 2 | Mochi | Sound-based detection; distraction items | Medium |
| 3 | Dusty | Proximity hazard; height advantage; item disable | Medium-High |
| 4 | Mochi + Dusty | Dual enemy; resource conservation (saved Paperclip?) | High |
| 5 | Shin | Vision + reach detection; camouflage mechanic | High (but PATH A is simple if understood) |

### Secret Content Layer

Players who explore danger zones in Ch2–4 collect the Secret Parts. This requires:
- Going behind the TV in Ch2 (passes through Mochi's zone)
- Opening a drawer in Ch3 (inside Dusty's loop)
- Reaching behind the Washing Machine in Ch4 (Dusty patrol + emotional moment)

None of these are required for any main path. They are purely optional, but they unlock the most emotionally resonant ending.

### Accessibility Features

- **QTE adaptive timing:** After 10 cumulative deaths, laser timing window widens. Silent, automatic.
- **Momo as UI:** No screen clutter. Players with visual processing preferences can focus entirely on Momo's glow rather than scanning the UI.
- **Multiple valid paths:** No single required skill for any chapter. Players who cannot execute Snap Spring timing can always use Quiet Hang.
- **Soft Chapter Resets:** Being caught resets the current chapter from the start. No progress loss between chapters.

---

*Document version 2.0 — validated via 24-round deep-interview session on 2026-06-29*
*Interview spec archived at: .omc/specs/deep-interview-hanging-on-the-line.md*
