using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Explosion : MonoBehaviour
{
    [SerializeField, Min(0.01f)] private float duration = 0.5f;
    private readonly HashSet<PlayerController> damagedTargets = new HashSet<PlayerController>();

    private void OnEnable()
    {
        StartCoroutine(DisableAfterDuration());
    }

    private IEnumerator DisableAfterDuration()
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
        Destroy(gameObject);
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
        string targetTag = GetTargetTag(other);
        PlayerController target = other.GetComponentInParent<PlayerController>();
        if (!string.IsNullOrEmpty(targetTag) && target != null && damagedTargets.Add(target))
        {
            target.ReduceLife(targetTag);
        }
    }

    private string GetTargetTag(Collider2D other)
    {
        Transform current = other.transform;
        while (current != null)
        {
            if (current.CompareTag("Player") || current.CompareTag("Enemy"))
            {
                return current.tag;
            }
            current = current.parent;
        }
        return string.Empty;
    }
}
