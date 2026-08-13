using UnityEngine;

public class RPSCard : MonoBehaviour
{
    [SerializeField] private RPSChoice choice;
    [SerializeField] private PlayerController player;

    public RPSChoice Choice => choice;

    public void SelectCard()
    {
        player.SelectChoice(choice);
    }
}
