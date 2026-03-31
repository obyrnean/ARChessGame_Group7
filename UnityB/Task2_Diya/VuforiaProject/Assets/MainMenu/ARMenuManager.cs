using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// ARMenuManager (Diya - Task 2)
/// Replaces MenuManager.cs for the New Plan AR scene.
///
/// Key differences from Ane's MenuManager:
///  - boardGameModels[] array: assign one model per dropdown option (chess, checkers, etc.)
///    Each model should be a child of the ImageTarget (so it appears ON the printed image).
///  - The main menu canvas follows the AR camera (World Space canvas).
///  - Only the selected game model is activated; all others stay hidden.
///  - Does NOT move the chess board by code — position is handled by the ImageTarget parent.
///
/// HOW TO USE IN UNITY (read the full guide at the bottom of this file):
///   1. Attach this script to an empty GameObject called "ARMenuManager".
///   2. Assign all Inspector fields.
///   3. Do NOT use Ane's MenuManager.cs in this scene — use only this one.
/// </summary>
public class ARMenuManager : MonoBehaviour
{
    // -------------------------------------------------------------------------
    // INSPECTOR FIELDS
    // -------------------------------------------------------------------------

    [Header("UI Canvases")]
    [Tooltip("The main menu panel (World Space canvas that floats in AR).")]
    public GameObject mainMenuCanvas;

    [Tooltip("The 'Currently Unavailable' popup canvas.")]
    public GameObject unavailableCanvas;

    [Tooltip("The instructions canvas shown after pressing Play.")]
    public GameObject instructionsCanvas;

    [Tooltip("The in-game canvas with Rules and Exit buttons.")]
    public GameObject inGameCanvas;

    [Header("Dropdown")]
    [Tooltip("The TMP_Dropdown for selecting a board game.")]
    public TMP_Dropdown gameDropdown;

    [Header("Board Game Models (children of ImageTarget)")]
    [Tooltip(
        "Assign one model per dropdown option, IN THE SAME ORDER as the dropdown options.\n" +
        "Example: index 0 = Chess model, index 1 = Checkers model, index 2 = Tic-Tac-Toe model, etc.\n" +
        "Each model must already be a child of the ImageTarget in the scene hierarchy.\n" +
        "All models start as inactive; only the chosen one is activated on Play."
    )]
    public GameObject[] boardGameModels;

    [Header("AR UI Settings")]
    [Tooltip("How far in front of the camera the main menu canvas floats (metres).")]
    public float menuDistance = 1.5f;

    [Tooltip("How fast the menu canvas follows the camera rotation (lower = smoother lag).")]
    public float menuFollowSpeed = 3f;

    [Tooltip("How many seconds to show the Unavailable popup.")]
    public float unavailableDisplayTime = 2f;

    // -------------------------------------------------------------------------
    // PRIVATE
    // -------------------------------------------------------------------------

    private Camera arCamera;
    private int activeModelIndex = -1; // which model is currently shown (-1 = none)

    // -------------------------------------------------------------------------
    // UNITY LIFECYCLE
    // -------------------------------------------------------------------------

    void Start()
    {
        // Cache the AR camera. Vuforia's ARCamera also registers as Camera.main.
        arCamera = Camera.main;

        // Initial UI state — same logic as Ane's MenuManager
        mainMenuCanvas.SetActive(true);
        unavailableCanvas.SetActive(false);
        instructionsCanvas.SetActive(false);
        inGameCanvas.SetActive(false);

        // Hide all board game models at start
        HideAllModels();

        // Position the menu in front of the camera immediately
        SnapMenuToCamera();
    }

    void Update()
    {
        // Only float the menu when it is visible
        if (mainMenuCanvas.activeSelf)
        {
            FloatMenuWithCamera();
        }
    }

    // -------------------------------------------------------------------------
    // PUBLIC BUTTON CALLBACKS
    // (wire these up in the Inspector just like Ane's MenuManager)
    // -------------------------------------------------------------------------

    /// <summary>
    /// Called by the Play button's OnClick event.
    /// </summary>
    public void OnPlayPressed()
    {
        int index = gameDropdown.value;

        // Safety check: make sure we have a model assigned for this index
        if (index < boardGameModels.Length && boardGameModels[index] != null)
        {
            // Hide main menu
            mainMenuCanvas.SetActive(false);

            // Activate the selected game model on the image target
            ShowModel(index);

            // Show instructions, then in-game UI
            instructionsCanvas.SetActive(true);
            inGameCanvas.SetActive(true);
        }
        else
        {
            // No model assigned for this dropdown option → show unavailable
            StartCoroutine(ShowUnavailable());
        }
    }

