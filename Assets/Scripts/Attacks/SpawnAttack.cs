using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnAttack : MonoBehaviour
{
    [Header("Attack Prefabs")]
    [SerializeField] private BombAttack bombPrefab;
    [SerializeField] private CannonAttack cannonPrefab;
    [SerializeField] private ShoeAttack shoePrefab;

    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && bombPrefab != null)
        {
            BombAttack bomb = Instantiate(bombPrefab);
            bomb.ActivateAiming();
        }

        if (Keyboard.current.cKey.wasPressedThisFrame && cannonPrefab != null)
        {
            CannonAttack cannon = Instantiate(cannonPrefab);
            cannon.ActivateCannonMode();
        }

        if (Keyboard.current.zKey.wasPressedThisFrame && shoePrefab != null)
        {
            ShoeAttack shoe = Instantiate(shoePrefab);
            shoe.ActivateShoeMode();
        }
    }
}
