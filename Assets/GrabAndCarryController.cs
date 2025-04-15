using UnityEngine;
using UnityEngine.InputSystem;

public class GrabAndCarryWithInputAction : MonoBehaviour
{
    public InputAction interactAction; // Input action for interaction (e.g., pressing a button)
    public Transform carryPoint; // The point to which the object will be carried
    public float grabDistance = 2f; // Distance to check for the grabbable object

    private GameObject carriedObject; // The object currently being carried
    private Rigidbody carriedObjectRb; // The Rigidbody of the carried object
    private bool isInteracting = false; // State to prevent constant grabbing/dropping

    private void OnEnable()
    {
        interactAction.Enable();
    }

    private void OnDisable()
    {
        interactAction.Disable();
    }

    void Update()
    {
        // Only handle interaction if not in the middle of a grab/drop action
        if (interactAction.triggered && !isInteracting)
        {
            if (carriedObject == null)
            {
                TryGrab();
            }
            else
            {
                Drop();
            }
        }
    }

    void TryGrab()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, grabDistance))
        {
            if (hit.collider.CompareTag("Grabbable"))
            {
                carriedObject = hit.collider.gameObject;
                carriedObjectRb = carriedObject.GetComponent<Rigidbody>();

                // Turn off physics and collider to prevent unwanted behavior
                carriedObjectRb.isKinematic = true;
                carriedObject.GetComponent<Collider>().enabled = false;

                carriedObject.transform.SetParent(carryPoint); // Parent to carry point
                carriedObject.transform.localPosition = Vector3.zero;
                carriedObject.transform.localRotation = Quaternion.identity;

                // Set interaction state to true to prevent re-triggering the grab immediately
                isInteracting = true;

                // Optionally, add a delay before the next interaction is allowed
                Invoke("ResetInteractionState", 0.2f); // Reset interaction state after 0.2 seconds
            }
        }
    }

    void Drop()
    {
        if (carriedObject != null)
        {
            carriedObject.transform.SetParent(null); // Detach object from carry point
            carriedObjectRb.isKinematic = false; // Enable physics again
            carriedObject.GetComponent<Collider>().enabled = true; // Re-enable collider

            // Optionally, apply a force to "throw" the object
            carriedObjectRb.AddForce(transform.forward * 2f, ForceMode.Impulse);

            // Reset carried object and Rigidbody
            carriedObject = null;
            carriedObjectRb = null;

            // Set interaction state to true to prevent immediate re-triggering
            isInteracting = true;

            // Optionally, add a delay before the next interaction is allowed
            Invoke("ResetInteractionState", 0.2f); // Reset interaction state after 0.2 seconds
        }
    }

    // Function to reset the interaction state after a short delay
    void ResetInteractionState()
    {
        isInteracting = false;
    }
}
