interface ILobby {

    void PlayerJoined(int id, string name);
    void PlayerLeft(int id, string name);
    void Authenticated(string guid, int id, string playerName, string lobbyId);
    void SetPlayers(string[] names);
    void GameStart(TempPlayer[] players);
}
