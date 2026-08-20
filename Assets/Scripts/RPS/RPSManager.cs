public class RPSManager
{
    public RPSResult Evaluate(RPSChoice player1choice, RPSChoice player2choice)
    {
        if (player1choice == player2choice) return RPSResult.Draw;

        if (
            (player1choice == RPSChoice.Rock &&
             player2choice == RPSChoice.Scissors) ||

            (player1choice == RPSChoice.Paper &&
             player2choice == RPSChoice.Rock) ||

            (player1choice == RPSChoice.Scissors &&
             player2choice == RPSChoice.Paper)
            )
        {
            return RPSResult.Player1Wins;
        }

        return RPSResult.Player2Wins;
    }
}
