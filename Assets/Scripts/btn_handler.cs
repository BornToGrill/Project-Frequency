using UnityEngine;
using System.Collections;

public class btn_handler : MonoBehaviour
{

    public void LoadScene(string scene)
    {
        Application.LoadLevel(scene);
    }
}