    /// <summary>
    /// Called by the Exit button's OnClick event.
    /// Returns to the main menu and hides all models.
    /// </summary>
    public void OnExitToMenu()
    {
        instructionsCanvas.SetActive(false);
        inGameCanvas.SetActive(false);

        HideAllModels();

        mainMenuCanvas.SetActive(true);

        // Reset dropdown to Chess (index 0)
        gameDropdown.value = 0;
    }

    // -------------------------------------------------------------------------
    // PRIVATE HELPERS
    // -------------------------------------------------------------------------

    /// <summary>
    /// Activate only the model at the given index; deactivate everything else.
    /// </summary>
    private void ShowModel(int index)
    {
        HideAllModels();

        if (index >= 0 && index < boardGameModels.Length && boardGameModels[index] != null)
        {
            boardGameModels[index].SetActive(true);
            activeModelIndex = index;
        }
    }

    /// <summary>
    /// Deactivate all board game models.
    /// </summary>
    private void HideAllModels()
    {
        for (int i = 0; i < boardGameModels.Length; i++)
        {
            if (boardGameModels[i] != null)
                boardGameModels[i].SetActive(false);
        }
        activeModelIndex = -1;
    }

    /// <summary>
    /// Instantly snaps the menu canvas to the camera's forward direction.
    /// Called once on Start.
    /// </summary>
    private void SnapMenuToCamera()
    {
        if (mainMenuCanvas == null || arCamera == null) return;

        Vector3 targetPos = arCamera.transform.position + arCamera.transform.forward * menuDistance;
        mainMenuCanvas.transform.position = targetPos;
        mainMenuCanvas.transform.rotation = Quaternion.LookRotation(
            mainMenuCanvas.transform.position - arCamera.transform.position
        );
    }

    /// <summary>
    /// Smoothly moves the menu canvas to float in front of the camera.
    /// Called every frame while the menu is visible.
    /// </summary>
    private void FloatMenuWithCamera()
    {
        if (mainMenuCanvas == null || arCamera == null) return;

        // Target position: directly in front of the camera
        Vector3 targetPos = arCamera.transform.position + arCamera.transform.forward * menuDistance;

        // Smoothly lerp position
        mainMenuCanvas.transform.position = Vector3.Lerp(
            mainMenuCanvas.transform.position,
            targetPos,
            Time.deltaTime * menuFollowSpeed
        );

        // Always face the camera (billboard effect)
        mainMenuCanvas.transform.rotation = Quaternion.Lerp(
            mainMenuCanvas.transform.rotation,
            Quaternion.LookRotation(mainMenuCanvas.transform.position - arCamera.transform.position),
            Time.deltaTime * menuFollowSpeed
        );
    }

    // -------------------------------------------------------------------------
    // COROUTINES
    // -------------------------------------------------------------------------

    private IEnumerator ShowUnavailable()
    {
        unavailableCanvas.SetActive(true);
        yield return new WaitForSeconds(unavailableDisplayTime);
        unavailableCanvas.SetActive(false);
    }
}

