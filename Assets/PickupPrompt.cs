using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PickupPrompt : MonoBehaviour
{
    public GameObject pickupPrompt; // The UI image like your skipButton
    public InputAction interactAction;

    private bool isPlayerNearby = false;

    void Start()
    {
        if (pickupPrompt != null)
            pickupPrompt.SetActive(false); // Hide prompt at start

        interactAction.Enable(); // Turn on input
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            if (pickupPrompt != null)
                pickupPrompt.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (pickupPrompt != null)
                pickupPrompt.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerNearby && interactAction.triggered)
        {
            // Do pickup logic here
            Debug.Log("Picked up the object!");
        }
    }

    private void OnDisable()
    {
        interactAction.Disable();
    }
}
