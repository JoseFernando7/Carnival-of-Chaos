using System.Collections.Generic;
using UnityEngine;

public class SpawnAttack : MonoBehaviour
{
    [Header("Attack Prefabs")]
    [SerializeField] private Attack[] attackPrefabs;

    [SerializeField] public GameObject uiWeapons;

    private readonly List<Attack> activeAttacks = new List<Attack>();

    public void SpawnRandomAttack(Transform playerTransform)
    {
      if (attackPrefabs.Length == 0) return;

      int randomIndex = Random.Range(0, attackPrefabs.Length);

      Attack attack = Instantiate(attackPrefabs[randomIndex]);
      activeAttacks.Add(attack);

      if (attack is CannonAttack && playerTransform != null)
      {
        uiWeapons.GetComponent<UiWeapons>().ActivateCanon(0);
        attack.transform.SetParent(playerTransform, false);
        attack.transform.localPosition = new Vector3(4.5f, 0f, 0f);
      }

      if (attack is ShoeAttack shoeAttack)
      {
        uiWeapons.GetComponent<UiWeapons>().ActivateShoe(0);
        shoeAttack.SetPlayerTransform(playerTransform);
      }

      if(attack is BombAttack bombAttack)
        {
            uiWeapons.GetComponent<UiWeapons>().ActivateDog(0);
        }

      attack.Activate();
    }

    public void DestroyAllAttacks()
    {
      foreach (Attack attack in activeAttacks)
      {
        if (attack != null)
        {
          Destroy(attack.gameObject);
        }
      }

        activeAttacks.Clear();

        uiWeapons.GetComponent<UiWeapons>().Desactivate();
    }
}
