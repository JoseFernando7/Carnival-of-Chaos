using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    [Header("Panel de Selección")]
    [SerializeField] private GameObject panelSeleccion;

    [Header("Skins de Jugador")]
    [SerializeField] private GameObject skinPerezosa;
    [SerializeField] private GameObject skinPez;
    [SerializeField] private GameObject skinOso;
    [SerializeField] private GameObject skinZorro;

    // Método para seleccionar Pérezosa
    public void SeleccionarPerezosa()
    {
        ActivarSkin(skinPerezosa);
    }

    // Método para seleccionar Pez
    public void SeleccionarPez()
    {
        ActivarSkin(skinPez);
    }

    // Método para seleccionar Oso
    public void SeleccionarOso()
    {
        ActivarSkin(skinOso);
    }

    // Método para seleccionar Zorro
    public void SeleccionarZorro()
    {
        ActivarSkin(skinZorro);
    }

    // Apaga todas las skins, activa la elegida y oculta el panel
    private void ActivarSkin(GameObject skinElegida)
    {
        // 1. Desactivar todas las skins
        skinPerezosa.SetActive(false);
        skinPez.SetActive(false);
        skinOso.SetActive(false);
        skinZorro.SetActive(false);

        // 2. Activar solo la skin seleccionada
        if (skinElegida != null)
        {
            skinElegida.SetActive(true);
        }

        // 3. Ocultar el panel de selección (esto desaparece todos los botones)
        if (panelSeleccion != null)
        {
            panelSeleccion.SetActive(false);
        }
    }
}
