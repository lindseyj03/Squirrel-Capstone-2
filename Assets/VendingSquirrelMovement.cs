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

                // Rotate to face the player smoothly
                RotateTowardsPlayer();

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

    private void RotateTowardsPlayer()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        directionToPlayer.y = 0; // Keep rotation only on the Y-axis

        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // Faster rotation
        }
    }
}