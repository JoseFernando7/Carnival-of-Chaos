using UnityEngine;

public class TriggerVictoriaSound : MonoBehaviour
{
    private void OnEnable()
    {
        // Se ejecuta solo cuando el VictoryPanel pasa de desactivado a activado (.SetActive(true))
        GameplayMusicController controller = FindFirstObjectByType<GameplayMusicController>();
        if (controller != null)
        {
            controller.ReproducirVictoria();
        }
    }
}