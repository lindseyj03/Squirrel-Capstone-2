using UnityEngine;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    public GameObject interactionPrompt; // Drag your "Press A to interact" UI text here
    public GameObject additionalText; // Drag the additional text for interaction here
    public GameObject taskListText; // Task list text that stays on screen
    public float additionalTextDuration = 3f; // Time in seconds before the additional text disappears

    // Input action for interaction
    public InputAction interactAction; // A button action (e.g., A button)

    private bool isPlayerNearby = false; // Check if the player is near to the interactable object

    private void Start()
    {
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
        if (additionalText != null) additionalText.SetActive(false);
        if (taskListText != null) taskListText.SetActive(false); // Hide the task list initially
    }

    private void OnEnable()
    {
        interactAction.Enable();
    }

    private void OnDisable()
    {
        interactAction.Disable();
    }

    private void Update()
    {
        // If player is nearby and presses interact button (e.g., A), show additional text
        if (isPlayerNearby && interactAction.triggered)
        {
            ShowAdditionalText();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure the player's tag is "Player"
        {
            isPlayerNearby = true;
            if (interactionPrompt != null) interactionPrompt.SetActive(true); // Show prompt
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (interactionPrompt != null) interactionPrompt.SetActive(false); // Hide prompt
        }
    }

    private void ShowAdditionalText()
    {
        if (interactionPrompt != null) interactionPrompt.SetActive(false); // Hide the initial prompt immediately
        if (additionalText != null)
        {
            additionalText.SetActive(true); // Show the additional text
            Invoke(nameof(HideAdditionalText), additionalTextDuration); // Hide the additional text after a delay
        }
    }

    private void HideAdditionalText()
    {
        if (additionalText != null) additionalText.SetActive(false); // Hide the additional text after a few seconds
        ShowTaskListText(); // Show the task list text after the additional text disappears
    }

    private void ShowTaskListText()
    {
        if (taskListText != null)
        {
            taskListText.SetActive(true); // Show the task list text after additional text is hidden
        }
    }
}


//using UnityEngine;

//public class Interactable : MonoBehaviour
//{
//    public GameObject interactionPrompt; // Drag your UI text here in the Inspector

//    private void Start()
//    {
//        if (interactionPrompt != null)
//        {
//            interactionPrompt.SetActive(false); // Hide UI at start
//        }
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("Player")) // Make sure your player has the "Player" tag
//        {
//            if (interactionPrompt != null)
//            {
//                interactionPrompt.SetActive(true);
//            }
//        }
//    }

//    private void OnTriggerExit(Collider other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            if (interactionPrompt != null)
//            {
//                interactionPrompt.SetActive(false);
//            }
//        }
//    }
//}
