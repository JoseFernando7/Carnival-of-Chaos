using System.Collections;
using UnityEngine;

public class UIButtonSound : MonoBehaviour
{
    [Header("Configuración de Audio")]
    [Tooltip("Sonido de selección botones")]
    public AudioSource sfxSource;

    [Header("Ajustes de Respuesta y Volumen")]
    [Range(0f, 1f)]
    [Tooltip("Volumen del sonido de clic")]
    public float clickVolume = 0.3f;

    [Tooltip("Duración máxima del sonidO antes de cortarlo")]
    public float maxSoundDuration = 0.2f;

    // Método OnClick() del botón
    public void ReproducirSonidoBoton()
    {
        if (sfxSource != null && sfxSource.clip != null)
        {
            // Detener sonido previo para respuesta instantánea
            StopAllCoroutines();
            sfxSource.Stop();

            // Asignar el volumen del clic
            sfxSource.volume = clickVolume;

            // Reproducir el sonido de inmediato
            sfxSource.Play();

            // Cortar el audio 
            StartCoroutine(CutAudioShort());
        }
    }

    IEnumerator CutAudioShort()
    {
        // Espera la duración deseada para el clic
        yield return new WaitForSeconds(maxSoundDuration);

        // Detener el audiO
        if (sfxSource != null && sfxSource.isPlaying)
        {
            sfxSource.Stop();
        }
    }
}