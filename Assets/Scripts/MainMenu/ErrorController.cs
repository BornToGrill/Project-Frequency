using UnityEngine;
using UnityEngine.UI;

class ErrorController : MonoBehaviour {

    public GameObject ErrorDialog;

    void Start() {
        GameObject error = GameObject.Find("ErrorMessage");
        if (error == null)
            return;
        ErrorData data = error.GetComponent<ErrorData>();
        if (data != null) {
            ErrorDialog.SetActive(true);
            ErrorDialog.GetComponentInChildren<Text>().text = data.ErrorMessage;
        }
        Destroy(error);
    }

    public void HideDialog() {
        gameObject.SetActive(false);
    }
}

class ErrorData : MonoBehaviour {
    public string ErrorMessage { get; set; }
}

