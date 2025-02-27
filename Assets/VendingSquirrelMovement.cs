using UnityEngine;

public class VendingSquirrelMovement : MonoBehaviour
{
    public GameObject player;         // Reference to the player
    public float followDistance = 2f; // Distance to follow the player
    public float followSpeed = 3f;    // Speed of following the player
    public Animator squirrelAnimator; // Animator for the squirrel

    void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        if (player != null && squirrelAnimator != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer > followDistance)
            {
                // Move the squirrel toward the player
                Vector3 direction = (player.transform.position - transform.position).normalized;
                transform.Translate(direction * followSpeed * Time.deltaTime, Space.World);

                // Set walking animation
                squirrelAnimator.SetBool("isWalking", true);
            }
            else
            {
                // Stop walking animation when close enough
                squirrelAnimator.SetBool("isWalking", false);
            }
        }
    }
}
