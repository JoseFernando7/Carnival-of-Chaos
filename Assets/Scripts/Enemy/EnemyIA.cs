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

    [Header("Attack")]
    [SerializeField] private float attackCD = 2f;
    [SerializeField] private float attackTimer;
    [SerializeField] private bool canAttack = false;
    private bool isAttacking = false;

    [Header("Weapons")]
    [SerializeField] GameObject canon;
    [SerializeField] GameObject shoe;
    [SerializeField] GameObject dog;
    [SerializeField] GameObject UiWeapons;

    private int randomAttack;

    public Vector2 center = new Vector2(10f, -3f);

    private Vector2 targerPosition;
    private Vector2 towardsTarget;

    //Cree esta variable como salvavidas para que la animacion solo se active una vez
    private int countOfAttacks = 0;
    
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
        Debug.Log(countOfAttacks);
        switch (state)
        {
            case State.Idle:

                rb.linearVelocity = Vector3.zero;
                _animator.SetFloat("Move", 0);
                countOfAttacks = 0;
                UiWeapons.GetComponent<UiWeapons>().Desactivate();
                canAttack = false;
                attackTimer = 0;
                isAttacking = false;

                canon.SetActive(false);

                randomAttack = GenerateRandomNumber();

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

                //RANDOMIZADOR DE ARMA

                float distanceWithPlayer = Vector3.Distance(transform.position, new Vector3(transform.position.x, player.transform.position.y, transform.position.z));

                switch (randomAttack)
                {
                    //Canon
                    case 0:
                        UiWeapons.GetComponent<UiWeapons>().ActivateCanon(countOfAttacks);
                        countOfAttacks++;
                        canon.SetActive(true);
                        if (distanceWithPlayer < 0.5 && canAttack)
                        {
                            //Enemigo siempre gira para mirar al jugador y disparar correctamente
                            transform.rotation = Quaternion.Euler(0, 180, 0);
                            attackTimer = 0;
                            canAttack = false;
                            canon.GetComponent<EnemyCanon>().Shot();
                        }
                    break;

                        //Shoe
                    case 1:
                        UiWeapons.GetComponent<UiWeapons>().ActivateShoe(countOfAttacks);
                        countOfAttacks++;
                        if (distanceWithPlayer < 0.5 && canAttack)
                        {
                            //Enemigo siempre gira para mirar al jugador y disparar correctamente
                            transform.rotation = Quaternion.Euler(0, 180, 0);
                            attackTimer = 0;
                            canAttack = false;

                            Vector3 setShoePosition = new Vector3(-1, transform.position.y, 0);

                            Instantiate(shoe, setShoePosition, transform.rotation);
                        }
                    break;

                    case 2:
                        UiWeapons.GetComponent<UiWeapons>().ActivateDog(countOfAttacks);
                        countOfAttacks++;
                        //Enemigo siempre gira para mirar al jugador y disparar correctamente
                        if (canAttack)
                        {
                            transform.rotation = Quaternion.Euler(0, 180, 0);
                            attackTimer = 0;
                            canAttack = false;

                            GameObject dogBomb = Instantiate(dog, transform.position, transform.rotation);
                            dogBomb.GetComponent<BombAttack>().isForEnemy = true;
                            dogBomb.GetComponent<BombAttack>().landingPosition = player.transform.position;
                            dogBomb.GetComponent<BombAttack>().ThrowBomb();
                        }
                    break;
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

    //Esta funcion permie obtener una posicion random dentro de un area circular limitada
    private void CalculateRandomPosition()
    {
        targerPosition = center + (Random.insideUnitCircle * radiusOfRunaway);
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

    private int GenerateRandomNumber()
    {
        int random = Random.Range(0, 3);

        return random;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position , new Vector3(transform.position.x, player.transform.position.y, transform.position.z));
    }

}
