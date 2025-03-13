using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class VendingMachineInteractable : MonoBehaviour
{
    public GameObject interactionPrompt;
    public GameObject additionalText;
    public GameObject taskListText; // Dialog Text (appears and disappears)
    public GameObject squirrel;
    public GameObject player;
    public GameObject permanentTaskList; // New task list that stays on screen

    public int requiredShakes = 3;
    public float additionalTextDuration = 4f;
    public float shakeAngle = 10f;
    public float shakeSpeed = 0.2f;
    public float resetSpeed = 0.3f;
    public float fallSpeed = 1.0f; // Speed of the fall animation
    public float squirrelThrowForce = 5f; // Force to throw the squirrel out
    public float squirrelWalkSpeed = 1.5f; // Speed at which squirrel walks away

//NEW 1/27
    //private Animator squirrelAnimator;  
    public float followDistance = 1.5f; // Distance at which squirrel stops following
    public float followSpeed = 2f; // Speed at which squirrel follows the player
    private bool isFollowingPlayer = false; // Check if the squirrel should follow the player
//END OF NEW 1/27


    private int shakeCount = 0;
    private bool isPlayerNearby = false;
    private bool machineUnlocked = false;
    private bool fallStarted = false; // New flag to track if the fall has started
    private Rigidbody vendingMachineRigidbody;
    private bool cutsceneActive = false;

    public InputAction interactAction;

    private void Start()
    {
        vendingMachineRigidbody = GetComponentInParent<Rigidbody>(); // Get the Rigidbody on the parent

        //// Assign the squirrel's Animator
        //    if (squirrel != null)
        //    {
        //        squirrelAnimator = squirrel.GetComponent<Animator>();
        //    }

        // Initially hide all UI elements
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
        if (additionalText != null) additionalText.SetActive(false);
        if (taskListText != null) taskListText.SetActive(false);
        if (squirrel != null) squirrel.SetActive(false);
        if (permanentTaskList != null) permanentTaskList.SetActive(false); // Hide permanent task list initially

        // Disable gravity and set kinematic to true initially
        vendingMachineRigidbody.isKinematic = true;
        vendingMachineRigidbody.useGravity = false;
    }

    private void OnEnable()
    {
        interactAction.Enable();
    }

    private void OnDisable()
    {
        interactAction.Disable();
    }
private void Update()
{
    // Handle player interaction with the vending machine
    if (isPlayerNearby && interactAction.triggered && !fallStarted)
    {
        if (!machineUnlocked)
        {
            ShakeVendingMachine();
        }
    }

    // Handle cutscene events
    if (cutsceneActive && squirrel != null)
    {
        // Logic for when the squirrel is talking (you can trigger animations or voice lines here)
    }

    //// Make the squirrel follow the player if following is enabled
    //if (isFollowingPlayer && player != null)
    //{
    //    FollowPlayer();
    //}
}

    // private void Update()
    // {
    //     if (isPlayerNearby && interactAction.triggered && !fallStarted)
    //     {
    //         if (!machineUnlocked)
    //         {
    //             ShakeVendingMachine();
    //         }
    //     }

    //     if (cutsceneActive && squirrel != null)
    //     {
    //         // Logic for when the squirrel is talking (you can trigger animations or voice lines here)
    //     }
    // }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            if (interactionPrompt != null && !fallStarted) interactionPrompt.SetActive(true); // Show prompt only if fall hasn't started
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (interactionPrompt != null && !fallStarted) interactionPrompt.SetActive(false); // Hide prompt only if fall hasn't started
        }
    }

    private void ShakeVendingMachine()
    {
        shakeCount++;

        // Show the additional text (dialog) only after the first shake
        if (shakeCount == 1 && taskListText != null)
        {
            taskListText.SetActive(true); // Show the dialog text
            Invoke(nameof(HideTaskListText), additionalTextDuration); // Hide dialog text after a delay
        }

        // Hide the interaction prompt immediately when the player presses "A"
        if (interactionPrompt != null) interactionPrompt.SetActive(false);

        StartCoroutine(RockMachine());

        if (shakeCount >= requiredShakes)
        {
            StartCoroutine(FallOver());
        }
    }

    private IEnumerator RockMachine()
    {
        Quaternion originalRotation = transform.rotation;

        // Shake around the center of the vending machine (original pivot)
        Quaternion leftTilt = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -shakeAngle);
        Quaternion rightTilt = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, shakeAngle);

        transform.rotation = Quaternion.Lerp(originalRotation, leftTilt, shakeSpeed);
        yield return new WaitForSeconds(shakeSpeed);

        transform.rotation = Quaternion.Lerp(originalRotation, rightTilt, shakeSpeed);
        yield return new WaitForSeconds(shakeSpeed);

        transform.rotation = Quaternion.Lerp(originalRotation, originalRotation, resetSpeed); // Reset to original
    }

    private IEnumerator FallOver()
    {
        fallStarted = true; // Mark that the fall has started

        // Immediately hide the interaction prompt and additional text once the fall begins
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
        if (taskListText != null) taskListText.SetActive(false); // Hide dialog text when fall begins

        machineUnlocked = true;

        // Disable physics for smooth manual rotation
        vendingMachineRigidbody.isKinematic = true;
        vendingMachineRigidbody.useGravity = false;

        // Smoothly rotate the vending machine over time
        Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 90f);
        Quaternion startRotation = transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < fallSpeed)
        {
            // Interpolate between the start and target rotation smoothly
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / fallSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final rotation is exactly 90 degrees on the Z-axis
        transform.rotation = targetRotation;

        // Freeze the position and rotation to prevent the player from pushing it
        vendingMachineRigidbody.constraints = RigidbodyConstraints.FreezeAll;

        // Add a delay before throwing the squirrel to give time for the fall
        yield return new WaitForSeconds(0.1f); // Adjust the delay if necessary

        // Now trigger the squirrel fall out with force
        SquirrelFallOut();

        // Delay before starting the cutscene
        yield return new WaitForSeconds(0.1f);

        // Enable the squirrel and show the new task list after the fall
        if (squirrel != null) squirrel.SetActive(true);
        StartCoroutine(PlayCutscene()); // Start cutscene

        // Show the permanent task list after dialog text disappears
        yield return new WaitForSeconds(additionalTextDuration); // Wait for dialog text to finish
        if (permanentTaskList != null) permanentTaskList.SetActive(true); // Show permanent task list
    }

    private void HideTaskListText()
    {
        if (taskListText != null) taskListText.SetActive(false); // Hide dialog text after duration
    }

    // This method hides the additional text after the duration
    private void HideAdditionalText()
    {
        if (additionalText != null) additionalText.SetActive(false); // Hide the additional text after duration
    }
    private void SquirrelFallOut()
    {
        // Make sure the squirrel is deactivated initially
        if (squirrel != null)
        {
            squirrel.SetActive(true);  // Make squirrel visible immediately

            Rigidbody squirrelRb = squirrel.GetComponent<Rigidbody>();
            if (squirrelRb != null)
            {
                // Ensure the Rigidbody is set up correctly to interact with physics
                squirrelRb.isKinematic = false; // Allow physics to apply
                squirrelRb.useGravity = true;   // Enable gravity to make it fall

                // Get direction towards the player (this is the direction we will throw the squirrel)
                Vector3 directionToPlayer = (player.transform.position - squirrel.transform.position).normalized;

                // Apply an impulse to throw the squirrel out towards the player
                squirrelRb.AddForce(directionToPlayer * squirrelThrowForce, ForceMode.Impulse);
            }

            // Rotate the squirrel to face the player after being thrown
            StartCoroutine(RotateSquirrelToFacePlayer());
        }
    }

    // Coroutine to rotate squirrel towards the player
    private IEnumerator RotateSquirrelToFacePlayer()
    {
        if (squirrel != null)
        {
            Vector3 directionToPlayer = player.transform.position - squirrel.transform.position;

            // Calculate the rotation to make the squirrel face the player
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            // Smoothly rotate the squirrel to face the player
            float rotationSpeed = 2f; // Adjust speed as needed
            while (Quaternion.Angle(squirrel.transform.rotation, targetRotation) > 1f)
            {
                squirrel.transform.rotation = Quaternion.Slerp(squirrel.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                yield return null;
            }
        }

        // After rotating, start the cutscene or show text
        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        cutsceneActive = true;

        // Disable player movement during the cutscene
        if (player != null)
        {
            player.GetComponent<CharacterMovement>().enabled = false; // Disable movement
        }

        // Optionally, add any animations or dialogues for the squirrel here before starting the text
        yield return new WaitForSeconds(1f);  // Adjust this to wait for the squirrel to finish turning

        // Show the dialog text after the squirrel has rotated
        if (additionalText != null) additionalText.SetActive(true);
        Invoke(nameof(HideAdditionalText), additionalTextDuration); // Hide it after a delay

        // Wait a bit before continuing the rest of the cutscene
        yield return new WaitForSeconds(1f);

        // Enable player movement again once the squirrel is done talking
        if (player != null)
        {
            player.GetComponent<CharacterMovement>().enabled = true; // Re-enable movement
        }

        // Start following the player instead of walking away
        Invoke(nameof(StartFollowingPlayer), 1f); // Delay before following starts
    }

private void StartFollowingPlayer()
{
    isFollowingPlayer = true; // Enable following
}

// private void Update()
// {
//     if (isFollowingPlayer && player != null)
//     {
//         FollowPlayer();
//     }
// }


//MOVED to VendingSquirrelMovement
//private void FollowPlayer()
//{
//    if (squirrel == null || player == null) return;

//    float distanceToPlayer = Vector3.Distance(squirrel.transform.position, player.transform.position);

//    if (distanceToPlayer > followDistance)
//    {
//        // Move towards the player
//        Vector3 direction = (player.transform.position - squirrel.transform.position).normalized;
//        squirrel.transform.position += direction * followSpeed * Time.deltaTime;

//        // Set walking animation if Animator is attached
//        if (squirrelAnimator != null)
//        {
//            squirrelAnimator.SetBool("isWalking", true);
//        }
//    }
//    else
//    {
//        // Stop moving when close enough
//        if (squirrelAnimator != null)
//        {
//            squirrelAnimator.SetBool("isWalking", false);
//        }
//    }
//}


    // private IEnumerator SquirrelWalkAway()
    // {
    //     Vector3 startPosition = squirrel.transform.position;
    //     Vector3 endPosition = new Vector3(-10f, squirrel.transform.position.y, squirrel.transform.position.z); // Exit point

    //     float elapsedTime = 0f;

    //     while (elapsedTime < 5f) // Time it takes to walk away
    //     {
    //         squirrel.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / 5f);
    //         elapsedTime += Time.deltaTime;
    //         yield return null;
    //     }

    //     squirrel.transform.position = endPosition;

    //     // Optionally, you can disable the squirrel once it reaches the exit
    //     if (squirrel != null)
    //     {
    //         squirrel.SetActive(false); // Squirrel exits the scene
    //     }
    // }
}
