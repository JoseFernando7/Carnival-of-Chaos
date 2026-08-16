using System.Collections;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private PlayerController player1;
    [SerializeField] private PlayerController player2;

    [Header("UI")]
    [SerializeField] private RPSUI rpsUI;
    [SerializeField] private UIAnimationsManager cards;

    [Header("Movement")]
    [SerializeField] private PlayerMovement player1Movement;
    [SerializeField] private EnemyIA player2Movement;
    //[SerializeField] private PlayerMovement player2Movement;
    [SerializeField] private float movementDuration = 5f;

    private RPSManager rpsManager;
    private GamePhase currentPhase;

    private bool roundResolved;
    private bool player1Win = false;
    private bool draw = false;

    private void Awake()
    {
        rpsManager = new RPSManager();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        currentPhase = GamePhase.RPS;

        player1Movement.SetMovementEnabled(false);
        player2Movement.state = EnemyIA.State.Idle;
        cards.CardStartGame();
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

        if(result == RPSResult.Player1Wins)
        {
            player1Win = true;
        }

        rpsUI.ShowResult(result);

        StartBattlePhase();
    }

    private void StartBattlePhase()
    {
        currentPhase = GamePhase.Battle;

        player1Movement.SetMovementEnabled(true);

        if(player1Win == true)
        {
            player2Movement.state = EnemyIA.State.Runaway;
        }

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
        player2Movement.state = EnemyIA.State.Idle;

        currentPhase = GamePhase.RPS;

        ResetRound();
    }

    public void ResetRound()
    {
        player1.ResetChoice();
        player2.ResetChoice();

        player1Win = false;
        draw = false;
        roundResolved = false;

        cards.CardRestart();

        rpsUI.HideResult();
    }
}
