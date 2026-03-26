using UnityEngine;
using TMPro;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Dropdown gameDropdown;
    public GameObject mainMenuCanvas;
    public GameObject unavailableCanvas;
    public GameObject instructionsCanvas;
    public GameObject inGameCanvas;

    [Header("Game")]
    public GameObject chessBoardManager;

    [Header("Settings")]
    public float unavailableDisplayTime = 2f;

    void Start()
    {
        // Initial state
        mainMenuCanvas.SetActive(true);
        unavailableCanvas.SetActive(false);
        instructionsCanvas.SetActive(false);
        inGameCanvas.SetActive(false);
        chessBoardManager.SetActive(false);
    }

    // PLAY BUTTON
    public void OnPlayPressed()
    {
        string selectedGame = gameDropdown.options[gameDropdown.value].text;

        if (selectedGame == "Chess")
        {
            // Hide main menu
            mainMenuCanvas.SetActive(false);

            // Spawn chess board
            SpawnChessBoard();

            // Show instructions on top
            instructionsCanvas.SetActive(true);

            // Enable in-game UI (rules + exit button)
            inGameCanvas.SetActive(true);
        }
        else
        {
            // Show unavailable message
            StartCoroutine(ShowUnavailable());
        }
    }

    // SPAWN CHESS BOARD
    void SpawnChessBoard()
    {
        chessBoardManager.SetActive(true);

        // Place in front of user (for AR/VR)
        Transform cam = Camera.main.transform;
        chessBoardManager.transform.position = cam.position + cam.forward * 1.5f;
    }

    // UNAVAILABLE MESSAGE
    IEnumerator ShowUnavailable()
    {
        unavailableCanvas.SetActive(true);
        yield return new WaitForSeconds(unavailableDisplayTime);
        unavailableCanvas.SetActive(false);
    }

    public void OnExitToMenu()
    {
        // Hide gameplay elements
        instructionsCanvas.SetActive(false);
        inGameCanvas.SetActive(false);
        chessBoardManager.SetActive(false);

        // Show main menu again
        mainMenuCanvas.SetActive(true);

        // Optional: reset dropdown to default (Chess)
        gameDropdown.value = 0;
    }
}
