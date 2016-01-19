using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Player : MonoBehaviour {

    public int Number;
    public int MoneyAmount;
    public Color Color;
    public Environment StartEnvironment;
    public List<GameObject> units;
	
    // Use this for initialization
	void Start () {
        switch (Number) {
            case 1:
                Color = Color.red;
                break;
            case 2:
                Color = Color.gray;
                break;
            case 3:
                Color = Color.cyan;
                break;
            case 4:
                Color = Color.green;
                break;
            default:
                Color = Color.black;
                break;
        };
	}
    void GenerateMoney() {
        throw new System.NotImplementedException();
    }
	

	// Update is called once per frame
	void Update () {
	
	}
}
