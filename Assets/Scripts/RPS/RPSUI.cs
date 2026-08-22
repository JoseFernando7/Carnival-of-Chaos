using UnityEngine;
using TMPro;

public class RPSUI : MonoBehaviour
{
    //[SerializeField] private GameObject resultPanel;
    //[SerializeField] TMP_Text resultText;
    [SerializeField] private GameObject resultPanel;

    public void ShowResult(RPSResult result)
    {
        //resultPanel.SetActive(true);

        switch (result)
        {
            case RPSResult.Player1Wins:

                resultPanel.GetComponent<UIResult>().ActivateAttackResult();

                //resultText.text = "PLAYER 1 WINS!!";
                break;

            case RPSResult.Player2Wins:

                resultPanel.GetComponent<UIResult>().ActivateRunResult();

                //resultText.text = "PLAYER 2 WINS!!";
                break;

            case RPSResult.Draw:

                resultPanel.GetComponent<UIResult>().ActivateDrawResult();

                //resultText.text = "DRAW!!";
                break;
        }
    }

    public void HideResult()
    {
        //resultPanel.SetActive(false);
    }
}
