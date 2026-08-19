using System.Collections;
using System.Threading;
using UnityEngine;

public class EnemyIA : MonoBehaviour
{
    public enum State
    {
        Idle,
        Runaway,
        FollorPlayer,
        Attack
    }

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float radiusOfRunaway = 5;
    [SerializeField] private float magnitude = 0.25f;

    [SerializeField] private float attackCD = 2f;
    [SerializeField] private float attackTimer;
    [SerializeField] private bool canAttack = false;
    private bool isAttacking = false;

    public Vector2 center = new Vector2(10f, -3f);

    private Vector2 targerPosition;
    private Vector2 towardsTarget;



   [SerializeField] private GameObject player;
    private Rigidbody2D rb;
    private Animator _animator;

    public State state;

    private void Awake()
    {
        state = State.Runaway;
        rb = GetComponent<Rigidbody2D>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        _animator = GetComponent<Animator>();
        transform.rotation = Quaternion.Euler(0,180,0);
        attackTimer = 0;
        canAttack = false;
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
                canAttack = false;
                attackTimer = 0;

                break;

            case State.Runaway:

                towardsTarget = targerPosition - (Vector2)transform.position;
                if (towardsTarget.magnitude < magnitude)
                {
                    CalculateRandomPosition();
                }

                //rb.linearVelocity = towardsTarget * moveSpeed; Esto genera un empuje xdd
                transform.position += (Vector3)towardsTarget.normalized * moveSpeed * Time.deltaTime;
                Rotate();
                _animator.SetFloat("Move", 1);

                Debug.DrawLine(transform.position, targerPosition, Color.green);
                break;

            case State.FollorPlayer:

                attackTimer += Time.deltaTime;

                if(attackTimer >=  attackCD)
                {
                    canAttack = true;
                }

                //El enemigo se mueve en posicion aleatoria pero alineado a la posicion del jugador
                targerPosition = new Vector2(targerPosition.x, player.transform.position.y);
                towardsTarget = targerPosition - (Vector2)transform.position;

                if (towardsTarget.magnitude < magnitude)
                {
                    CalculateRandomPosition();
                }

                //rb.linearVelocity = towardsTarget * moveSpeed; Esto genera un empuje xdd
                transform.position += (Vector3)towardsTarget.normalized * moveSpeed * Time.deltaTime;
                Rotate();
                _animator.SetFloat("Move", 1);

                Debug.DrawLine(transform.position, targerPosition, Color.green);

                float distanceWithPlayer = Vector3.Distance(transform.position, new Vector3(transform.position.x, player.transform.position.y, transform.position.z));
                if(distanceWithPlayer < 0.5 && canAttack)
                {
                    attackTimer = 0;
                    canAttack = false;
                    state = State.Attack;
                }


                break;

            case State.Attack:
                rb.linearVelocity = Vector3.zero;
                _animator.SetFloat("Move", 0);

                if (!isAttacking)
                {
                    StartCoroutine(EnemyAttack());
                }
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

        //Debug.Log(targerPosition);
    }

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

    private IEnumerator EnemyAttack()
    {
        isAttacking = true;
        Debug.Log("Enemy atacando");
        yield return new WaitForSeconds(0.2f);
        state = State.FollorPlayer;
        isAttacking = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position , new Vector3(transform.position.x, player.transform.position.y, transform.position.z));
    }

}
