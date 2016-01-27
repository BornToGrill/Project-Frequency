using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class WinScreen : MonoBehaviour {
    
	private int winner;
	private List<Player> losers;
    private Text _winnerName;
    public GameObject loserText;
    public List<GameObject> loserList;
    private float yOffSet = 0;
    public float margin = 20;



    // Use this for initialization
    void Start ()
    {
        winner = WinCondition.winner.PlayerId;
        _winnerName = GameObject.Find("winnerName").GetComponent<Text>();
        _winnerName.text = "Player " + winner.ToString();
        losers = WinCondition.losers;
        for (int i =0; i < losers.Count; i++)
        {
            AddLoser("Player " + losers[i].PlayerId.ToString());
        }
             
        loserList = new List<GameObject>();
    } 
	
	void AddLoser (string name)
    {
        margin += 10;
        GameController gc = GetComponent<GameController>();
        GameObject loserName = Instantiate(loserText) as GameObject;
        yOffSet += loserName.GetComponent<Text>().rectTransform.rect.height;
        loserName.transform.SetParent(transform);
        loserName.GetComponent<Text>().text = name;
        loserName.transform.position = new Vector2(transform.position.x, transform.position.y - (yOffSet - 12) - margin);
        loserList.Add(loserName);
    }

    
}
