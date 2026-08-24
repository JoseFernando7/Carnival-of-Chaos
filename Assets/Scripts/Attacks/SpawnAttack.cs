using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAttack : MonoBehaviour
{
    [Header("Attack Prefabs")]
    [SerializeField] private Attack[] attackPrefabs;

    [SerializeField] public GameObject uiWeapons;

    private readonly List<Attack> activeAttacks = new List<Attack>();
    private Attack attackPrefabForSequence;
    private Transform playerTransform;
    private int remainingAttackUses;
    private bool attackSequenceActive;
    private Coroutine nextAttackRoutine;

    public void SpawnRandomAttack(Transform playerTransform)
    {
        if (attackPrefabs.Length == 0)
        {
            return;
        }

        int randomIndex = Random.Range(0, attackPrefabs.Length);
        this.playerTransform = playerTransform;
        attackPrefabForSequence = attackPrefabs[randomIndex];
        remainingAttackUses = IsMultiUseAttack(attackPrefabForSequence) ? 3 : 1;
        attackSequenceActive = true;

        SpawnNextAttack();
    }

    private void SpawnNextAttack()
    {
        if (!attackSequenceActive || remainingAttackUses <= 0)
        {
            return;
        }

        Attack attack = Instantiate(attackPrefabForSequence);
        remainingAttackUses--;
        activeAttacks.Add(attack);
        attack.AttackUsed += HandleAttackUsed;

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

        if (attack is BombAttack bombAttack)
        {
            uiWeapons.GetComponent<UiWeapons>().ActivateDog(0);
        }

        attack.Activate();
    }

    private void HandleAttackUsed()
    {
        if (!attackSequenceActive || remainingAttackUses <= 0 || nextAttackRoutine != null)
        {
            return;
        }

        nextAttackRoutine = StartCoroutine(SpawnNextAttackAfterDelay());
    }

    private IEnumerator SpawnNextAttackAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);

        nextAttackRoutine = null;
        SpawnNextAttack();
    }

    private bool IsMultiUseAttack(Attack attackPrefab)
    {
        return attackPrefab is BombAttack || attackPrefab is CannonAttack;
    }

    public void DestroyAllAttacks()
    {
        attackSequenceActive = false;
        if (nextAttackRoutine != null)
        {
            StopCoroutine(nextAttackRoutine);
            nextAttackRoutine = null;
        }

        attackPrefabForSequence = null;
        playerTransform = null;
        remainingAttackUses = 0;

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
