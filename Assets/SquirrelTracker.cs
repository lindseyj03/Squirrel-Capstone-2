using UnityEngine;
using UnityEngine.UI;

public class SquirrelTracker : MonoBehaviour
{
    public static SquirrelTracker instance;
    private int squirrelsFound = 0;
    private int totalSquirrels = 4;

    public GameObject exitTunnel;

    // Array of Image references for each squirrel found
    public Image[] squirrelImages;

    // Public getter for squirrelsFound
    public int SquirrelsFound
    {
        get { return squirrelsFound; }
    }

    // Public getter for totalSquirrels
    public int TotalSquirrels
    {
        get { return totalSquirrels; }
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
        // Hide all images first
        foreach (var img in squirrelImages)
        {
            img.gameObject.SetActive(false);
        }

        // Show the images for the squirrels found so far
        for (int i = 0; i < squirrelsFound; i++)
        {
            if (i < squirrelImages.Length)
            {
                squirrelImages[i].gameObject.SetActive(true); // Show the image for the squirrel
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
        }
    }
}
