
interface IInvokable {


    void Authenticated(string guid, int id);

    void PlayerConnected(int id, string name);
    void SetPlayers(string[] names, int[] ids);

    void StartGame();

}
