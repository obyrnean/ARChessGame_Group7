using UnityEngine;

public class InstructionsUI : MonoBehaviour
{
    public GameObject instructionsCanvas;

    public void OnOKPressed()
    {
        instructionsCanvas.SetActive(false);
    }

    public void ShowInstructions()
    {
        instructionsCanvas.SetActive(true);
    }
}