using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Configuración de Pausa")]
    public GameObject[] panelesParaCerrar;
    public GameObject panelElementosPrincipales;

    // Cambiar de escena
    public void CambiarEscena(int indiceEscena)
    {
        SceneManager.LoadScene(indiceEscena);
    }

    // Activar/Abrir un panel (arrastrándolo al botón)
    public void ActivarPanel(GameObject panel)
    {
        if (panel != null)
        {
            panel.SetActive(true);
        }
    }

    // Desactivar/Cerrar un panel (arrastrándolo al botón)
    public void CerrarPanel(GameObject panel)
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    // Pausar el juego
    public void PausarJuego(GameObject panelPausa)
    {
        if (panelPausa != null) panelPausa.SetActive(true);

        foreach (GameObject panel in panelesParaCerrar)
        {
            if (panel != null) panel.SetActive(false);
        }
        
        Time.timeScale = 0f;
    }

    // Reanudar el juego
    public void ReanudarJuego(GameObject panelPausa)
    {
        if (panelPausa != null) panelPausa.SetActive(false);
        
        if (panelElementosPrincipales != null) panelElementosPrincipales.SetActive(true);
        
        Time.timeScale = 1f;
    }

    public void ReiniciarJuego()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}