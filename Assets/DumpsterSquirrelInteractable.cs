using UnityEngine;
using System.Collections;

public class DumpsterSquirrelInteractable : MonoBehaviour
{
    public GameObject cutsceneText;
    public GameObject followText;
    public GameObject squirrel;
    public GameObject player;
    private bool cutsceneActive = false;

    private void Start()
    {
        if (cutsceneText != null) cutsceneText.SetActive(false);
        if (followText != null) followText.SetActive(false);
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

        // Disable player movement during cutscene
        if (player != null)
        {
            player.GetComponent<CharacterMovement>().enabled = false;
        }

        // Shorter cutscene for testing purposes
        if (cutsceneText != null) cutsceneText.SetActive(true);
        yield return new WaitForSeconds(1f); // Shorten cutscene time for debugging

        if (cutsceneText != null) cutsceneText.SetActive(false);

        // Enable following behavior immediately after cutscene
        if (squirrel != null)
        {
            DumpsterSquirrelMovement movementScript = squirrel.GetComponent<DumpsterSquirrelMovement>();
            if (movementScript != null)
            {
                movementScript.isFollowingPlayer = true; // Enable squirrel follow behavior
            }
        }

        // Re-enable player movement after cutscene
        if (player != null)
        {
            player.GetComponent<CharacterMovement>().enabled = true;
        }
    }
}


//using UnityEngine;
//using System.Collections;

//public class DumpsterSquirrelInteractable : MonoBehaviour
//{
//    public GameObject cutsceneText;
//    public GameObject followText;
//    public GameObject squirrel;
//    public GameObject player;
//    public float followDistance = 1.5f;
//    public float followSpeed = 2f;

//    private bool isFollowingPlayer = false;
//    private bool cutsceneActive = false;

//    private void Start()
//    {
//        if (cutsceneText != null) cutsceneText.SetActive(false);
//        if (followText != null) followText.SetActive(false);
//        isFollowingPlayer = false; // Ensure the squirrel doesn't follow immediately
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        Debug.Log("OnTriggerEnter called with " + other.tag); // Debugging the trigger call

//        if (other.CompareTag("Player") && !cutsceneActive)
//        {
//            StartCoroutine(TriggerCutscene());
//        }
//    }

//    private IEnumerator TriggerCutscene()
//    {
//        cutsceneActive = true;

//        // Disable player movement during cutscene
//        if (player != null)
//        {
//            player.GetComponent<CharacterMovement>().enabled = false;
//        }

//        // Shorter cutscene for testing purposes
//        if (cutsceneText != null) cutsceneText.SetActive(true);
//        yield return new WaitForSeconds(1f); // Shorten cutscene time for debugging

//        if (cutsceneText != null) cutsceneText.SetActive(false);

//        // Enable following behavior immediately after cutscene
//        if (squirrel != null)
//        {
//            DumpsterSquirrelMovement movementScript = squirrel.GetComponent<DumpsterSquirrelMovement>();
//            if (movementScript != null)
//            {
//                movementScript.isFollowingPlayer = true;
//            }
//            //else
//            //{
//            //    Debug.LogError("DumpsterSquirrelMovement script not found on squirrel!");
//            //}
//        }

//        // Re-enable player movement after cutscene
//        if (player != null)
//        {
//            player.GetComponent<CharacterMovement>().enabled = true;
//        }
//    }
//}
