using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int lifes = 3;
    [Tooltip("Asigna aquí los tres iconos de vida en orden: Vida 1, Vida 2 y Vida 3.")]
    [SerializeField] private GameObject[] lifeIcons = new GameObject[3];
    [SerializeField] private GameFlowManager gameFlowManager;

    private Collider2D[] colliders;

    public RPSChoice CurrentChoice { get; private set; }
    public bool HasSelected { get; private set; }

    private void Awake()
    {
        colliders = GetComponentsInChildren<Collider2D>(true);

        if (gameFlowManager == null)
        {
            gameFlowManager = FindFirstObjectByType<GameFlowManager>();
        }
    }

    public void SetCombatCollidersEnabled(bool enabled)
    {
        if (colliders == null)
        {
            colliders = GetComponentsInChildren<Collider2D>(true);
        }

        foreach (Collider2D collider in colliders)
        {
            if (collider != null)
            {
                collider.enabled = enabled;
            }
        }
    }

    public void SelectChoice(RPSChoice choice)
    {
        CurrentChoice = choice;
        HasSelected = true;
    }

    public void ResetChoice()
    {
        HasSelected = false;
    }

    public void ReduceLife(string targetTag)
    {
        if (!CompareTag(targetTag) || lifes <= 0)
        {
            return;
        }

        lifes--;
        RemoveLifeIcon();
        Debug.Log($"{targetTag} ahora tiene {lifes} vidas");

        if (lifes == 0 && gameFlowManager != null)
        {
            if (targetTag == "Player")
            {
                gameFlowManager.GameOver();
            }
            else if (targetTag == "Enemy")
            {
                gameFlowManager.Victory();
            }
        }
    }

    public void ReceiveDamage()
    {
        ReduceLife("Player");
    }

    private void RemoveLifeIcon()
    {
        if (lifeIcons == null || lifeIcons.Length == 0)
        {
            return;
        }

        // Con 3 vidas iniciales, al quedar 2 se elimina el icono de índice 2;
        // después se eliminan los índices 1 y 0.
        int iconIndex = Mathf.Clamp(lifes, 0, lifeIcons.Length - 1);
        if (lifeIcons[iconIndex] != null)
        {
            Destroy(lifeIcons[iconIndex]);
            lifeIcons[iconIndex] = null;
            return;
        }

        // Respaldo por si los iconos no fueron asignados exactamente en orden.
        for (int i = lifeIcons.Length - 1; i >= 0; i--)
        {
            if (lifeIcons[i] != null)
            {
                Destroy(lifeIcons[i]);
                lifeIcons[i] = null;
                return;
            }
        }
    }
}