/*
==============================================================================
FULL UNITY SETUP GUIDE FOR DIYA — STEP BY STEP
==============================================================================

PREREQUISITES:
- You have Unity open with the ARBoardGames_Group7 project.
- You have already git-pulled Ane's ChessModelSetup folder into the project.
- You have already done Lab 4 (Vuforia is imported, ARCamera exists, ImageTarget exists).
  If not, do Lab 4 setup first before continuing.

------------------------------------------------------------------------------
STEP 1 — VERIFY YOUR VUFORIA SCENE IS WORKING
------------------------------------------------------------------------------
1. Open your Unity scene (or create a new one called "ARBoardGames").
2. Make sure you have:
   - ARCamera (from GameObject > Vuforia Engine > ARCamera)
   - ImageTarget (from GameObject > Vuforia Engine > Image Target)
   - Your Vuforia license key entered in the ARCamera Inspector.
   - The printed image (bckgrnd.png — your checkerboard welcome image) assigned
     to the ImageTarget. Import it into Assets/Images and assign it in the
     ImageTarget Behaviour (Inspector > Type: From Image > select your image).
3. Press Play and confirm the camera feed appears and the image target works
   (you can test by showing the image to your webcam — you should see a yellow
   outline around it). DO THIS BEFORE ANYTHING ELSE.

------------------------------------------------------------------------------
STEP 2 — SET UP THE CHESS MODEL ON THE IMAGE TARGET
------------------------------------------------------------------------------
1. In the Hierarchy, find your ImageTarget.
2. Drag Ane's ChessModel prefab/GameObject to be a CHILD of the ImageTarget.
   (Right-click ImageTarget > drag the ChessModel in, or cut-paste in hierarchy.)
3. Set the ChessModel's Transform to:
   - Position: (0, 0, 0) — relative to the ImageTarget
   - Scale: (0.02, 0.02, 0.02) — as per Ane's README
4. Make sure ChessSetup.cs is attached to ChessModel and all its Inspector
   fields are assigned (chessBoardModel, all piece prefabs, materials).
5. DISABLE the ChessModel (uncheck the checkbox at the top of its Inspector)
   — ARMenuManager will enable it when the user presses Play.
6. Repeat this for any other board game models you have (Checkers, etc.):
   each one goes as a CHILD of the ImageTarget, positioned at (0,0,0),
   and starts DISABLED.

------------------------------------------------------------------------------
STEP 3 — SET UP THE CANVASES AS WORLD SPACE
------------------------------------------------------------------------------
The key change from the old setup: canvases must be WORLD SPACE (not Screen
Space Overlay) so they float in AR and follow the camera.

For EACH canvas (InitialInterfaceCanvas, UnavailableMessage, InstructionsCanvas,
GameplayCanvas):
1. Select the canvas in the Hierarchy.
2. In the Inspector, under Canvas component:
   - Render Mode: World Space
3. Set the canvas scale to something small so it doesn't appear giant in 3D.
   A good starting scale is (0.001, 0.001, 0.001).
4. Set the canvas size (Rect Transform) to your existing sizes from Ane's README:
   e.g., InitialInterfaceCanvas: Width 1000, Height 600.
   (With scale 0.001, this becomes 1m × 0.6m in world space — readable on phone.)
5. Add a Canvas Scaler component (if not present):
   - UI Scale Mode: World
6. Make sure an Event System exists in the Hierarchy (it should be there
   automatically if you have any canvas; if not: GameObject > UI > Event System).

IMPORTANT — RAYCASTER FOR AR TOUCH INPUT:
Each canvas also needs to receive touch/click input through the ARCamera.
- Select the ARCamera GameObject.
- In Inspector, click Add Component > search "Physics Raycaster" and add it.
  (This lets Unity detect which UI element you tap through the AR camera.)
- On each canvas, make sure there is a "Graphic Raycaster" component
  (this is added automatically when you create a canvas — just verify it exists).

------------------------------------------------------------------------------
STEP 4 — ADD ARMenuManager TO THE SCENE
------------------------------------------------------------------------------
1. Create an empty GameObject: right-click in Hierarchy > Create Empty.
   Name it "ARMenuManager".
2. Drag the ARMenuManager.cs script onto this GameObject.
3. In the Inspector, assign all fields:
   - Main Menu Canvas → InitialInterfaceCanvas
   - Unavailable Canvas → UnavailableMessage canvas
   - Instructions Canvas → InstructionsCanvas
   - In Game Canvas → GameplayCanvas
   - Game Dropdown → the TMP_Dropdown inside InitialInterfaceCanvas
   - Board Game Models:
       Size: however many games you have (start with 5 to match the dropdown)
       Element 0: ChessModel (child of ImageTarget)
       Element 1: CheckersModel (child of ImageTarget, or leave null)
       Element 2: TicTacToeModel (if you have it, else null)
       Element 3: MonopolyModel (null for now)
       Element 4: ParchisModel (null for now)
   - Menu Distance: 1.5
   - Menu Follow Speed: 3
   - Unavailable Display Time: 2

------------------------------------------------------------------------------
STEP 5 — WIRE UP BUTTON EVENTS
------------------------------------------------------------------------------
All button OnClick() events should now point to ARMenuManager instead of
the old MenuManager:

Play button → ARMenuManager > OnPlayPressed()
OKAY button → InstructionsUI > OnOKPressed()   (Ane's script, unchanged)
RULES button → InstructionsUI > ShowInstructions()   (Ane's script, unchanged)
EXIT button  → ARMenuManager > OnExitToMenu()

------------------------------------------------------------------------------
STEP 6 — POSITION THE MAIN MENU CANVAS INITIALLY
------------------------------------------------------------------------------
The ARMenuManager script will auto-position the canvas in front of the camera
on Start(). But for the Editor, place it somewhere visible:
- InitialInterfaceCanvas Transform Position: (0, 1.5, 1.5)
This is just for editing convenience; at runtime it snaps to the camera.

------------------------------------------------------------------------------
STEP 7 — TEST
------------------------------------------------------------------------------
1. Press Play in Unity.
2. You should see the InitialInterfaceCanvas floating in front of the camera feed.
3. Select "Chess" in the dropdown and press Play.
4. The canvas should hide and the chess board should appear on the image target
   (show the printed image to your webcam to confirm).
5. The instructions canvas should appear.
6. Press Exit — you should return to the main menu canvas.

==============================================================================
*/
