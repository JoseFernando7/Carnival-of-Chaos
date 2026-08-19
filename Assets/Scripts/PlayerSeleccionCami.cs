using UnityEngine;

public class PlayerSeleccionCami : MonoBehaviour
{
    public void ActivarCartaSeleccionada(string nombreCartaElegida)
    {
        // Recorrer todos los hijos directos (CartaZorro, CartaPez, etc.)
        foreach (Transform hijo in transform)
        {
            // Si el nombre coincide, se habilita el objeto. Las otras 3 opciones se deshabilitan.
            bool esLaElegida = hijo.name.Equals(nombreCartaElegida, System.StringComparison.OrdinalIgnoreCase);
            hijo.gameObject.SetActive(esLaElegida);
        }
    }
}
