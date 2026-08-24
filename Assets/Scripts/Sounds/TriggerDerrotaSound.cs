using UnityEngine;

public class TriggerDerrotaSound : MonoBehaviour
{
    private void OnEnable()
    {
        // Se ejecuta solo cuando el LosePanel pasa de desactivado a activado
        GameplayMusicController controller = FindFirstObjectByType<GameplayMusicController>();
        if (controller != null)
        {
            controller.ReproducirDerrota();
        }
    }
}