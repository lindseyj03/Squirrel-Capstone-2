using TMPro; // Import TextMeshPro namespace
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    public static InteractionUI Instance; // Singleton for easy access
    public TextMeshProUGUI interactionTextUI; // The UI Text component to show the prompt

    private void Awake()
    {
        // Ensuring there's only one instance of this script
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Call this to show the prompt when the player is near an interactable object
    public void ShowInteractionPrompt(string message)
    {
        interactionTextUI.text = message; // Update the text
        interactionTextUI.gameObject.SetActive(true); // Make sure the prompt is visible
    }

    // Call this to hide the prompt when the player leaves the area
    public void HideInteractionPrompt()
    {
        interactionTextUI.gameObject.SetActive(false); // Hide the prompt
    }
}
