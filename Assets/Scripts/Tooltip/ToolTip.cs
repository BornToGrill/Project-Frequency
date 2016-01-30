using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ToolTip : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler {

	public GameObject TooltipPrefab;
	public string Title;
	public string Subtitle;
	public string Maintext;
	private string _cost;
	private string _warning;
	private bool _hasEnoughMoney;
	private ToolTipController _tooltip;


	void Start () {
		
	}

	public void SetCost(int cost, bool hasEnoughMoney) {
		_cost = cost.ToString();
		_hasEnoughMoney = hasEnoughMoney;
	}

	public void SetWarning(string warning) {
		_warning = warning;
	}

	public void OnPointerEnter(PointerEventData eventData) {
		GameObject go = Instantiate (TooltipPrefab) as GameObject;
		go.transform.SetParent (transform.parent, false);
		_tooltip = go.GetComponent<ToolTipController> ();
		_tooltip.Initialize (Title, Subtitle, Maintext);
		_tooltip = go.GetComponent<ToolTipController> ();
		_tooltip.SetCost (_cost, _hasEnoughMoney);

		if (_warning != "")
			_tooltip.SetWarning (_warning);
	}

	public void OnPointerExit(PointerEventData eventData) {
		GameObject.Destroy (_tooltip.gameObject);
	}
}