interface ILobby {

    void PlayerJoined(int id, string name);
    void PlayerLeft(int id, string name);
    void SetPlayers(string[] names);
    void GameStart(TempPlayer[] players);
}
