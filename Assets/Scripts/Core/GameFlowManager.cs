using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private PlayerController player1;
    [SerializeField] private PlayerController player2;

    [Header("UI")]
    [SerializeField] private RPSUI rpsUI;
    [SerializeField] private UIAnimationsManager cards;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject losePanel;

    [Header("Movement")]
    [SerializeField] private PlayerMovement player1Movement;
    [SerializeField] private EnemyIA player2Movement;
    //[SerializeField] private PlayerMovement player2Movement;
    [SerializeField] private float movementDuration = 5f;

    [Header("Attacks")]
    [SerializeField] private SpawnAttack spawnAttack;

    private RPSManager rpsManager;
    private GamePhase currentPhase;

    private bool roundResolved;
    private bool player1Win = false;
    private bool draw = false;
    private bool gameEnded;

    private void Awake()
    {
        rpsManager = new RPSManager();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        currentPhase = GamePhase.RPS;

        player1Movement.SetMovementEnabled(false);
        player2Movement.state = EnemyIA.State.Idle;
        cards.CardStartGame();
        //player2Movement.SetMovementEnabled(false);
    }

    private void Update()
    {
        if (gameEnded || currentPhase != GamePhase.RPS) return;

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

        if (result == RPSResult.Draw)
        {
            StartCoroutine(DrawPhase());
            return;
        }

        StartBattlePhase();
    }

    private IEnumerator DrawPhase()
    {
        currentPhase = GamePhase.RPS;

        player1Movement.SetMovementEnabled(false);
        player2Movement.state = EnemyIA.State.Idle;

        yield return new WaitForSeconds(1.5f);

        ResetRound();
    }

    private void StartBattlePhase()
    {
        currentPhase = GamePhase.Battle;

        if (player1Win)
        {
            spawnAttack.SpawnRandomAttack(player1.transform);
        }

        player1Movement.SetMovementEnabled(true);

        if (player1Win)
        {
            player2Movement.state = EnemyIA.State.Runaway;
        }
        else
        {
            player2Movement.state = EnemyIA.State.FollorPlayer;
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
        spawnAttack.DestroyAllAttacks();

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

    public void GameOver()
    {
        EndGame(losePanel);
    }

    public void Victory()
    {
        EndGame(victoryPanel);
    }

    private void EndGame(GameObject panelToShow)
    {
        if (gameEnded)
        {
            return;
        }

        gameEnded = true;
        player1Movement.SetMovementEnabled(false);
        player2Movement.state = EnemyIA.State.Idle;
        spawnAttack.DestroyAllAttacks();

        if (panelToShow != null)
        {
            panelToShow.SetActive(true);
        }
    }

    public void ResetScene()
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowMenuScene()
    {
      SceneManager.LoadScene("Menu");
    }
}
