using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Dropdown gameDropdown;   // TMP Dropdown
    public GameObject mainMenuCanvas;
    public GameObject unavailableCanvas;
    public GameObject instructionsCanvas;

    [Header("Settings")]
    public float unavailableDisplayTime = 2f; // seconds

    void Start()
    {
        mainMenuCanvas.SetActive(true);
        unavailableCanvas.SetActive(false);
        instructionsCanvas.SetActive(false);

        if (gameDropdown == null)
            Debug.LogError("GameDropdown is not assigned in the inspector!");
    }

    public void OnPlayPressed()
    {
        if (gameDropdown == null)
        {
            Debug.LogError("GameDropdown not set!");
            return;
        }

        string selectedGame = gameDropdown.options[gameDropdown.value].text;

        if (selectedGame == "Chess")
        {
            mainMenuCanvas.SetActive(false);
            instructionsCanvas.SetActive(true);
        }
        else
        {
            StartCoroutine(ShowUnavailable());
        }
    }

    IEnumerator ShowUnavailable()
    {
        unavailableCanvas.SetActive(true);
        yield return new WaitForSeconds(unavailableDisplayTime);
        unavailableCanvas.SetActive(false);
    }
}
