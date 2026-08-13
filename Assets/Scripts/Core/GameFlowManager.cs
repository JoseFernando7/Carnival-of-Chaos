using System.Collections;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private PlayerController player1;
    [SerializeField] private PlayerController player2;

    [Header("UI")]
    [SerializeField] private RPSUI rpsUI;

    [Header("Movement")]
    [SerializeField] private PlayerMovement player1Movement;
    //[SerializeField] private PlayerMovement player2Movement;
    [SerializeField] private float movementDuration = 5f;

    private RPSManager rpsManager;
    private GamePhase currentPhase;

    private bool roundResolved;

    private void Awake()
    {
        rpsManager = new RPSManager();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        currentPhase = GamePhase.RPS;

        player1Movement.SetMovementEnabled(false);
        //player2Movement.SetMovementEnabled(false);
    }

    private void Update()
    {
        if (currentPhase != GamePhase.RPS) return;

        if (player1.HasSelected && player2.HasSelected && !roundResolved)
        {
            ResolveRPS();
        }
    }

    public void ResolveRPS()
    {
        roundResolved = true;

        RPSResult result = rpsManager.Evaluate(player1.CurrentChoice, player2.CurrentChoice);

        rpsUI.ShowResult(result);

        StartBattlePhase();
    }

    private void StartBattlePhase()
    {
        currentPhase = GamePhase.Battle;

        player1Movement.SetMovementEnabled(true);
        //player2Movement.SetMovementEnabled(true);

        StartCoroutine(BattlePhaseTimer());
    }

    private IEnumerator BattlePhaseTimer()
    {
        yield return new WaitForSeconds(movementDuration);

        EndBattlePhase();
    }

    private void EndBattlePhase()
    {
        player1Movement.SetMovementEnabled(false);
        //player2Movement.SetMovementEnabled(false);

        currentPhase = GamePhase.RPS;

        ResetRound();
    }

    public void ResetRound()
    {
        player1.ResetChoice();
        player2.ResetChoice();

        roundResolved = false;

        rpsUI.HideResult();
    }
}
