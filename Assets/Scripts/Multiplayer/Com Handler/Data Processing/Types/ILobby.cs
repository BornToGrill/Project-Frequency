interface ILobby {

    void PlayerJoined(int id, string name);
    void PlayerLeft(int id, string name);
    void GameStart();
}
