using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
    [SerializeField, Min(0.01f)] private float duration = 0.5f;

    private void OnEnable()
    {
        StartCoroutine(DisableAfterDuration());
    }

    private IEnumerator DisableAfterDuration()
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        LogCollisionWithValidTag(collision.collider);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        LogCollisionWithValidTag(other);
    }

    private void LogCollisionWithValidTag(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            Debug.Log($"Colisión con {other.tag}");
        }
    }
}
