using UnityEngine;

public class SpawnAttack : MonoBehaviour
{
    [Header("Attack Prefabs")]
    [SerializeField] private Attack[] attackPrefabs;

    public void SpawnRandomAttack()
    {
      if (attackPrefabs.Length == 0) return;

      int randomIndex = Random.Range(0, attackPrefabs.Length);

      Attack attack = Instantiate(attackPrefabs[randomIndex]);
      attack.Activate();
    }
}
