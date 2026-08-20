using UnityEngine;

public class UIAnimationsManager : MonoBehaviour
{
    [SerializeField] private Animator cardAnimator;

    public void CardPause()
    {

        cardAnimator.Play("Hidden");
    }

    public void CardStartGame()
    {
        cardAnimator.Play("Idle");
    }

    public void CardRestart()
    {
        cardAnimator.Play("Unhidden");
    }
}
