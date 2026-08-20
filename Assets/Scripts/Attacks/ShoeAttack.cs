using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ShoeAttack : Attack
{
    [Header("Spawn")]
    [SerializeField] private Vector3 spawnPosition = new Vector3(1.28f, -3.95f, 0f);

    [Header("Movement")]
    [SerializeField] private float minimumY = -12.6f;
    [SerializeField] private float maximumY = 3.4f;

    [Header("Strike")]
    [SerializeField] private float strikeHeight = 10f;
    [SerializeField, Min(0.01f)] private float returnDuration = 0.3f;

    private Camera mainCamera;
    private Collider2D shoeCollider;
    private bool isShoeModeActive;
    private Coroutine returnRoutine;

    public override void Activate()
    {
      ActivateShoeMode();
    }

    private void Awake()
    {
        mainCamera = Camera.main;
        shoeCollider = GetComponent<Collider2D>();
        SetColliderEnabled(false);
        transform.position = spawnPosition;
    }

    private void Update()
    {
        if (!isShoeModeActive || Mouse.current == null || mainCamera == null)
        {
            return;
        }

        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        float distanceToPlane = -mainCamera.transform.position.z;
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(
            new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, distanceToPlane));

        float clampedY = Mathf.Clamp(mouseWorldPosition.y, minimumY, maximumY);
        transform.position = new Vector3(spawnPosition.x, clampedY, spawnPosition.z);

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
        SetColliderEnabled(false);
        transform.position = spawnPosition;
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

    private void LogCollisionWithValidTag(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            Debug.Log($"Colisión con {other.tag}");
        }
    }
}
