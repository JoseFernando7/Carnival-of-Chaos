using UnityEngine;
using TMPro;

public class RPSUI : MonoBehaviour
{
    [SerializeField] private GameObject resultPanel;
    [SerializeField] TMP_Text resultText;

    public void ShowResult(RPSResult result)
    {
        resultPanel.SetActive(true);

        switch (result)
        {
            case RPSResult.Player1Wins:
                resultText.text = "PLAYER 1 WINS!!";
                break;

            case RPSResult.Player2Wins:
                resultText.text = "PLAYER 2 WINS!!";
                break;

            case RPSResult.Draw:
                resultText.text = "DRAW!!";
                break;
        }
    }

    public void HideResult()
    {
        resultPanel.SetActive(false);
    }
}
