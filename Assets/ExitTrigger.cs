using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    // Reference to the SquirrelTracker to manipulate UI and game logic
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Trigger the message when the player enters the trigger area
            SquirrelTracker.instance.TryToGoToTunnel();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // When the player exits the trigger area, hide the "You can't leave yet" message
            if (SquirrelTracker.instance.tunnelMessage != null)
            {
                SquirrelTracker.instance.tunnelMessage.gameObject.SetActive(false);
            }
        }
    }
}
