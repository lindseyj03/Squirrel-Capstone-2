using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class EndingCutsceneManager : MonoBehaviour
{
    public static EndingCutsceneManager instance; // Add this static instance
    public Image cutsceneImage; // The image for the cutscene sprite
    public Sprite[] cutsceneSprites; // Array of cutscene sprites for the ending
    public GameObject blackFadePanel; // Panel for the fade-to-black effect
    public TextMeshProUGUI endMessageText; // Use TextMeshProUGUI for the end message text

    private int currentImageIndex = 0;

    void Awake()
    {
        // Ensure only one instance exists and make it accessible globally
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        cutsceneImage.gameObject.SetActive(false); // Initially hide the cutscene image
        blackFadePanel.SetActive(false); // Hide black fade panel at start
        endMessageText.gameObject.SetActive(false); // Hide end message text at the start
    }

    public void StartEndingCutscene()
    {
        StartCoroutine(FadeInFromBlack());
    }

    IEnumerator FadeInFromBlack()
    {
        // Fade from black to reveal the cutscene
        CanvasGroup canvasGroup = blackFadePanel.GetComponent<CanvasGroup>();
        float elapsedTime = 0f;
        float fadeDuration = 1f; // Fade duration

        blackFadePanel.SetActive(true); // Show black panel at the start

        // Fade in from black
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        blackFadePanel.SetActive(false); // Hide the black panel

        // Start the cutscene images
        cutsceneImage.gameObject.SetActive(true);
        ShowImage();

        // Wait a moment before showing each image
        yield return new WaitForSeconds(1f);
        ShowNextImage();
    }

    void ShowImage()
    {
        if (currentImageIndex < cutsceneSprites.Length)
        {
            cutsceneImage.sprite = cutsceneSprites[currentImageIndex];
        }
    }

    void ShowNextImage()
    {
        if (currentImageIndex < cutsceneSprites.Length - 1)
        {
            currentImageIndex++; // Go to the next image in the array
            ShowImage();
            StartCoroutine(WaitBeforeNextImage());
        }
        else
        {
            // End the cutscene and show the message
            StartCoroutine(FadeToBlack());
        }
    }

    IEnumerator WaitBeforeNextImage()
    {
        // Wait for a short time before showing the next image
        yield return new WaitForSeconds(1.5f);
        ShowNextImage();
    }

    IEnumerator FadeToBlack()
    {
        // Fade to black before ending the game
        CanvasGroup canvasGroup = blackFadePanel.GetComponent<CanvasGroup>();
        float elapsedTime = 0f;
        float fadeDuration = 1f; // Fade duration

        blackFadePanel.SetActive(true); // Show black panel for fade

        // Fade to black
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;

        // Show the ending message once the cutscene is over
        endMessageText.gameObject.SetActive(true);
        endMessageText.text = "All squirrels found! You win!";

        // Here, you can add code to end the game, load a new scene, or show an end screen
        EndGame();
    }

    void EndGame()
    {
        // End the game (You can load a new scene, show a "game over" screen, etc.)
        Debug.Log("Game Over! You found all the squirrels!");
        // Example: Load an end game scene or show a message
        // SceneManager.LoadScene("EndGameScene");
    }
}
