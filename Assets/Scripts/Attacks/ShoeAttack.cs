using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ShoeAttack : Attack
{
    [Header("Spawn")]
    [SerializeField] private Vector3 spawnPosition = new Vector3(1.28f, -3.95f, 0f);

    [Header("Strike")]
    [SerializeField] private float strikeHeight = 10f;
    [SerializeField, Min(0.01f)] private float returnDuration = 0.3f;

    private Transform playerTransform;
    private Collider2D shoeCollider;
    private bool isShoeModeActive;
    private Coroutine returnRoutine;
    private bool collisionWasLogged;

    public override void Activate()
    {
      ActivateShoeMode();
    }

    private void Awake()
    {
        shoeCollider = GetComponent<Collider2D>();
        SetColliderEnabled(false);
        transform.position = spawnPosition;
    }

    private void Update()
    {
        if (!isShoeModeActive || Mouse.current == null || playerTransform == null)
        {
            return;
        }

        transform.position = new Vector3(spawnPosition.x, playerTransform.position.y, spawnPosition.z);

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            isShoeModeActive = false;
            returnRoutine = StartCoroutine(ReturnAfterStrike());
        }
    }

    private IEnumerator ReturnAfterStrike()
    {
        Vector3 originalPosition = transform.position;
        Vector3 raisedPosition = originalPosition + Vector3.up * strikeHeight;

        // Keyframe 0.00: el zapato aparece 10 unidades por encima del punto clicado.
        transform.position = raisedPosition;

        float elapsedTime = 0f;
        while (elapsedTime < returnDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / returnDuration);
            transform.position = Vector3.Lerp(raisedPosition, originalPosition, progress);
            yield return null;
        }

        // Keyframe 0.30: vuelve exactamente al punto que tenía al hacer clic.
        transform.position = originalPosition;
        SetColliderEnabled(true);
        Physics2D.SyncTransforms();
        LogOverlappingTarget();
        returnRoutine = null;

        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }

    public void ActivateShoeMode()
    {
        if (returnRoutine != null)
        {
            StopCoroutine(returnRoutine);
            returnRoutine = null;
        }

        isShoeModeActive = true;
        collisionWasLogged = false;
        SetColliderEnabled(false);
        transform.position = spawnPosition;
    }

    public void SetPlayerTransform(Transform playerTransform)
    {
      this.playerTransform = playerTransform;
    }

    private void SetColliderEnabled(bool enabled)
    {
        if (shoeCollider != null)
        {
            shoeCollider.enabled = enabled;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        LogCollisionWithValidTag(collision.collider);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        LogCollisionWithValidTag(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        LogCollisionWithValidTag(other);
    }

    private void LogCollisionWithValidTag(Collider2D other)
    {
        string targetTag = GetTargetTag(other);
        if (!string.IsNullOrEmpty(targetTag) && !collisionWasLogged)
        {
            PlayerController target = other.GetComponentInParent<PlayerController>();
            if (target != null)
            {
                collisionWasLogged = true;
                target.ReduceLife(targetTag);
            }
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

    private void LogOverlappingTarget()
    {
        if (shoeCollider == null)
        {
            return;
        }

        ContactFilter2D filter = new ContactFilter2D { useTriggers = true, useLayerMask = false };
        Collider2D[] overlaps = new Collider2D[16];
        int count = shoeCollider.Overlap(filter, overlaps);
        for (int i = 0; i < count; i++)
        {
            LogCollisionWithValidTag(overlaps[i]);
        }
    }
}
