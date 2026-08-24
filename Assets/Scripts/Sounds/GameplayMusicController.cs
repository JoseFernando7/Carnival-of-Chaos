using System.Collections;
using UnityEngine;

public class GameplayMusicController : MonoBehaviour
{
    [Header("Audio Sources")]
    [Tooltip("Sonido de inicio de partida")]
    public AudioSource matchStartSource;

    [Tooltip("Música principal de fondo")]
    public AudioSource gameplayLoopSource;

    [Tooltip("Música al Ganar la partida")]
    public AudioSource victorySource;

    [Tooltip("Música al Perder la partida")]
    public AudioSource defeatSource;

    [Header("Ajustes de Volumen Finales")]
    [Range(0f, 1f)]
    [Tooltip("Volumen de las pantallas finales (Ganar/Perder)")]
    public float endScreensVolume = 0.4f;

    void Start()
    {
        if (matchStartSource != null && gameplayLoopSource != null)
        {
            // Aplica los volúmenes leyendo los Ajustes del jugador
            AplicarVolumenes();

            // Reproduce el sonido de inicio de partida
            matchStartSource.Play();

            // Espera a que termine la intro para iniciar la música del juego
            StartCoroutine(WaitForStartSoundEnd());
        }
        else if (gameplayLoopSource != null)
        {
            AplicarVolumenes();
            gameplayLoopSource.loop = true;
            gameplayLoopSource.Play();
        }
    }

    void Update()
    {
        // Mantiene actualizado el volumen desde Ajustes
        AplicarVolumenes();
    }

    private void AplicarVolumenes()
    {
        float masterMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);

        if (matchStartSource != null)
        {
            matchStartSource.volume = masterMusicVolume;
        }

        if (gameplayLoopSource != null)
        {
            gameplayLoopSource.volume = masterMusicVolume;
        }

        // Aplica el volumen regulado para la victoria y derrota
        if (victorySource != null)
        {
            victorySource.volume = masterMusicVolume * endScreensVolume;
        }

        if (defeatSource != null)
        {
            defeatSource.volume = masterMusicVolume * endScreensVolume;
        }
    }

    IEnumerator WaitForStartSoundEnd()
    {
        // Espera la duración del sonido de inicio
        yield return new WaitForSeconds(matchStartSource.clip.length);

        // Inicia el tema principal continuo
        gameplayLoopSource.loop = true;
        gameplayLoopSource.Play();
    }

    // Métodos para llamar al Ganar o Perder
    public void ReproducirVictoria()
    {
        DetenerMusicaPartida();

        if (victorySource != null)
        {
            victorySource.loop = true;
            victorySource.Play();
        }
    }

    public void ReproducirDerrota()
    {
        DetenerMusicaPartida();

        if (defeatSource != null)
        {
            defeatSource.loop = true;
            defeatSource.Play();
        }
    }

    private void DetenerMusicaPartida()
    {
        if (matchStartSource != null && matchStartSource.isPlaying) matchStartSource.Stop();
        if (gameplayLoopSource != null && gameplayLoopSource.isPlaying) gameplayLoopSource.Stop();
    }
}