using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    // You may want to call the TryToExit method when the player triggers the exit area.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Directly check if the player has found all squirrels
            if (SquirrelTracker.instance.SquirrelsFound >= SquirrelTracker.instance.TotalSquirrels)
            {
                Debug.Log("All squirrels found! You can now exit.");
                if (SquirrelTracker.instance.exitTunnel != null)
                {
                    SquirrelTracker.instance.exitTunnel.SetActive(true);
                }
            }
            else
            {
                Debug.Log("You still need to find more squirrels before you can exit.");
            }
        }
    }
}