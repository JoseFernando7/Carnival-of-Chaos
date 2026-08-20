using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class PlayerControllerCami : MonoBehaviour
{
    [Header("Configuración de Rotación")]
    public float velocidadGiro = 360f; // Velocidad de animación de revelado

    [Header("Configuración de Jugador")]
    public bool es_player_1 = true;
    public float velocidadMovimiento = 5f;

    [Header("Estado")]
    public int vida = 3;
    public bool estaMuerto = false;

    [Header("Efectos de Sonido")]
    public AudioClip sonidoHurt;
    public AudioClip sonidoMuerte;
    public AudioClip sonidoBloqueo;
    public AudioClip sonidoAtaque;

    // Componentes e internos
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private Vector2 entradaMovimiento;
    private bool estaAtacando = false;
    private bool estaDefendiendo = false;

    // Tags originales
    private string tagOriginal;

    void Awake()
    {
        // Obtener componentes
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        // Configuración automática de física 2D
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Evita que el personaje gire al colisionar
    }

    void Start()
    {
        // Asignar tag inicial según configuración
        tagOriginal = es_player_1 ? "player1" : "player2";
        gameObject.tag = tagOriginal;
    }

    void Update()
    {
        if (estaMuerto) return;

        ProcesarEntrada();
    }

    void FixedUpdate()
    {
        if (estaMuerto)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Movimiento por físicas sin gravedad
        rb.linearVelocity = entradaMovimiento * velocidadMovimiento;
    }

    private void ProcesarEntrada()
    {
        float x = 0f;
        float y = 0f;

        if (es_player_1)
        {
            // Movimiento WASD
            if (Input.GetKey(KeyCode.A)) x = -1f;
            if (Input.GetKey(KeyCode.D)) x = 1f;
            if (Input.GetKey(KeyCode.S)) y = -1f;
            if (Input.GetKey(KeyCode.W)) y = 1f;

            // Acciones Q / E
            ManejarAcciones(KeyCode.Q, KeyCode.E);
        }
        else
        {
            // Movimiento Numpad (4: Izq, 6: Der, 5: Abajo, 8: Arriba)
            if (Input.GetKey(KeyCode.Keypad4)) x = -1f;
            if (Input.GetKey(KeyCode.Keypad6)) x = 1f;
            if (Input.GetKey(KeyCode.Keypad5)) y = -1f;
            if (Input.GetKey(KeyCode.Keypad8)) y = 1f;

            // Acciones Numpad (7: Ataque, 9: Defensa - usando 9 para no solapar con 8 que es arriba)
            ManejarAcciones(KeyCode.Keypad7, KeyCode.Keypad9);
        }

        entradaMovimiento = new Vector2(x, y).normalized;
    }

    private void ManejarAcciones(KeyCode teclaAtaque, KeyCode teclaDefensa)
    {
        // Ataque
        if (Input.GetKeyDown(teclaAtaque) && !estaDefendiendo)
        {
            estaAtacando = true;
            gameObject.tag = "arma";
            ReproducirSonido(sonidoAtaque);
        }
        else if (Input.GetKeyUp(teclaAtaque))
        {
            estaAtacando = false;
            ActualizarTagBase();
        }

        // Defensa
        if (Input.GetKeyDown(teclaDefensa) && !estaAtacando)
        {
            estaDefendiendo = true;
            gameObject.tag = "escudo";
        }
        else if (Input.GetKeyUp(teclaDefensa))
        {
            estaDefendiendo = false;
            ActualizarTagBase();
        }
    }

    private void ActualizarTagBase()
    {
        if (!estaAtacando && !estaDefendiendo)
        {
            tagOriginal = es_player_1 ? "player1" : "player2";
            gameObject.tag = tagOriginal;
        }
    }

    private void OnCollisionEnter2D(Collision2D colision)
    {
        if (estaMuerto) return;

        // Si colisiona con un objeto con el tag "arma"
        if (colision.gameObject.CompareTag("arma"))
        {
            if (estaDefendiendo)
            {
                ReproducirSonido(sonidoBloqueo);
            }
            else
            {
                RecibirDano();
            }
        }
    }

    private void RecibirDano()
    {
        vida--;

        if (vida <= 0)
        {
            vida = 0;
            estaMuerto = true;
            ReproducirSonido(sonidoMuerte);
        }
        else
        {
            ReproducirSonido(sonidoHurt);
        }
    }

    private void ReproducirSonido(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void ActivarCartaSeleccionada(string nombreCartaElegida)
    {
        foreach (Transform hijo in transform)
        {
            bool esLaElegida = hijo.name.Equals(nombreCartaElegida, System.StringComparison.OrdinalIgnoreCase);

            hijo.gameObject.SetActive(esLaElegida);

            if (esLaElegida)
            {
                // Inicia la corrutina para girar la carta elegida
                StartCoroutine(GirarCartaAFrontal(hijo));
            }
        }
    }

    private IEnumerator GirarCartaAFrontal(Transform cartaTransform)
    {
        // Al estar el Player a Y = -180, la carta debe terminar en localRotation Y = -180 
        // para que la rotación global/mundo resulte en Y = 0 (cara frontal)
        Quaternion rotacionObjetivo = Quaternion.Euler(0f, -180f, 0f);

        // Inicia en localRotation Y = 0 (que con el padre rotado equivale a Y = -180 en el mundo)
        Vector3 rotActual = cartaTransform.localEulerAngles;
        cartaTransform.localRotation = Quaternion.Euler(rotActual.x, 0f, rotActual.z);

        // Girar suavemente en su eje Y local
        while (Quaternion.Angle(cartaTransform.localRotation, rotacionObjetivo) > 0.1f)
        {
            cartaTransform.localRotation = Quaternion.RotateTowards(
                cartaTransform.localRotation,
                rotacionObjetivo,
                velocidadGiro * Time.deltaTime
            );
            yield return null;
        }

        cartaTransform.localRotation = rotacionObjetivo;
    }
}
