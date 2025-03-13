using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageFade : MonoBehaviour
{
    public Image img;
    public float duration = 0.05f; // Extremely fast reveal
    public float spinSpeed = 1440f; // Faster spin

    void Start()
    {
        img.transform.localScale = Vector3.zero; // Start tiny
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0f); // Start transparent
        StartCoroutine(Reveal());
    }

    IEnumerator Reveal()
    {
        float progress = 0f;
        while (progress < 1f)
        {
            progress = Mathf.Clamp01(progress + (Time.deltaTime / duration) * 2f); // Increase speed

            // Scale up
            img.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, progress);

            // Fade in
            img.color = new Color(img.color.r, img.color.g, img.color.b, progress);

            // Spin faster
            img.transform.Rotate(0, 0, spinSpeed * Time.deltaTime);

            yield return null;
        }

        // Instantly set final state
        img.transform.localScale = Vector3.one;
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
        img.transform.rotation = Quaternion.identity; // Reset spin
    }
}
