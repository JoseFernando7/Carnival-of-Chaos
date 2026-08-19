using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Configuración de AudioMixer y UI")]
    public AudioMixer mainMixer;      // Asignar el MainMixer en el Inspector
    public Slider sliderVolumen;      // Asignar el Slider de UI en el Inspector

    [Header("Fuentes de Audio (Audio Sources)")]
    public AudioSource musicaSource;  // AudioSource para música de ambiente
    public AudioSource sfxSource;     // AudioSource para efectos de sonido

    [Header("Clips de Audio - Música")]
    public AudioClip musicaSeleccionPersonaje;

    [Header("Clips de Audio - Cartas (SFX)")]
    public AudioClip sfxCartaZorro;
    public AudioClip sfxCartaPez;
    public AudioClip sfxCartaOso;
    public AudioClip sfxCartaPereza;

    [Header("Clips de Audio - Eventos")]
    public AudioClip sfxInicioCombate;

    private void Awake()
    {
        // Patron Singleton para acceder fácilmente desde cualquier script
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Reproducir música de ambiente al inicio
        if (musicaSeleccionPersonaje != null && musicaSource != null)
        {
            musicaSource.clip = musicaSeleccionPersonaje;
            musicaSource.loop = true;
            musicaSource.Play();
        }

        // Configurar el Slider de Volumen
        if (sliderVolumen != null)
        {
            sliderVolumen.minValue = 0.0001f; // Evitar el 0 matemático para el cálculo logarítmico del Mixer
            sliderVolumen.maxValue = 1f;
            sliderVolumen.value = 0.5f;       // Por defecto al medio (50%)

            // Asignar el evento para detectar cuando se arrastra la barra a la izquierda/derecha
            sliderVolumen.onValueChanged.AddListener(CambiarVolumenGeneral);

            // Establecer el volumen inicial en el medio
            CambiarVolumenGeneral(sliderVolumen.value);
        }
    }

    // Método para controlar el volumen del AudioMixer en decibelios (dB)
    public void CambiarVolumenGeneral(float valorSlider)
    {
        if (mainMixer != null)
        {
            // Convierte el valor lineal del slider (0.0001 a 1) a una escala logarítmica de decibelios (-80dB a 0dB)
            float decibelios = Mathf.Log10(valorSlider) * 20f;
            mainMixer.SetFloat("MasterVolume", decibelios);
        }
    }

    // Método para reproducir el sonido específico de cada una de las 4 cartas
    public void ReproducirSonidoCarta(string nombreCarta)
    {
        AudioClip clipAProducir = null;

        switch (nombreCarta.ToLower())
        {
            case "cartazorro":
            case "zorro":
                clipAProducir = sfxCartaZorro;
                break;
            case "cartapez":
            case "pez":
                clipAProducir = sfxCartaPez;
                break;
            case "cartaoso":
            case "oso":
                clipAProducir = sfxCartaOso;
                break;
            case "cartapereza":
            case "pereza":
                clipAProducir = sfxCartaPereza;
                break;
        }

        if (clipAProducir != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clipAProducir);
        }
    }

    // Método para reproducir el efecto de inicio de combate
    public void ReproducirSonidoInicioCombate()
    {
        if (sfxInicioCombate != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(sfxInicioCombate);
        }
    }

    // Detener la música de fondo gradualmente al iniciar combate
    public void DetenerMusicaAmbiente()
    {
        if (musicaSource != null)
        {
            StartCoroutine(FadeOutMusica(1.0f));
        }
    }

    private IEnumerator FadeOutMusica(float duracion)
    {
        float volumenInicial = musicaSource.volume;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            musicaSource.volume = Mathf.Lerp(volumenInicial, 0f, tiempo / duracion);
            tiempo += Time.deltaTime;
            yield return null;
        }

        musicaSource.Stop();
        musicaSource.volume = volumenInicial;
    }
}
