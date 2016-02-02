using UnityEngine;
using System.Collections;

public class SetActiveHandler : MonoBehaviour {

    public GameObject Object;
   
	public void SetObjectActive()
    {
        Object.SetActive(true);
    }
}
