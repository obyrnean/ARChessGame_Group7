# Task 2: AR Menu Interface & Chess Board Spawn (Vuforia)

**Author:** Diya Monica Mathew  
**Module:** CSU44057 Extended Reality.Group 7

---

## Overview

This is built on the same foundation as Lab 4 where we implemented a Vuforia image target detection in Unity. The idea is simple: print out a reference image, point the camera at it, and the chess board spawns on top of it in AR. On top of that, there's a floating AR menu interface where you pick a game from a dropdown and press Play to trigger the spawn.

The whole thing runs in Unity's Play Mode using the laptop webcam. Phone support is Albertina's task.

---

## How It Works

1. Press Play in Unity. The WELCOME menu appears floating in the camera feed
2. Pick a game from the dropdown (Chess, Checkers, Tic-Tac-Toe, Monopoly, Parchis)
3. Press the Play button
4. Point the camera at the printed reference image (`reference1.png`)
5. The chess board spawns on top of it
6. Instructions panel appears and we press OKAY to dismiss it
7. Use the RULES button to bring instructions back, EXIT to return to the main menu

---

## Scene Structure

```
SampleScene
├── ARCamera (Vuforia)
├── ImageTarget (reference1.png)
│   └── ChessModel (disabled by default)
│       └── CHessBoardModel2
├── InitialInterfaceCanvas (World Space, always active)
│   └── Panel (bckgrnd welcome image)
│       ├── GameDropdown
│       └── PlayButton
├── UnavailableCanvas (disabled)
├── InstructionsCanvas (disabled)
│   └── Panel
│       └── OKAYButton
├── GameplayCanvas (disabled)
│   └── Panel
│       ├── RulesButton
│       └── ExitButton
└── ARMenuManager (empty GameObject holding ARMenuManager.cs)
```

---

## Scripts

### ARMenuManager.cs
This is the main controller. It handles all canvas switching and decides which board game model to activate based on the dropdown selection. The `boardGameModels[]` array has the dropdown indices: index 0 is Chess, index 1 is Checkers, and so on. If no model is assigned for a selection, it shows the Unavailable popup automatically.

Configured to worldspace so it actually follows the camera. 

Important: Ane's `MenuManager.cs` is completely untouched. This is a separate script with a different name so original file is still available.

### ChessSetup_Diya.cs
Extended from Ane's `ChessSetup.cs` — again, her original is untouched. The two changes made:
- Added `Start()` alongside `OnEnable()` so pieces spawn correctly when the scene first loads and also when ChessModel gets re-enabled at runtime

### ButtonAnimator.cs / InstructionsUI.cs
Ane's scripts, used as it is with no modifications.

---

## Setup Instructions

1. Open `VuforiaProject` in Unity 6
2. Make sure Vuforia Engine is imported (Window > Package Manager > My Assets)
3. Add your Vuforia license key to the ARCamera Inspector (Open Vuforia Engine Configuration)
4. Print `reference1.png` from `Assets/Images/` or display it on a phone screen
5. Press Play
6. Point the webcam at the reference image to trigger the chess board spawn

---

## Known Issues

- Chess pieces are being created in the Hierarchy (WhitePieces and BlackPieces show up) but aren't visible in the game view. The board position calculation is working but the pieces end up at incorrect world coordinates. I believe this is a scaling issue. It is still being looked into.
- The Play button face shows "Button" instead of "PLAY" visually — the TMP child is correctly set but the Button's Target Graphic is pointing to the text object instead of the image. But everything works fine functionally.

---

## What's Pending

- Phone camera support instead of laptop webcam (Albertina)
- Piece interaction: grab and move pieces (separate task)
- Models for the other board games in the dropdown (Ane)

