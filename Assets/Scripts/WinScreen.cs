using UnityEngine;
using UnityEngine.UI;
public class WinScreen : MonoBehaviour {
    
    public Text Winner;
    public Text[] Losers;

    // Use this for initialization
    void Start () {

        Winner.text = WinCondition.Winner.Name;
        for (int i = 0; i < WinCondition.Losers.Length; i++)
            Losers[i].text = WinCondition.Losers[i].Name;
    } 
}
