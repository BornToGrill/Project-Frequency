using UnityEngine;
using UnityEngine.UI;
public class WinScreen : MonoBehaviour {
    
    public Text Winner;
    public Text[] Losers;

    // Use this for initialization
    void Start () {
        GameObject winObj = GameObject.Find("EndGameData");
        WinCondition cond = winObj.GetComponent<WinCondition>();
        Winner.text = cond.Winner.Name;
        for (int i = 0; i < cond.Losers.Length; i++)
            Losers[i].text = cond.Losers[i].Name;
        GameObject.Destroy(winObj);
    } 
}
