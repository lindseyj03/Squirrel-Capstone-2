using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SquirrelTracker : MonoBehaviour
{
    public static SquirrelTracker instance;
    private int squirrelsFound = 0;
    private int totalSquirrels = 4;

    public GameObject exitTunnel;
    public Image[] squirrelImages;
    public Text tunnelMessage; // Reference to the UI Text element for messages

    public int SquirrelsFound => squirrelsFound;
    public int TotalSquirrels => totalSquirrels;

    private void Start()
    {
        HideSquirrelImages();
    }

    public void HideSquirrelImages()
    {
        foreach (var img in squirrelImages)
        {
            img.gameObject.SetActive(false); // Hide all images at the start
        }
    }

    public void ShowSquirrelImages()
    {
        foreach (var img in squirrelImages)
        {
            img.gameObject.SetActive(true); // Show images after cutscene
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Ensure the message is hidden at start
        if (tunnelMessage != null)
        {
            tunnelMessage.gameObject.SetActive(false);
        }
    }

    public void FoundSquirrel(string squirrelName)
    {
        squirrelsFound++;
        Debug.Log("Squirrels found: " + squirrelsFound + "/" + totalSquirrels);
        UpdateSquirrelImages();
        CheckIfAllFound();
    }

    private void UpdateSquirrelImages()
    {
        foreach (var img in squirrelImages)
        {
            img.gameObject.SetActive(false);
        }

        if (squirrelsFound == 0)
        {
            if (squirrelImages.Length > 0)
            {
                squirrelImages[0].gameObject.SetActive(true);
            }
            return;
        }

        for (int i = 0; i < squirrelsFound; i++)
        {
            if (i < squirrelImages.Length)
            {
                squirrelImages[i].gameObject.SetActive(true);
            }
        }
    }

    public void TryToGoToTunnel()
    {
        if (squirrelsFound >= totalSquirrels)
        {
            // All squirrels found, start the ending cutscene
            EndingCutsceneManager.instance.StartEndingCutscene();
        }
        else
        {
            // Not all squirrels found, show a message
            if (tunnelMessage != null)
            {
                tunnelMessage.gameObject.SetActive(true);
                tunnelMessage.text = "You can't leave yet! Find all the squirrels first!";
                tunnelMessage.color = Color.white;
            }
        }
    }


    private void CheckIfAllFound()
    {
        if (squirrelsFound >= totalSquirrels)
        {
            Debug.Log("All squirrels found! Tunnel unlocked.");
            if (exitTunnel != null)
            {
                exitTunnel.SetActive(true);
            }

            // Show the tunnel message
            if (tunnelMessage != null)
            {
                tunnelMessage.gameObject.SetActive(true);
                tunnelMessage.text = "All squirrels found! Go to the tunnel!";
                tunnelMessage.color = Color.white; // Set the text color to white
            }
        }
    }

    public void HideTrackerUI()
    {
        HideSquirrelImages();

        if (tunnelMessage != null)
        {
            tunnelMessage.gameObject.SetActive(false);
        }
    }

}
