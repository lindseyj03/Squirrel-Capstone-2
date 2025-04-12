using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndingCutsceneManager : MonoBehaviour
{
    public static EndingCutsceneManager instance;

    public Image cutsceneImage;
    public Sprite[] cutsceneSprites;
    public GameObject blackFadePanel;
    public Button skipButton;
    public InputAction interactAction;

    private int currentImageIndex = 0;
    private bool cutsceneActive = false;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        cutsceneImage.gameObject.SetActive(false);
        blackFadePanel.SetActive(true);
        skipButton.gameObject.SetActive(false);
        interactAction.Disable();
    }

    void OnEnable()
    {
        interactAction.Enable();
        interactAction.performed += OnInteract;
    }

    void OnDisable()
    {
        interactAction.performed -= OnInteract;
        interactAction.Disable();
    }

    void Start()
    {
        skipButton.onClick.AddListener(NextImage);
    }

    public void StartEndingCutscene()
    {
        // Hide squirrel UI before beginning the cutscene
        if (SquirrelTracker.instance != null)
        {
            SquirrelTracker.instance.HideTrackerUI();
        }

        StartCoroutine(FadeInFromBlack());
    }

    IEnumerator FadeInFromBlack()
    {
        CanvasGroup canvasGroup = blackFadePanel.GetComponent<CanvasGroup>();
        float elapsedTime = 0f;
        float fadeDuration = 1f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        blackFadePanel.SetActive(false);

        cutsceneImage.gameObject.SetActive(true);
        skipButton.gameObject.SetActive(true);
        cutsceneActive = true;

        ShowImage();
    }

    void ShowImage()
    {
        if (currentImageIndex < cutsceneSprites.Length)
        {
            cutsceneImage.sprite = cutsceneSprites[currentImageIndex];
        }
    }

    public void NextImage()
    {
        if (!cutsceneActive) return;

        if (currentImageIndex < cutsceneSprites.Length - 1)
        {
            currentImageIndex++;
            ShowImage();
        }
        else
        {
            StartCoroutine(FadeToBlack());
        }
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        NextImage();
    }

    IEnumerator FadeToBlack()
    {
        cutsceneActive = false;
        interactAction.Disable();
        skipButton.gameObject.SetActive(false);

        // Fade to black effect
        CanvasGroup canvasGroup = blackFadePanel.GetComponent<CanvasGroup>();
        blackFadePanel.SetActive(true); // Activate the black fade panel
        float elapsedTime = 0f;
        float fadeDuration = 1f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f; // Ensure it's fully black.

        // Now the screen is fully black, and the UI elements like cutscene image are hidden.
        cutsceneImage.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);

        // After fading to black, we don't do anything else until the game resets.
        // We will wait for the game reset here.
        StartCoroutine(WaitForReset()); // This will wait for a moment before resetting the scene.
    }

    IEnumerator WaitForReset()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds (or any duration you want) before resetting.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reset the game by reloading the scene.
    }

    void EndCutscene()
    {
        Debug.Log("Game Over! You found all the squirrels!");
        // Reset the image index to 0 so the cutscene restarts at the first image.
        currentImageIndex = 0;
        StartCoroutine(FadeOutToGameplay());
    }

    IEnumerator FadeOutToGameplay()
    {
        CanvasGroup canvasGroup = blackFadePanel.GetComponent<CanvasGroup>();
        float elapsedTime = 0f;
        float fadeDuration = 1f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        blackFadePanel.SetActive(false);

        // Optional: trigger something else here like a results screen or credits
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

}
