using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup logoGroup;

    [Header("Timing")]
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float displayDuration = 2f;
    [SerializeField] private float fadeOutDuration = 1f;

    [Header("Scene")]
    [SerializeField] private string nextSceneName = "Menu";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(PlaySplash());
    }

    private IEnumerator PlaySplash()
    {
        logoGroup.alpha = 0f;

        // Fade In
        yield return Fade(0f, 1f, fadeInDuration);

        // Display logo
        yield return new WaitForSeconds(displayDuration);

        // Fade out
        yield return Fade(1f, 0f, fadeOutDuration);

        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float progress = elapsed / duration;

            logoGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);

            yield return null;
        }

        // Make sure that ends in estimated value
        logoGroup.alpha = endAlpha;
    }
}
