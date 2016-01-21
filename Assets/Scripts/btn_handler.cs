using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class btn_handler : MonoBehaviour
{

    public void LoadScene(string scene) {

        SceneManager.LoadScene(scene);
    }
}
