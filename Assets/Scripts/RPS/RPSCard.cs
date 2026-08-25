using UnityEngine;
using System;

public class RPSCard : MonoBehaviour
{
    [SerializeField] private RPSChoice choice;
    [SerializeField] private PlayerController player;
    [SerializeField] private PlayerController playerIA;

    public RPSChoice Choice => choice;

    public void SelectCard()
    {
        player.SelectChoice(choice);
    }

    public void RandomChoice()
    {
        RPSChoice botChoice = (RPSChoice)UnityEngine.Random.Range(0, Enum.GetValues(typeof(RPSChoice)).Length);

        playerIA.SelectChoice(botChoice);
    }
}
