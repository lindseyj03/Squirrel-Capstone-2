using UnityEngine;

public class DumpsterSquirrelMovement : MonoBehaviour
{
    public GameObject player;         // Reference to the player
    public float followDistance = 2f; // Distance to follow the player
    public float followSpeed = 3f;    // Speed of following the player
    public Animator squirrelAnimator; // Animator for the squirrel

    public bool isFollowingPlayer = false; // Ensures the squirrel only follows when triggered

    void Update()
    {
        // Only check and execute follow logic if isFollowingPlayer is true
        if (isFollowingPlayer)
        {
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {
        // Only follow if the player is a certain distance away
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer > followDistance)
            {
                Vector3 direction = (player.transform.position - transform.position).normalized;
                transform.Translate(direction * followSpeed * Time.deltaTime, Space.World);

                // Optionally rotate to face the player (smooth rotation)
                RotateTowardsPlayer();

                // Trigger walk animation
                if (squirrelAnimator != null)
                {
                    squirrelAnimator.SetBool("isWalking", true);
                }
            }
            else
            {
                // Stop moving when close enough
                if (squirrelAnimator != null)
                {
                    squirrelAnimator.SetBool("isWalking", false);
                }
            }
        }
    }

    private void RotateTowardsPlayer()
    {
        if (player != null)
        {
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            directionToPlayer.y = 0; // Ensure it only rotates on the Y-axis

            if (directionToPlayer != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // Adjust speed as necessary
            }
        }
    }
}
