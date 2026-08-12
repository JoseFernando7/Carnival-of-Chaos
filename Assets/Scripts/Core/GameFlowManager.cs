using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    [SerializeField] private PlayerController player1;
    [SerializeField] private PlayerController player2;
    [SerializeField] private RPSUI rpsUI;

    private RPSManager rpsManager;
    private GamePhase currentPhase;

    private bool roundResolved;

    private void Awake()
    {
        rpsManager = new RPSManager();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPhase = GamePhase.RPS;
    }

    private void Update()
    {
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
    }

    public void ResetRound()
    {
        player1.ResetChoice();
        player2.ResetChoice();

        roundResolved = false;

        rpsUI.HideResult();
    }
}
