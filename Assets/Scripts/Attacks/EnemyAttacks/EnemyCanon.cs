using UnityEngine;

public class EnemyCanon : MonoBehaviour
{
    [SerializeField] private CannonBall bulletPrefab;
     private CannonBall bullet;

    [SerializeField] private Transform bulletPosition;
    [SerializeField] private float shotCD = 1.5f;

    private float timer = 0;

    //private void OnEnable()
    //{
    //    InvokeRepeating(nameof(Shot), shotCD, shotCD);
    //}

    //private void Update()
    //{
    //    timer += Time.deltaTime;

    //    if(timer >= shotCD)
    //    {
    //        timer = 0;
    //        Shot();
    //    }
    //}


    public void Shot()
    {
       bullet = Instantiate(bulletPrefab, bulletPosition.position, transform.rotation);
       bullet.Launch(Vector2.left);
    }
}
