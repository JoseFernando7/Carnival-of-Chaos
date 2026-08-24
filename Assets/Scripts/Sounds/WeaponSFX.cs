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

    [Tooltip("Si está activo, silencia el sonido al inicio de la escena")]
    public bool muteOnAwake = true;

    void Awake()
    {
        // Silencia la fuente al inicio para evitar el estruendo de OnEnable
        if (muteOnAwake && weaponAudioSource != null)
        {
            weaponAudioSource.mute = true;
        }
    }

    void OnEnable()
    {
        // El script sigue funcionando si se activa/desactiva,
        // pero ahora la animación será la principal encargada.
        ReproducirSonidoArma();
    }

    // --- ESTA ES LA FUNCIÓN IMPORTANTE PARA LA ANIMACIÓN ---
    // Método público que permite ser llamado desde eventos de animación (Animation Events)
    public void ReproducirSonidoArma()
    {
        if (weaponAudioSource != null && weaponAudioSource.clip != null)
        {
            // Primero, asegúrate de desmutear si estaba silenciado por Awake
            weaponAudioSource.mute = false;

            float masterSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
            weaponAudioSource.volume = sfxVolume * masterSFXVolume;
            
            // Usamos PlayOneShot para que el sonido no se corte si el arma se oculta rápido
            weaponAudioSource.PlayOneShot(weaponAudioSource.clip);
        }
    }
}

//Script no terminado,  implementando sonido de armas