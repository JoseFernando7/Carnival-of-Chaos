using UnityEngine;

public class EnemyShoe : MonoBehaviour
{
    private bool hasDamagedPlayer;

    private void OnEnable()
    {
        hasDamagedPlayer = false;

        // REPRODUCCIÓN DE AUDIO DE ZAPATO (ENEMIGO)
        WeaponSFX weaponAudio = GetComponentInChildren<WeaponSFX>();
        if (weaponAudio != null)
        {
            weaponAudio.ReproducirSonidoArma();
        }
        
        Destroy(gameObject, 1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryDamagePlayer(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryDamagePlayer(other);
    }

    private void TryDamagePlayer(Collider2D other)
    {
        if (hasDamagedPlayer || !IsPlayer(other))
        {
            return;
        }

        PlayerController player = other.GetComponentInParent<PlayerController>();
        if (player == null)
        {
            return;
        }

        hasDamagedPlayer = true;
        player.ReceiveDamage();
    }

    private bool IsPlayer(Collider2D other)
    {
        Transform current = other.transform;
        while (current != null)
        {
            if (current.CompareTag("Player"))
            {
                return true;
            }

            current = current.parent;
        }

        return false;
    }
}
