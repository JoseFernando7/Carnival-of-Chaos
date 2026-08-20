using UnityEngine;

using System;
using System.Collections;


public class CartaSeleccion : MonoBehaviour
{
    [Header("Identificación")]
    public string nombrePersonaje; // "CartaZorro", "CartaPez", etc.
    public bool fueSeleccionada = false;

    [Header("Posiciones / Coordenadas")]
    public Vector2 posicionInicial;
    public Vector2 puntoMezcla;
    public Vector2 puntoFueraDePantalla;

    [Header("Configuración de Animación")]
    public float velocidadRotacion = 360f;

    // Evento estático que envía el objeto de esta carta y su nombre
    public static event Action<CartaSeleccion, string> OnCartaClick;

    private Quaternion rotacionOriginal;
    private Quaternion rotacionRevelada;
    private bool estaGirando = false;

    private void Start()
    {
        // Si no se asignó en el inspector, toma el nombre del GameObject por defecto
        if (string.IsNullOrEmpty(nombrePersonaje))
        {
            nombrePersonaje = gameObject.name;
        }

        // Guardar la ubicación inicial en X y Y al Start()
        posicionInicial = transform.position;

        // Guardar rotación base (reversa) y rotación cara visible (180° en eje Y)
        rotacionOriginal = transform.rotation;
        rotacionRevelada = rotacionOriginal * Quaternion.Euler(0, 180, 0);
    }

    private void OnMouseEnter()
    {
        if (!fueSeleccionada && !estaGirando)
        {
            StartCoroutine(GirarA(rotacionRevelada));
        }
    }

    private void OnMouseLeave()
    {
        if (!fueSeleccionada && !estaGirando)
        {
            StartCoroutine(GirarA(rotacionOriginal));
        }
    }

    private void OnMouseDown()
    {
        if (fueSeleccionada) return;

        fueSeleccionada = true;

        // --- REPRODUCIR SONIDO DE LA CARTA SELECCIONADA ---
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.ReproducirSonidoCarta(nombrePersonaje);
        }

        // Notifica el clic enviando el objeto carta y el nombre
        OnCartaClick?.Invoke(this, nombrePersonaje);

    }

    private IEnumerator GirarA(Quaternion objetivo)
    {
        estaGirando = true;
        while (Quaternion.Angle(transform.rotation, objetivo) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, objetivo, velocidadRotacion * Time.deltaTime);
            yield return null;
        }
        transform.rotation = objetivo;
        estaGirando = false;
    }

    public IEnumerator MoverA(Vector2 destino, float duracion = 0.5f)
    {
        Vector3 inicio = transform.position;
        Vector3 fin = new Vector3(destino.x, destino.y, inicio.z);
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            transform.position = Vector3.Lerp(inicio, fin, tiempo / duracion);
            tiempo += Time.deltaTime;
            yield return null;
        }
        transform.position = fin;
    }

    public IEnumerator SalirGirando(Vector2 destino, float duracion = 0.8f)
    {
        Vector3 inicio = transform.position;
        Vector3 fin = new Vector3(destino.x, destino.y, inicio.z);
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            transform.position = Vector3.Lerp(inicio, fin, tiempo / duracion);
            transform.Rotate(Vector3.forward, 720f * Time.deltaTime); // Giro cosmético adicional al salir
            tiempo += Time.deltaTime;
            yield return null;
        }
        transform.position = fin;
    }
}