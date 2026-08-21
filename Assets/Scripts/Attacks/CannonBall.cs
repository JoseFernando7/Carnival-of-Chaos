using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField, Min(0.01f)] private float speed = 20f;
    [SerializeField, Min(0.01f)] private float lifetime = 5f;

    private Rigidbody2D rb;
    private Collider2D[] ballColliders;
    private SpriteRenderer spriteRenderer;
    private bool hasHitTarget;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ballColliders = GetComponents<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.gravityScale = 0f;
    }

    public void Launch()
    {
        Launch(Vector2.right);
    }

    public void Launch(Vector2 direction)
    {
        rb.gravityScale = 0f;
        float horizontalDirection = Mathf.Sign(direction.x);
        if (Mathf.Approximately(horizontalDirection, 0f))
        {
            horizontalDirection = 1f;
        }

        rb.linearVelocity = new Vector2(horizontalDirection * speed, 0f);

        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = horizontalDirection < 0f;
        }

        Destroy(gameObject, lifetime);
    }

    private void IgnorePlayerCollisions()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            foreach (Collider2D playerCollider in player.GetComponentsInChildren<Collider2D>())
            {
                foreach (Collider2D ballCollider in ballColliders)
                {
                    Physics2D.IgnoreCollision(ballCollider, playerCollider);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ProcessTargetCollision(collision.collider);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        ProcessTargetCollision(collider);
    }

    private void ProcessTargetCollision(Collider2D collider)
    {
      if (hasHitTarget)
      {
        return;
      }

      string targetTag = GetTargetTag(collider);
      PlayerController target = collider.GetComponentInParent<PlayerController>();
      if (!string.IsNullOrEmpty(targetTag) && target != null)
      {
        hasHitTarget = true;
        target.ReduceLife(targetTag);
        Destroy(gameObject);
      }
    }

    private string GetTargetTag(Collider2D collider)
    {
        Transform current = collider.transform;
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
