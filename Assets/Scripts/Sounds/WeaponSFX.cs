using UnityEngine;

public class WeaponSFX : MonoBehaviour
{
    [Header("Configuración de Audio")]
    [Tooltip("AudioSource del arma")]
    public AudioSource weaponAudioSource;

    [Header("Ajustes de Volumen")]
    [Range(0f, 1f)]
    [Tooltip("Volumen del arma respecto al máster de SFX")]
    public float sfxVolume = 0.5f;

    public void ReproducirSonidoArma()
    {
        if (weaponAudioSource != null && weaponAudioSource.clip != null)
        {
            float masterSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
            weaponAudioSource.volume = sfxVolume * masterSFXVolume;
            weaponAudioSource.PlayOneShot(weaponAudioSource.clip);
        }
    }
}