using UnityEngine;
using System.Collections;

public class DumpsterSquirrelInteractable : MonoBehaviour
{
    public GameObject cutsceneText;
    public GameObject followText;
    public GameObject squirrel;
    public GameObject player;
    public float followDistance = 1.5f;
    public float followSpeed = 2f;

    private bool isFollowingPlayer = false;
    private bool cutsceneActive = false;

    private void Start()
    {
        if (cutsceneText != null) cutsceneText.SetActive(false);
        if (followText != null) followText.SetActive(false);
        isFollowingPlayer = false; // Ensure the squirrel doesn't follow immediately
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called with " + other.tag); // Debugging the trigger call

        if (other.CompareTag("Player") && !cutsceneActive)
        {
            StartCoroutine(TriggerCutscene());
        }
    }

    private IEnumerator TriggerCutscene()
    {
        cutsceneActive = true;

        if (player != null)
        {
            player.GetComponent<CharacterMovement>().enabled = false;
        }

        if (cutsceneText != null) cutsceneText.SetActive(true);
        yield return new WaitForSeconds(3f);
        if (cutsceneText != null) cutsceneText.SetActive(false);

        if (followText != null) followText.SetActive(true);
        yield return new WaitForSeconds(2f);
        if (followText != null) followText.SetActive(false);

        if (player != null)
        {
            player.GetComponent<CharacterMovement>().enabled = true;
        }

        yield return new WaitForSeconds(1f); // Ensure there's a short delay before following starts

        // Now enable following on the squirrel
        if (squirrel != null)
        {
            DumpsterSquirrelMovement movementScript = squirrel.GetComponent<DumpsterSquirrelMovement>();
            if (movementScript != null)
            {
                movementScript.isFollowingPlayer = true;  // Activate following in the DumpsterSquirrelMovement script
            }
            else
            {
                Debug.LogError("DumpsterSquirrelMovement script not found on squirrel!");
            }
        }
    }
}
