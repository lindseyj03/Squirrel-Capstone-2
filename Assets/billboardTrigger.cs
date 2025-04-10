using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class BillboardTrigger : MonoBehaviour
{
    public TextMeshProUGUI promptText;  // Text prompt for interaction
    public Sprite billboardSprite;      // The billboard sprite to show
    public InputAction interactAction;  // Input action for interaction
    public GameObject spriteContainer;  // The container object to hold the sprite

    private bool playerInRange = false;
    private bool spriteShowing = false;

    void Start()
    {
        promptText.gameObject.SetActive(false);  // Initially hide the prompt text
        spriteContainer.SetActive(false);        // Initially hide the sprite container
        interactAction.Disable();                // Disable interact action initially
    }

    void Update()
    {
        // Check if the player is in range and presses the interact button
        if (playerInRange && !spriteShowing && interactAction.triggered)
        {
            ShowBillboard();
        }
    }

    private void ShowBillboard()
    {
        spriteShowing = true;
        promptText.gameObject.SetActive(false);  // Hide the prompt text
        spriteContainer.SetActive(true);         // Show the sprite container
        spriteContainer.GetComponent<SpriteRenderer>().sprite = billboardSprite;  // Set the sprite
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (!spriteShowing)
            {
                promptText.text = "Press the interact button to view the billboard";
                promptText.gameObject.SetActive(true);  // Show the prompt text
                interactAction.Enable();               // Enable the interact action when in range
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            promptText.gameObject.SetActive(false);  // Hide the prompt text
            interactAction.Disable();                // Disable the interact action when out of range
        }
    }

    private void OnDisable()
    {
        interactAction.Disable();  // Ensure interact action is disabled when the script is disabled
    }
}
