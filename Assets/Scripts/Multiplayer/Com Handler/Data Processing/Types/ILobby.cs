interface ILobby {

    void PlayerJoined(int id, string name);
    void PlayerLeft(int id, string name);
    void Authenticated(string guid, int id);
    void GameStart(TempPlayer[] players);
}
