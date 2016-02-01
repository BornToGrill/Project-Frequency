using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LocalPlay : MonoBehaviour {

    void Awake() {
        GameObject settings = GameObject.Find("LocalGameSettings");
        GameObject amount = GameObject.Find("PlayerAmount");
        if (settings != null) {
            amount.GetComponent<Text>().text = settings.GetComponent<GameData>().AmountOfPlayers.ToString();
        }
        else {
            settings = new GameObject("LocalGameSettings");
            settings.AddComponent<GameData>().AmountOfPlayers = 4;
            DontDestroyOnLoad(settings);
            amount.GetComponent<Text>().text = "4";
        }

    }

    public void IncrementPlayers(int incr) {
        GameObject go = GameObject.Find("PlayerAmount");
        int current = int.Parse(go.GetComponent<Text>().text);
        current += incr;
        current = current > 4 ? 4 : current;
        current = current < 2 ? 2 : current;
        go.GetComponent<Text>().text = current.ToString();

        GameObject settings = GameObject.Find("LocalGameSettings");
        if (settings == null) {
            settings = new GameObject("LocalGameSettings");
            settings.AddComponent<GameData>().AmountOfPlayers = current;
            DontDestroyOnLoad(settings);
        }
        else {
            settings.GetComponent<GameData>().AmountOfPlayers = current;
        }
    }

    public void StartLocal() {
        GameObject localSettings = GameObject.Find("LocalGameSettings");
        if (localSettings == null) {
            localSettings = new GameObject("LocalGameSettings");
            GameData data = localSettings.AddComponent<GameData>();
            data.AmountOfPlayers = int.Parse(GameObject.Find("PlayerAmount").GetComponent<Text>().text);
            DontDestroyOnLoad(localSettings);
        }
        SceneManager.LoadScene("MainGame");
    }
}


public class GameData : MonoBehaviour {
    public int AmountOfPlayers;
}