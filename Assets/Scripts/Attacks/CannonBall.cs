using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField, Min(0.01f)] private float speed = 20f;
    [SerializeField, Min(0.01f)] private float lifetime = 5f;

    private Rigidbody2D rb;
    private Collider2D[] ballColliders;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ballColliders = GetComponents<Collider2D>();
        rb.gravityScale = 0f;
    }

    public void Launch()
    {
        rb.gravityScale = 0f;
        IgnorePlayerCollisions();
        rb.linearVelocity = Vector2.right * speed;
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
        if (collision.collider.CompareTag("Player"))
        {
            foreach (Collider2D ballCollider in ballColliders)
            {
                Physics2D.IgnoreCollision(ballCollider, collision.collider);
            }

            return;
        }

        if (collision.collider.CompareTag("Enemy"))
        {
          Debug.Log($"Colisión con {collision.collider.tag}");
          Destroy(gameObject);
        }
    }
}
