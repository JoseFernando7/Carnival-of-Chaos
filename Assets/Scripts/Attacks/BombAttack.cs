using UnityEngine;
using UnityEngine.InputSystem;

public class BombAttack : Attack
{
    [Header("Trajectory")]
    [SerializeField] public Vector2 landingPosition = new Vector2(5.30000019f, -2.5f);
    [Tooltip("Altura, en unidades de Unity, que alcanza la bomba sobre el punto más alto entre origen y destino.")]
    [SerializeField, Min(0.01f)] private float arcHeight = 6f;
    [Tooltip("Aumenta la rapidez del vuelo manteniendo la misma altura de la parábola.")]
    [SerializeField, Min(1f)] private float flightSpeedMultiplier = 2f;

    [Header("Targeting")]
    [SerializeField] private Vector2 minimumTargetPosition = new Vector2(2.5f, -12.4f);
    [SerializeField] private Vector2 maximumTargetPosition = new Vector2(20f, 3.6f);
    [SerializeField, Min(2)] private int trajectorySegments = 32;

    [Header("Impact")]
    [SerializeField] private GameObject explosion;

    private Rigidbody2D rb;
    private Camera mainCamera;
    private LineRenderer trajectoryLine;
    private Material runtimeTrajectoryMaterial;
    private bool isAiming;
    private bool hasBeenThrown;
    private float remainingFlightTime;

    [Header("Enemy Parameters")]
    public bool isForEnemy = false;

    public override void Activate()
    {
        ActivateAiming();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        mainCamera = Camera.main;
        trajectoryLine = GetComponentInChildren<LineRenderer>(true);
        if (trajectoryLine != null)
        {
            ConfigureTrajectoryRenderer();
            trajectoryLine.enabled = false;
        }
    }

    private void OnDestroy()
    {
        if (runtimeTrajectoryMaterial != null)
        {
            Destroy(runtimeTrajectoryMaterial);
        }
    }

    private void Update()
    {
        if (hasBeenThrown || !isAiming || Mouse.current == null)
        {
            return;
        }
        if (isForEnemy == false)
        {
            landingPosition = GetClampedMousePosition();
        }
        if (landingPosition != null)
        {
            DrawTrajectory(landingPosition);
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            isAiming = false;
            trajectoryLine.enabled = false;
            ThrowBomb();
        }
    }

    private void FixedUpdate()
    {
        if (!hasBeenThrown || remainingFlightTime <= 0f)
        {
            return;
        }

        remainingFlightTime -= Time.fixedDeltaTime;
        if (remainingFlightTime > 0f)
        {
            return;
        }

        rb.position = landingPosition;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;

        if (explosion != null)
        {
            Vector3 explosionPosition = new Vector3(landingPosition.x, landingPosition.y, 0f);
            GameObject spawnedExplosion = Instantiate(explosion, explosionPosition, explosion.transform.rotation);
            spawnedExplosion.SetActive(true);
        }

        gameObject.SetActive(false);
        Destroy(gameObject, 1f);
    }

    public void ThrowBomb()
    {
        hasBeenThrown = true;
        rb.gravityScale = flightSpeedMultiplier;

        WeaponSFX weaponAudio = GetComponent<WeaponSFX>();
        if (weaponAudio == null)
        {
            Debug.LogError(" [Audio] No se encontró el script WeaponSFX en el GameObject de la Bomba.");
        }
        else
        {
            weaponAudio.ReproducirSonidoArma();
        }

        // Rigidbody2D simula únicamente X e Y; mantenemos el plano visual pedido.
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

        if (!CalculateLaunch(rb.position, landingPosition, out Vector2 initialVelocity, out float flightTime))
        {
            return;
        }

        remainingFlightTime = flightTime;
        rb.linearVelocity = initialVelocity;
        NotifyAttackUsed();
    }

    public void ActivateAiming()
    {
        if (hasBeenThrown)
        {
            return;
        }

        if (mainCamera == null || trajectoryLine == null)
        {
            Debug.LogError("BombAttack necesita una cámara principal y un LineRenderer hijo para apuntar.", this);
            return;
        }

        isAiming = true;
        trajectoryLine.enabled = true;
    }

    private Vector2 GetClampedMousePosition()
    {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        float distanceToPlane = -mainCamera.transform.position.z;
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(
            new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, distanceToPlane));

        return new Vector2(
            Mathf.Clamp(mouseWorldPosition.x, minimumTargetPosition.x, maximumTargetPosition.x),
            Mathf.Clamp(mouseWorldPosition.y, minimumTargetPosition.y, maximumTargetPosition.y));
    }

    private void DrawTrajectory(Vector2 target)
    {
        if (!CalculateLaunch(rb.position, target, out Vector2 initialVelocity, out float flightTime))
        {
            trajectoryLine.enabled = false;
            return;
        }

        trajectoryLine.positionCount = trajectorySegments + 1;
        Vector2 gravity = Physics2D.gravity * flightSpeedMultiplier;
        float lineZ = trajectoryLine.transform.position.z;

        for (int i = 0; i <= trajectorySegments; i++)
        {
            float time = flightTime * i / trajectorySegments;
            Vector2 point = rb.position + initialVelocity * time + gravity * (0.5f * time * time);
            trajectoryLine.SetPosition(i, new Vector3(point.x, point.y, lineZ));
        }
    }

    private void ConfigureTrajectoryRenderer()
    {
        trajectoryLine.loop = false;
        trajectoryLine.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        trajectoryLine.receiveShadows = false;

        Shader unlitShader = Shader.Find("Universal Render Pipeline/Unlit");
        if (unlitShader == null)
        {
            Debug.LogWarning("No se encontró el shader URP/Unlit para la trayectoria.", this);
            return;
        }

        runtimeTrajectoryMaterial = new Material(unlitShader)
        {
            renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent
        };
        runtimeTrajectoryMaterial.SetFloat("_Surface", 1f);
        runtimeTrajectoryMaterial.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        runtimeTrajectoryMaterial.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
        runtimeTrajectoryMaterial.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        runtimeTrajectoryMaterial.SetFloat("_ZWrite", 0f);
        trajectoryLine.material = runtimeTrajectoryMaterial;
    }

    private bool CalculateLaunch(Vector2 origin, Vector2 target, out Vector2 initialVelocity, out float flightTime)
    {
        float gravity = Mathf.Abs(Physics2D.gravity.y) * flightSpeedMultiplier;
        if (gravity <= Mathf.Epsilon)
        {
            Debug.LogError("BombAttack necesita una gravedad vertical distinta de cero.", this);
            initialVelocity = Vector2.zero;
            flightTime = 0f;
            return false;
        }

        // Se calcula primero la subida hasta el vértice y luego el tiempo total de vuelo.
        // Así la parábola tiene la altura indicada y la velocidad X sigue llegando al destino.
        float peakY = Mathf.Max(origin.y, target.y) + arcHeight;
        float initialVerticalVelocity = Mathf.Sqrt(2f * gravity * (peakY - origin.y));
        float timeToPeak = initialVerticalVelocity / gravity;
        float fallTime = Mathf.Sqrt(2f * (peakY - target.y) / gravity);
        flightTime = timeToPeak + fallTime;
        float horizontalVelocity = (target.x - origin.x) / flightTime;

        initialVelocity = new Vector2(horizontalVelocity, initialVerticalVelocity);
        return true;
    }
}
