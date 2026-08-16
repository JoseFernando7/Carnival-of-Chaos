using UnityEngine;

public class EnemyIA : MonoBehaviour
{
    public enum State
    {
        Idle,
        Runaway
    }

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float radiusOfRunaway = 5;
    [SerializeField] private float magnitude = 0.25f;

    public Vector2 center = new Vector2(10f, -3f);

    private Vector2 targerPosition;
    private Vector2 towardsTarget;


    private GameObject player;
    private Rigidbody2D rb;
    private Animator _animator;

    public State state;

    private void Awake()
    {
        state = State.Runaway;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        _animator = GetComponent<Animator>();
        transform.rotation = Quaternion.Euler(0,180,0);
    }

    private void Start()
    {
        CalculateRandomPosition();
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:

                rb.linearVelocity = Vector3.zero;
                _animator.SetFloat("Move", 0);

                break;

            case State.Runaway:
               
                towardsTarget = targerPosition - (Vector2)transform.position;
                if(towardsTarget.magnitude < magnitude)
                {
                    CalculateRandomPosition();
                }

                //rb.linearVelocity = towardsTarget * moveSpeed; Esto genera un empuje xdd
                transform.position += (Vector3)towardsTarget.normalized * moveSpeed * Time.deltaTime;
                Rotate();
                _animator.SetFloat("Move", 1);

                Debug.DrawLine(transform.position, targerPosition, Color.green);
               break;
        }
    }

    //Esta funcion permie obtener una posicion random dentro de un area circular limitada. 
    private void CalculateRandomPosition()
    {
        targerPosition = center + (Random.insideUnitCircle * radiusOfRunaway);

        //if((targerPosition.x > maxX || targerPosition.x < minX) || (targerPosition.y > maxY || targerPosition.y < minY))
        //{
        //    targerPosition = -targerPosition;
        //}

        Debug.Log(targerPosition);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;    
    //    Gizmos.DrawSphere(center, radiusOfRunaway);
    //}

    private void Rotate()
    {
        if(transform.position.x > targerPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

}
