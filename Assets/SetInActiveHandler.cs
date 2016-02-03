using UnityEngine;
using System.Collections;

public class SetInActiveHandler : MonoBehaviour {

    public GameObject Object;

	public void SetInActive()
    {
        Object.SetActive(false);

    }
}
