using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LocalPlay : MonoBehaviour {


    public void IncrementPlayers(int incr) {
        GameObject go = GameObject.Find("PlayerAmount");
        int current = int.Parse(go.GetComponent<Text>().text);
        current += incr;
        current = current > 4 ? 4 : current;
        current = current < 2 ? 2 : current;
        go.GetComponent<Text>().text = current.ToString();
    }

    public void StartLocal() {
        GameObject localSettings = new GameObject("LocalGameSettings");
        GameData data = localSettings.AddComponent<GameData>();
        data.AmountOfPlayers = int.Parse(GameObject.Find("PlayerAmount").GetComponent<Text>().text);
        DontDestroyOnLoad(localSettings);
        SceneManager.LoadScene("MainGame");
    }
}


public class GameData : MonoBehaviour {
    public int AmountOfPlayers;
}