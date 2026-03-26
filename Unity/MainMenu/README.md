# Initial Interface Setup for Menu System

This project provides the setup for the **initial interface menu and its interactions** in Unity.  
>  **Note:** No AR features are integrated yet. This interface works with a normal camera view and will be updated later for AR interactions.

---

## Overview

The interface is designed to guide the user through the menu system in a simple sequence:

1. **Main Menu Appears First**
   - When the application starts, the `InitialInterfaceCanvas` is displayed.
   - The user sees the main menu panel with the **game selection dropdown** and **Play button**.

2. **Selecting a Game**
   - The dropdown allows the user to choose from three options:
     - Chess
     - Checkers (Unavailable)
     - Tic-Tac-Toe (Unavailable)

3. **Pressing Play**
   - **If a game other than Chess is selected and "Play" is pressed:**
     - The `UnavailableMessage` canvas becomes active.
     - A transparent gray panel with the text "Currently Unavailable" appears, informing the user that the selected game is not ready.
   - **If Chess is selected and "Play" is pressed:**
     - The interface transitions to the `InstructionsCanvas`.
     - The instructions panel appears, guiding the user on how to proceed.
     - The user can then press the **OKAY button** to confirm and continue.

This flow ensures that the user always sees the main menu first, can explore options, and receives clear feedback if a game is not available yet. Once Chess is chosen, the instructions are shown to prepare the user before starting the game.
---

## Included Assets

- All drawings/images are located in the `Img` folder.
   **Important:** For every image:
  1. Go to the **Inspector**.
  2. Set **Texture Type** → `Sprite (2D and UI)`
  3. Set **Sprite Mode** → `Single`
  4. Click **Apply** at the bottom

---

## Setup Instructions

The project consists of three main UI canvases:

1. **UnavailableMessage Canvas**
   - Size: 500 x 200
   - Position: X: 0, Y: 1.5, Z: 1.85
   - Contains a panel:
     - Transparent gray (alpha 230)
     - Text: "Currently Unavailable" in white
   - Initially inactive

2. **InitialInterfaceCanvas**
   - Size: 1000 x 600
   - Position: X: 0, Y: 1.5, Z: 2
   - Contains a panel with the main menu image as background
   - Inside the panel:
     - **Play Button**
       - Size: 150 x 100
       - Position: X: 0, Y: -235, Z: 0
       - Background: Play button image
       - Inspector → `OnClick()` → `MainMenuManager` → `OnPlayPressed`
       - Component: `ButtonAnimator` script
     - **UI Dropdown**
       - Size: 400 x 70
       - Position: X: 0, Y: -30, Z: 0
       - Background: Light brown
       - Text: Size 20, Dark brown
       - Options: Chess, Checkers (Unavailable), Tic-Tac-Toe (Unavailable)
     - Initially active

3. **InstructionsCanvas**
   - Size: 500 x 700
   - Position: X: 0, Y: 1.5, Z: 1.7
   - Contains a panel with the instructions image as background
   - Inside the panel:
     - **OKAY Button**
       - Size: 120 x 150
       - Position: X: 140, Y: -340, Z: 0
       - Component: `ButtonAnimator` script
    - Initially inactive

Finally, an **empty GameObject** called `MenuManager` is created to hold all canvases and is linked to the `MenuManager` script. Inspector values must be set accordingly.


---

## Demonstration

Check the `MainMenuDemoVideo.mp4` for a visual walkthrough of:

---
