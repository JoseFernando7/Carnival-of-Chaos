using UnityEngine;
using UnityEngine.InputSystem;

public class CannonAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CannonBall cannonBallPrefab;
    [SerializeField] private Transform muzzlePoint;

    [Header("Movement")]
    [SerializeField] private float minimumY = -12.6f;
    [SerializeField] private float maximumY = 3.4f;

    private Camera mainCamera;
    private float fixedX;
    private bool isCannonModeActive;
    private CannonBall activeCannonBall;

    private void Awake()
    {
        mainCamera = Camera.main;
        fixedX = transform.position.x;

        if (muzzlePoint == null)
        {
            muzzlePoint = transform.Find("MuzzlePoint");
        }

        // Si se usa el objeto Bala de la escena como plantilla, no debe verse ni simularse.
        if (cannonBallPrefab != null && cannonBallPrefab.gameObject.scene.IsValid())
        {
            cannonBallPrefab.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!isCannonModeActive || Mouse.current == null || mainCamera == null)
        {
            return;
        }

        MoveCannonToMouseY();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            isCannonModeActive = false;
            Fire();
        }
    }

    private void MoveCannonToMouseY()
    {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        float distanceToPlane = -mainCamera.transform.position.z;
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(
            new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, distanceToPlane));

        float clampedY = Mathf.Clamp(mouseWorldPosition.y, minimumY, maximumY);
        transform.position = new Vector3(fixedX, clampedY, transform.position.z);
    }

    public void ActivateCannonMode()
    {
        if (activeCannonBall == null)
        {
            isCannonModeActive = true;
        }
    }

    private void Fire()
    {
        if (cannonBallPrefab == null || muzzlePoint == null || activeCannonBall != null)
        {
            return;
        }

        activeCannonBall = Instantiate(cannonBallPrefab, muzzlePoint.position, Quaternion.identity);
        activeCannonBall.gameObject.SetActive(true);
        activeCannonBall.Launch();
    }
}
