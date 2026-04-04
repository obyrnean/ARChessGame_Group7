**Author:** Diya Monica Mathew  
**Module:** CSU44057 Extended Reality.Group 7

# Initial Interface: Meta Quest 3 Implementation

This folder contains the Unity project for the initial menu interface built for the Meta Quest 3 headset. It's based on the Hello World tutorial project sent by professor Son that Ane set up and extends it with a full menu system that appears when the app launches.

---

## What this does

When the app starts, instead of going straight into the chess scene, the user is greeted with a welcome screen where they can choose a board game and press Play. Only Chess is available. Selecting anything else briefly shows an "unavailable" message. Pressing Play spawns the chess board in front of the user and brings up an instructions panel explaining how to play. From there, the user can dismiss the instructions, bring them back at any time using the Rules button, or exit back to the main menu.

---

## Scripts

**ARMenuManager.cs** — the main controller. Handles showing and hiding all four canvases, spawning the chess board in front of the user using the OVRCameraRig's center eye anchor (so it works correctly on the headset), and managing the dropdown logic. Attach this to an empty GameObject in the scene and wire up all the canvas and chess model references in the Inspector.

---

## Scene setup

All canvases are World Space with Scale set to `0.002` on all axes. The Event Camera field on each canvas must be set to `CenterEyeAnchor`, found inside `[BuildingBlock] Camera Rig > TrackingSpace` in the Hierarchy. Without this, the canvases won't receive input correctly on the headset.

There are four canvases:

### InitialInterfaceCanvas
- Size: `1000 x 600`, Position: `X: 0, Y: 0, Z: 2`
- Contains `MainMenuPanel` with `bckgrnd` as the Source Image
- Inside the panel:
  - **GameDropdown** (TMP Dropdown): Size `400 x 70`, Position `X: 0, Y: -30`. Options: Chess, Checkers (Unavailable), Tic-Tac-Toe (Unavailable)
  - **PlayButton**: Size `150 x 100`, Position `X: 0, Y: -235`. Source Image: `playBttnpng`. OnClick → `ARMenuManager > OnPlayPressed()`. Has `ButtonAnimator` component.
- Starts **active**

### UnavailableMessage
- Size: `500 x 200`, Position: `X: 0, Y: 0, Z: 2`
- Contains a Panel with Color set to gray (R:128 G:128 B:128 A:230) and a TMP text reading "Currently Unavailable" in white
- Starts **inactive**

---

### InstructionsCanvas
- Size: `500 x 700`, Position: `X: 0, Y: 0, Z: 1.5`
- Contains a Panel with `ChessRulesBckgrnd` as the Source Image
- Inside the panel:
  - **OKAYButton**: Size `120 x 50`, Position `X: 140, Y: -340`. Source Image: `OKAYbttn`. OnClick → `InstructionsUI_AR > OnOKPressed()`. Has `ButtonAnimator` component.
- Has `InstructionsUI_AR` component attached with:
  - `instructionsCanvas` → InstructionsCanvas
  - `gameplayCanvas` → GameplayCanvas
  - `frontZ` → 1.5
  - `backZ` → 4.0
- Starts **inactive**

### GameplayCanvas
- Size: `1500 x 1000`, Position: `X: 0, Y: 0, Z: 2`
- Contains a Panel with Color Alpha set to `0` (fully transparent)
- Inside the panel:
  - **RulesButton**: Size `100 x 100`, Position `X: -300, Y: 200`. Source Image: `RULESbttn`. OnClick → `InstructionsUI_AR > ShowInstructions()`. Has `ButtonAnimator` component.
  - **ExitButton**: Size `100 x 100`, Position `X: 300, Y: 200`. Source Image: `ExitBttn`. OnClick → `ARMenuManager > OnExitToMenu()`. Has `ButtonAnimator` component.
- Starts **inactive**

---

## Notes

- Did not modify `MenuManager.cs` or any of Ane's original files — all new scripts use different names
- The chess board showing through the instructions canvas in the Unity editor is a known World Space rendering limitation and looks correct on the actual headset
- If OnClick() functions don't appear in the Inspector dropdown, change from assets to Scenes.