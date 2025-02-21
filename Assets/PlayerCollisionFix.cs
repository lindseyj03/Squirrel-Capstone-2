using UnityEngine;

public class PlayerCollisionFix : MonoBehaviour
{
    private Rigidbody playerRigidbody;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>(); // Get the player's Rigidbody component
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("VendingMachine"))
        {
            // Reduce player's velocity temporarily to avoid tunneling
            playerRigidbody.linearVelocity = Vector3.zero; // Stops the player's movement
        }
    }

    // If you're using newer Unity versions (2020+), use OnCollisionStay2D for 2D collisions
    // if you need to handle collisions on 2D colliders instead of 3D:
    // void OnCollisionStay2D(Collision2D collision) { ... }
}
