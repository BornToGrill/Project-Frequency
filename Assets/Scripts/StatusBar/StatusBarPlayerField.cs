using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatusBarPlayerField : MonoBehaviour {
	private Text _goldAmount;
	private Text _generateAmount;

	public Player _player;
	private RectTransform rt;

	public void Initialize(RectTransform parent, Player player) {
		rt = gameObject.GetComponent<RectTransform> ();
		transform.GetChild (2).GetComponent<Text> ().text = "P" + player.PlayerId.ToString ();
		_goldAmount = transform.GetChild (1).GetComponent<Text>();
		_generateAmount = transform.GetChild (4).GetComponent<Text>();

		_player = player;
	    rt.SetParent(parent);
		rt.localScale = new Vector3 (1, 1);
		rt.anchoredPosition = new Vector2 (rt.rect.width * (player.PlayerId - 1), -10.0f);
		UpdateStats ();
	}

	public void UpdateStats() {
		_goldAmount.text = _player.MoneyAmount.ToString();
		_generateAmount.text = 0.ToString();
	}
}
