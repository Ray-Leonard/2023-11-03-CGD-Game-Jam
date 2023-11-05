using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeIn : MonoBehaviour
{
    public float fadeDuration = 2.0f; // Duration of the fade-in effect in seconds
    public Image uiImage; // Reference to the UI Image component

    private float initialAlpha; // Initial alpha value of the Image component

    private void Start()
    {
        // Store the initial alpha value
        initialAlpha = uiImage.color.a;

        // Make sure the UI element is initially transparent
        Color initialColor = uiImage.color;
        initialColor.a = 0;
        uiImage.color = initialColor;

        // Start the fade-in effect
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            // Calculate the alpha value based on the elapsed time
            float alpha = Mathf.Lerp(0, initialAlpha, elapsedTime / fadeDuration);

            // Apply the alpha value to the Image color
            Color newColor = uiImage.color;
            newColor.a = alpha;
            uiImage.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final alpha value is exactly the initial alpha
        Color finalColor = uiImage.color;
        finalColor.a = initialAlpha;
        uiImage.color = finalColor;
    }
}
