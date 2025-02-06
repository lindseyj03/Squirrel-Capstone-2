using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject interactionPrompt; // Drag your UI text here in the Inspector

    private void Start()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false); // Hide UI at start
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure your player has the "Player" tag
        {
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
        }
    }
}
