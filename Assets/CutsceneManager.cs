using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    public Image cutsceneImage; // The image for the cutscene sprite
    public Sprite[] cutsceneSprites; // Array of cutscene sprites
    public Button skipButton; // Button to skip or advance the cutscene
    public InputAction interactAction; // Interact action to advance cutscene
    public GameObject blackFadePanel; // Panel for the fade-to-black effect

    private int currentImageIndex = 0;

    void Start()
    {
        cutsceneImage.gameObject.SetActive(false); // Initially hide the cutscene image
        skipButton.gameObject.SetActive(false); // Hide the skip button at start
        blackFadePanel.SetActive(false); // Hide black fade panel at start
        interactAction.Disable(); // Disable interact action initially
    }

    public void StartCutsceneWithFade()
    {
        blackFadePanel.SetActive(true); // Enable black panel for fade
        StartCoroutine(FadeInFromBlack()); // Start fading from black to begin cutscene
    }

    void Update()
    {
        // When interact action is triggered and cutscene is visible, show the next image
        if (interactAction.triggered && cutsceneImage.gameObject.activeSelf)
        {
            ShowNextImage();
        }
    }

    void ShowImage()
    {
        if (currentImageIndex < cutsceneSprites.Length)
        {
            cutsceneImage.sprite = cutsceneSprites[currentImageIndex]; // Show the current image
        }
    }

    void ShowNextImage()
    {
        if (currentImageIndex < cutsceneSprites.Length - 1) // Check if it's not the last sprite
        {
            currentImageIndex++; // Increment to the next sprite
            ShowImage(); // Instantly show the next sprite
        }
        else
        {
            StartCoroutine(FadeToBlack()); // If it's the last sprite, fade to black
        }
    }

    IEnumerator FadeInFromBlack()
    {
        CanvasGroup canvasGroup = blackFadePanel.GetComponent<CanvasGroup>();
        float elapsedTime = 0f;
        float fadeDuration = 1f; // 1 second fade duration

        // Now show the first cutscene image
        cutsceneImage.gameObject.SetActive(true); // Enable the cutscene image
        skipButton.gameObject.SetActive(true); // Enable the skip button
        skipButton.onClick.AddListener(NextImage); // Button functionality to go to next image
        interactAction.Enable(); // Enable the interact action
        ShowImage(); // Show the first image of the cutscene


        // Fade from black to transparent
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration); // Fade out black screen
            yield return null;
        }

        canvasGroup.alpha = 0f; // Ensure it's fully transparent
        blackFadePanel.SetActive(false); // Disable the black fade panel after fade-out

        
    }

    IEnumerator FadeToBlack()
    {
        blackFadePanel.SetActive(true); // Show the black fade panel at the end
        CanvasGroup canvasGroup = blackFadePanel.GetComponent<CanvasGroup>();
        float elapsedTime = 0f;
        float fadeDuration = 1f; // 1 second fade duration

        // Fade to black
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration); // Fade to black
            yield return null;
        }

        canvasGroup.alpha = 1f; // Ensure it's fully black
        EndCutscene(); // End the cutscene after fade to black
    }

    void NextImage()
    {
        ShowNextImage(); // Go to the next image when the button is clicked
    }

    void EndCutscene()
    {
        cutsceneImage.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        interactAction.Disable();

        Time.timeScale = 1;
        blackFadePanel.SetActive(true);
        StartCoroutine(FadeOutToGameplay());

        // Show squirrel images after cutscene
        SquirrelTracker.instance.ShowSquirrelImages();
    }

    IEnumerator FadeOutToGameplay()
    {
        CanvasGroup canvasGroup = blackFadePanel.GetComponent<CanvasGroup>();
        float elapsedTime = 0f;
        float fadeDuration = 1f; // 1 second fade duration

        // Fade from black to transparent (back to gameplay)
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration); // Fade out black screen
            yield return null;
        }

        canvasGroup.alpha = 0f; // Ensure it's fully transparent
        blackFadePanel.SetActive(false); // Disable the black fade panel after fade-out
        Time.timeScale = 1; // Ensure game is resumed
    }

    private void OnDisable()
    {
        interactAction.Disable(); // Disable the interact action if the cutscene manager is disabled
    }
}
