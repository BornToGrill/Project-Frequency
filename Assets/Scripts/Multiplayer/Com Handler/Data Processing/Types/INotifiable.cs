
interface INotifiable {

    void EndTurn(string name, int id);
    void GameWon(int winner, int[] losers);
    void GameLoaded();
    void PlayerLeft(int id, string name);

}
