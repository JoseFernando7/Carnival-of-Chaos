using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Movement Limits")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;


    private Rigidbody2D rb;
    private Vector2 movementInput;
    private bool canMove = false;
    private Animator _animator;

    public InputAction MoveAction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        MoveAction.Enable();
    }

    private void OnDisable()
    {
        MoveAction.Disable();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!canMove) return;

        movementInput = MoveAction.ReadValue<Vector2>();
        movementInput = movementInput.normalized;

        _animator.SetFloat("Move", movementInput.magnitude);
        Debug.Log(movementInput.magnitude);

        FixedDirection();
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = movementInput * moveSpeed;

        Vector2 clampedPosition = rb.position;

        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);

        rb.position = clampedPosition;
    }

    public void SetMovementEnabled(bool enabled)
    {
        canMove = enabled;

        if (!canMove)
        {
            movementInput = Vector2.zero;
            rb.linearVelocity = Vector2.zero;
            _animator.SetFloat("Move", movementInput.magnitude);
        }
    }

    public void FixedDirection()
    {
        if(movementInput.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
