using UnityEngine;

public class CardButtonSound : MonoBehaviour
{
    [Header("Configuración de Audio")]
    [Tooltip("Sonido de la tarjeta asignado)")]
    public AudioSource cardAudioSource;

    [Header("Ajustes de Volumen")]
    [Range(0f, 1f)]
    [Tooltip("Volumen de la tarjeta")]
    public float cardVolume = 0.5f;

    // Método para llamar desde el OnClick() de cada carta
    public void ReproducirSonidoTarjeta()
    {
        if (cardAudioSource != null && cardAudioSource.clip != null)
        {
            float masterSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
            cardAudioSource.volume = cardVolume * masterSFXVolume;
            
            // Reproduce el clip asignado directamente en ese AudioSource
            cardAudioSource.PlayOneShot(cardAudioSource.clip);
        }
    }
}