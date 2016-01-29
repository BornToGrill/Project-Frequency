
interface INotifiable {

    void EndTurn(string name, int id);
    void GameWon(int winner, int[] losers);

}
