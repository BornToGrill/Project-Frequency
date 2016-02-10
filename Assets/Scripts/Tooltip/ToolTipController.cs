using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToolTipController : MonoBehaviour {

	public Text Title;
	public Text Subtitle;
	public Text Main;
	public Text Cost;
	public Text Warning;
	public Image Image;
	private RectTransform rectTransform;

	void Awake () {
		rectTransform = (RectTransform)transform;
		SetTooltipPosition();
	}

	void FixedUpdate() {
        SetTooltipPosition();
	}

    public void SetTooltipPosition() {
        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
        rectTransform.position = canvas.transform.TransformPoint(pos);
    }

	public void Initialize(string title, string subtitle, string main) {
		Title.text = title;
		Subtitle.text = subtitle;
		Main.text = main;
	}

	public void SetCost(string cost, bool enoughMoney) {
		Cost.text = cost;
		if (enoughMoney)
		{
			Cost.color = Color.green;
		}
		else
		{
			Cost.color = Color.red;
		}
	}

	public void SetWarning(string warning) {
		Warning.text = warning;
	}
}
