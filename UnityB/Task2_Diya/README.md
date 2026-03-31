# Task 2 - AR Menu Interface & Chess Board Spawn

**Assignee:** Diya Monica Mathew  
**Project:** CSU44057 Extended Reality — Group 7

---

## What This Does

Built on top of Lab 4's image target setup (Vuforia + Unity). When you point the camera at the printed reference image, the chess board spawns on top of it. On top of that, there's a full floating AR menu interface that lets you choose a board game from a dropdown and press Play to spawn the corresponding model.

The UI is set up as World Space canvases so they float in AR rather than covering the whole screen like a regular overlay would.

---

## What's in This Folder

- `Assets/ChessModels/` — all 7 FBX models (board + 6 pieces) and the ChessSetup_Diya script
- `Assets/MainMenu/` — ARMenuManager, ButtonAnimator, InstructionsUI, MenuManager (Ane's original, untouched)
- `Assets/MainMenu/Img/` — background image for the main menu panel
- `Assets/Images/` — reference1.png (the Vuforia image target)

---

## Scene Structure

```
SampleScene
├── ARCamera
├── ImageTarget
│   └── ChessModel (disabled by default, enabled on Play)
│       └── CHessBoardModel2
├── InitialInterfaceCanvas (World Space)
│   └── Panel (bckgrnd image)
│       ├── GameDropdown (Chess, Checkers, Tic-Tac-Toe, Monopoly, Parchis)
│       └── PlayButton
├── UnavailableCanvas (disabled)
├── InstructionsCanvas (disabled)
│   └── Panel
│       └── OKAYButton
├── GameplayCanvas (disabled)
│   └── Panel
│       ├── RulesButton
│       └── ExitButton
└── ARMenuManager (holds ARMenuManager.cs)
```

---

## How to Set It Up

1. Open the `VuforiaProject` in Unity 6
2. Make sure Vuforia is imported and your license key is in the ARCamera inspector
3. Print out `reference1.png` (in Assets/Images) or display it on your phone
4. Press Play — the WELCOME interface should appear floating in the camera feed
5. Select Chess from the dropdown and press Play
6. Point the camera at the reference image — the chess board should spawn on it

---

## Scripts

### ARMenuManager.cs (mine)
Handles all the canvas switching and model spawning. When Play is pressed, it checks the dropdown value and activates the corresponding model from the `boardGameModels[]` array. The canvas floats in front of the camera using a lerp in Update(). Did NOT touch Ane's MenuManager.cs — made this as a separate script.

### ChessSetup_Diya.cs (mine, extended from Ane's ChessSetup.cs)
Same as Ane's ChessSetup but with `Start()` + `OnEnable()` instead of just `Start()`, so pieces respawn correctly when the ChessModel is re-enabled at runtime. Also uses `GetComponentInChildren<Renderer>()` instead of `GetComponent<Renderer>()` to handle the FBX structure properly.

---

## Known Issues

- Chess pieces are currently not appearing on the board at runtime even though WhitePieces and BlackPieces GameObjects are being created in the Hierarchy. The board position calculation works but the pieces spawn at incorrect world coordinates due to the 0.02 scale on ChessModel interacting weirdly with the bounds calculation. Still investigating.
- The Play button label shows "Button" visually even though the TMP child is correctly set to "PLAY" — cosmetic issue only, functionality works fine.

---

## What's Not Done Yet

- Phone visualization (Albertina's task — needed to test on mobile instead of laptop webcam)
- Piece interaction / grab and move (separate task)
- Models for Checkers, Tic-Tac-Toe, Monopoly, Parchis (Ane's task)

