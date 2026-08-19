using System.Collections;
using UnityEngine;

public class ControlCortinas : MonoBehaviour
{
    [Header("Referencias Hijas")]
    public Transform cortinaIzquierda;
    public Transform cortinaDerecha;

    [Header("Estado de las Cortinas")]
    [SerializeField] private bool estaAbierta = false;

    [Header("Configuración de Velocidad")]
    public float velocidadMovimiento = 8f;

    [Header("Límites Locales Cortina Izquierda (Cerrada vs Abierta)")]
    public Vector2 posIzqCerrada = new Vector2(-3.71f, 0.19f);
    public Vector2 posIzqAbierta = new Vector2(-13f, 0.19f);

    [Header("Límites Locales Cortina Derecha (Cerrada vs Abierta)")]
    public Vector2 posDerCerrada = new Vector2(4.95f, 0.32f);
    public Vector2 posDerAbierta = new Vector2(15f, 0.32f);

    private Coroutine rutinaMovimiento;

    public bool EstaAbierta
    {
        get => estaAbierta;
        set
        {
            if (estaAbierta != value)
            {
                estaAbierta = value;
                ActualizarEstadoCortinas();
            }
        }
    }

    private void Awake()
    {
        if (cortinaIzquierda == null) cortinaIzquierda = transform.Find("izquierda");
        if (cortinaDerecha == null) cortinaDerecha = transform.Find("derecha");
    }

    private void Start()
    {
        // Posicionar en coordenadas LOCALES al iniciar
        if (cortinaIzquierda != null)
            cortinaIzquierda.localPosition = new Vector3(posIzqCerrada.x, posIzqCerrada.y, cortinaIzquierda.localPosition.z);

        if (cortinaDerecha != null)
            cortinaDerecha.localPosition = new Vector3(posDerCerrada.x, posDerCerrada.y, cortinaDerecha.localPosition.z);
    }

    private void ActualizarEstadoCortinas()
    {
        if (rutinaMovimiento != null)
        {
            StopCoroutine(rutinaMovimiento);
        }

        rutinaMovimiento = StartCoroutine(MoverCortinas());
    }

    private IEnumerator MoverCortinas()
    {
        // Objetivos definidos en espacio LOCAL
        Vector3 targetIzq = estaAbierta ?
            new Vector3(posIzqAbierta.x, posIzqAbierta.y, cortinaIzquierda.localPosition.z) :
            new Vector3(posIzqCerrada.x, posIzqCerrada.y, cortinaIzquierda.localPosition.z);

        Vector3 targetDer = estaAbierta ?
            new Vector3(posDerAbierta.x, posDerAbierta.y, cortinaDerecha.localPosition.z) :
            new Vector3(posDerCerrada.x, posDerCerrada.y, cortinaDerecha.localPosition.z);

        while (Vector3.Distance(cortinaIzquierda.localPosition, targetIzq) > 0.01f ||
               Vector3.Distance(cortinaDerecha.localPosition, targetDer) > 0.01f)
        {
            cortinaIzquierda.localPosition = Vector3.MoveTowards(cortinaIzquierda.localPosition, targetIzq, velocidadMovimiento * Time.deltaTime);
            cortinaDerecha.localPosition = Vector3.MoveTowards(cortinaDerecha.localPosition, targetDer, velocidadMovimiento * Time.deltaTime);
            yield return null;
        }

        cortinaIzquierda.localPosition = targetIzq;
        cortinaDerecha.localPosition = targetDer;
    }
}