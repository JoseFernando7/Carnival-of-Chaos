using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public RPSChoice CurrentChoice { get; private set; }
    public bool HasSelected { get; private set; }

    public void SelectChoice(RPSChoice choice)
    {
        CurrentChoice = choice;
        HasSelected = true;
    }

    public void ResetChoice()
    {
        HasSelected = false;
    }
}
