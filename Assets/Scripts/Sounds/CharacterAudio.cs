using System.Collections;
using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    [Header("Sonido Característico del Animal")]
    public AudioClip clipAnimal;
    [Range(0f, 1f)] [SerializeField] private float volumenAnimal = 0.5f;
    [SerializeField] private float minTiempoGrumete = 5f;
    [SerializeField] private float maxTiempoGrumete = 10f;

    [Header("Sonidos Comunes")]
    public AudioClip clipImpacto;
    [Range(0f, 1f)] [SerializeField] private float volumenImpacto = 0.6f;

    public AudioClip clipPasos;
    [Range(0f, 1f)] [SerializeField] private float volumenPasos = 0.3f;

    private AudioSource audioVozSource;
    private AudioSource audioPasosSource;
    private Coroutine rutinasAnimal;

    private void Awake()
    {
        // Source para ruiditos e impactos
        audioVozSource = gameObject.AddComponent<AudioSource>();
        audioVozSource.playOnAwake = false;

        // Source para pasos (looping)
        audioPasosSource = gameObject.AddComponent<AudioSource>();
        audioPasosSource.playOnAwake = false;
        audioPasosSource.loop = true;
    }

    private void OnEnable()
    {
        IniciarRuiditosAnimal();
    }

    private void OnDisable()
    {
        DetenerRuiditosAnimal();
    }

    // --- 1. RUIDITO DEL ANIMAL CADA 3 - 5 SEGUNDOS ---
    public void IniciarRuiditosAnimal()
    {
        if (rutinasAnimal == null && gameObject.activeInHierarchy)
        {
            rutinasAnimal = StartCoroutine(RutinaRuiditosAnimal());
        }
    }

    public void DetenerRuiditosAnimal()
    {
        if (rutinasAnimal != null)
        {
            StopCoroutine(rutinasAnimal);
            rutinasAnimal = null;
        }
    }

    private IEnumerator RutinaRuiditosAnimal()
    {
        while (true)
        {
            float tiempoEspera = Random.Range(minTiempoGrumete, maxTiempoGrumete);
            yield return new WaitForSeconds(tiempoEspera);

            if (clipAnimal != null && audioVozSource != null)
            {
                audioVozSource.PlayOneShot(clipAnimal, volumenAnimal);
            }
        }
    }

    // --- 2. IMPACTO AL RECIBIR DAÑO ---
    public void ReproducirImpacto()
    {
        if (clipImpacto != null && audioVozSource != null)
        {
            audioVozSource.PlayOneShot(clipImpacto, volumenImpacto);
        }
    }

    // --- 3. PASOS AL MOVERSE ---
    public void ControlarPasos(bool estaCaminando)
    {
        if (clipPasos == null || audioPasosSource == null) return;

        audioPasosSource.volume = volumenPasos;

        if (estaCaminando && !audioPasosSource.isPlaying)
        {
            audioPasosSource.clip = clipPasos;
            audioPasosSource.Play();
        }
        else if (!estaCaminando && audioPasosSource.isPlaying)
        {
            audioPasosSource.Stop();
        }
    }
}