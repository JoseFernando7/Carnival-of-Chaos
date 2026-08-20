using UnityEngine;

public class EnemyShoe : MonoBehaviour
{
    private void OnEnable()
    {
        Destroy(gameObject, 1);
    }

}
