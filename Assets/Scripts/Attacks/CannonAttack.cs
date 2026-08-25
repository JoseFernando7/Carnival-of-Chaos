using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class CannonAttack : Attack
{
    [Header("References")]
    [SerializeField] private CannonBall cannonBallPrefab;
    [SerializeField] private Transform muzzlePoint;

    private float fixedX;
    private bool isCannonModeActive;
    private CannonBall activeCannonBall;

    public override void Activate()
    {
        ActivateCannonMode();
    }

    private void Awake()
    {
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
        if (!isCannonModeActive || Mouse.current == null)
        {
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            isCannonModeActive = false;
            Fire();
        }
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
        activeCannonBall.Launch(Vector2.right);

        // REPRODUCCIÓN DE AUDIO DE CAÑÓN 
        WeaponSFX weaponAudio = GetComponent<WeaponSFX>();
        if (weaponAudio != null)
        {
            weaponAudio.ReproducirSonidoArma();
        }

        NotifyAttackUsed();

        StartCoroutine(DisableCannonAfterDelay());
    }

    private IEnumerator DisableCannonAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
