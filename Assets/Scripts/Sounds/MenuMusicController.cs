using System.Collections;
using UnityEngine;

public class MenuMusicController : MonoBehaviour
{
    [Header("Audio Sources")]
    [Tooltip("Sonido del presentador")]
    public AudioSource introSource;

    [Tooltip("Sonido de Tema menu")]
    public AudioSource loopSource;

    [Header("Ajustes de Volumen Intro")]
    [Range(0f, 1f)]
    [Tooltip("Volumen inicial del tema menu")]
    public float loopStartVolume = 0.25f;

    [Range(0f, 1f)]
    [Tooltip("Volumen normal del tema menu")]
    public float loopTargetVolume = 1.0f;

    [Tooltip("Tiempo en segundos que tarda la música en subir de volumen")]
    public float fadeDuration = 1.5f;

    private float currentVolumeFactor = 0.25f; // fade actual
    private bool isFadingFinished = false;

    void Start()
    {
        currentVolumeFactor = loopStartVolume;

        if (introSource != null && loopSource != null)
        {
            // Aplica volumen inicial mezclando el factor de intro con el slider de Ajustes
            AplicarVolumenes();

            // Reproducir ambos audios
            introSource.Play();
            loopSource.Play();

            // Espera para subir el volumen cuando la voz termine
            StartCoroutine(WaitForPresenterEnd());
        }
        else if (loopSource != null)
        {
            currentVolumeFactor = loopTargetVolume;
            isFadingFinished = true;
            AplicarVolumenes();
            loopSource.Play();
        }
    }

    void Update()
    {
        // Mantiene actualizado el volumen si el jugador mueve el audio en Ajustes
        AplicarVolumenes();
    }

    private void AplicarVolumenes()
    {
        // Lee la configuración global de audio
        float masterMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);

        if (introSource != null)
        {
            introSource.volume = masterMusicVolume;
        }

        if (loopSource != null)
        {
            // Terminar fade y usar ajuste de audio
            float targetFactor = isFadingFinished ? loopTargetVolume : currentVolumeFactor;
            loopSource.volume = targetFactor * masterMusicVolume;
        }
    }

    IEnumerator WaitForPresenterEnd()
    {
        // Espera la duración de la voz del presentador
        yield return new WaitForSeconds(introSource.clip.length);

        float currentTime = 0f;
        float startVol = loopStartVolume;

        // Transición progresiva hacia el volumen normal
        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            currentVolumeFactor = Mathf.Lerp(startVol, loopTargetVolume, currentTime / fadeDuration);
            yield return null;
        }

        currentVolumeFactor = loopTargetVolume;
        isFadingFinished = true;
    }
}