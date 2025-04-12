using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class BillboardTrigger : MonoBehaviour
{
    public TextMeshProUGUI promptText;
    public Sprite billboardSprite;
    public InputAction interactAction;
    public GameObject spriteContainer;

    private bool playerInRange = false;
    private bool isBillboardShowing = false;

    void Start()
    {
        promptText.gameObject.SetActive(false);
        spriteContainer.SetActive(false);
    }

    void OnEnable()
    {
        interactAction.performed += OnInteract;
        interactAction.Enable();
    }

    void OnDisable()
    {
        interactAction.performed -= OnInteract;
        interactAction.Disable();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (playerInRange && !isBillboardShowing)
        {
            ShowBillboard();
        }
    }

    private void ShowBillboard()
    {
        isBillboardShowing = true;
        promptText.gameObject.SetActive(false);
        spriteContainer.SetActive(true);
        spriteContainer.GetComponent<SpriteRenderer>().sprite = billboardSprite;

        Invoke(nameof(HideBillboard), 2f);  // Automatically hide after 3 seconds
    }

    private void HideBillboard()
    {
        spriteContainer.SetActive(false);
        isBillboardShowing = false;

        if (playerInRange)
        {
            promptText.gameObject.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (!isBillboardShowing)
            {
                promptText.text = "Press B to view the billboard";
                promptText.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            promptText.gameObject.SetActive(false);
        }
    }
}
