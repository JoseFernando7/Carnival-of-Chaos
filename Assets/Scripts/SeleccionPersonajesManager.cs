using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeleccionPersonajesManager : MonoBehaviour
{
    [Header("Referencias del Mapa de Jerarquía")]
    public PlayerControllerCami player1;
    public PlayerControllerCami player2;
    public Transform contenedorCartas; // Objeto "Cartas" en la jerarquía
    public ControlCortinas cortinas;    // Objeto "Cortinas" con su script

    [Header("Configuración de Tiempos")]
    [Tooltip("Tiempo en segundos a esperar después de la selección de Player 2 para empezar a cerrar las cortinas.")]
    public float tiempoEsperaAntesDeCerrarCortinas = 3.0f;

    [Tooltip("Tiempo en segundos a esperar para deshabilitar este GameObject una vez ordenado el cierre de cortinas.")]
    public float tiempoEsperaParaDesaparecer = 1.0f; // <--- Nueva variable pública (default: 1.0s)

    [Header("Configuración de Finalización")]
    [Tooltip("Si es true, deshabilita este GameObject (Seleccion_personajes) al terminar el tiempo de espera final.")]
    public bool deshabilitarAlFinal = true;

    private List<CartaSeleccion> listaCartas = new List<CartaSeleccion>();
    private bool seleccionIniciada = false;

    private void Awake()
    {
        if (contenedorCartas == null)
        {
            contenedorCartas = transform.Find("Cartas");
        }

        if (contenedorCartas != null)
        {
            listaCartas.AddRange(contenedorCartas.GetComponentsInChildren<CartaSeleccion>());
        }

        if (cortinas == null)
        {
            cortinas = GetComponentInChildren<ControlCortinas>();
        }
    }

    private void Start()
    {
        // Abrir cortinas suavemente al iniciar la escena
        if (cortinas != null)
        {
            cortinas.EstaAbierta = true;
        }
    }

    private void OnEnable()
    {
        CartaSeleccion.OnCartaClick += AlSeleccionarCartaPlayer1;
    }

    private void OnDisable()
    {
        CartaSeleccion.OnCartaClick -= AlSeleccionarCartaPlayer1;
    }

    private void AlSeleccionarCartaPlayer1(CartaSeleccion cartaElegida, string nombreCarta)
    {
        if (seleccionIniciada) return;
        seleccionIniciada = true;

        StartCoroutine(ProcesoSeleccion(cartaElegida, nombreCarta));
    }

    private IEnumerator ProcesoSeleccion(CartaSeleccion cartaP1, string nombreCartaP1)
    {
        // 1. Mover la carta seleccionada de la mesa a la posición de Player 1
        yield return StartCoroutine(cartaP1.MoverA(player1.transform.position, 0.6f));

        // Activar la carta oficial en Player 1
        player1.ActivarCartaSeleccionada(nombreCartaP1);
        cartaP1.gameObject.SetActive(false);

        // 2. Filtrar las 3 cartas restantes en la mesa
        List<CartaSeleccion> cartasRestantes = new List<CartaSeleccion>();
        foreach (var carta in listaCartas)
        {
            if (carta != cartaP1)
            {
                cartasRestantes.Add(carta);
            }
        }

        // 3. Las 3 cartas restantes se desplazan al mismo punto X y Y (puntoMezcla)
        List<Coroutine> movimientosMezcla = new List<Coroutine>();
        foreach (var carta in cartasRestantes)
        {
            movimientosMezcla.Add(StartCoroutine(carta.MoverA(carta.puntoMezcla, 0.5f)));
        }

        foreach (var corrutina in movimientosMezcla)
        {
            yield return corrutina;
        }

        yield return new WaitForSeconds(0.2f);

        // 4. Elegir aleatoriamente la carta para Player 2 entre las 3 restantes
        int indiceRandom = Random.Range(0, cartasRestantes.Count);
        CartaSeleccion cartaP2 = cartasRestantes[indiceRandom];
        cartaP2.fueSeleccionada = true;

        // Reproducir sonido correspondiente a la carta del Player 2
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.ReproducirSonidoCarta(cartaP2.nombrePersonaje);
        }

        // Mover la carta elegida de la mesa a la posición de Player 2
        yield return StartCoroutine(cartaP2.MoverA(player2.transform.position, 0.6f));

        // Activar la carta oficial en Player 2
        player2.ActivarCartaSeleccionada(cartaP2.nombrePersonaje);
        cartaP2.gameObject.SetActive(false);

        // 5. Las 2 cartas sobrantes salen girando de la mesa a puntoFueraDePantalla
        List<CartaSeleccion> cartasSobrantes = new List<CartaSeleccion>();
        foreach (var carta in cartasRestantes)
        {
            if (carta != cartaP2)
            {
                cartasSobrantes.Add(carta);
            }
        }

        List<Coroutine> salidas = new List<Coroutine>();
        foreach (var carta in cartasSobrantes)
        {
            salidas.Add(StartCoroutine(carta.SalirGirando(carta.puntoFueraDePantalla, 0.8f)));
        }

        foreach (var corrutina in salidas)
        {
            yield return corrutina;
        }

        // Eventos de sonido finales
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.ReproducirSonidoInicioCombate();
            SoundManager.Instance.DetenerMusicaAmbiente();
        }

        // 6. Esperar el tiempo configurado antes de cerrar cortinas
        yield return new WaitForSeconds(tiempoEsperaAntesDeCerrarCortinas);

        // Ordenar a las cortinas que se cierren
        if (cortinas != null)
        {
            cortinas.EstaAbierta = false;
        }

        // 7. Esperar el tiempo configurado para desaparecer
        yield return new WaitForSeconds(tiempoEsperaParaDesaparecer);

        // 8. Deshabilitar el GameObject si la opción está activada
        if (deshabilitarAlFinal)
        {
            gameObject.SetActive(false);
        }
    }
}