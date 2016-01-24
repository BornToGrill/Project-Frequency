using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatusBarPlayerField : MonoBehaviour {

    private RectTransform _rectTransform;
    private Text _playerNumber;
    private Text _goldAmount;
	private Text _generateAmount;

	public Player Player;

    void Awake() {
        _rectTransform = gameObject.GetComponent<RectTransform>();
        _playerNumber = transform.Find("PlayerNumber").GetComponent<Text>();
        _goldAmount = transform.Find("GoldAmount").GetComponent<Text>();
        _generateAmount = transform.Find("GenerateAmount").GetComponent<Text>();
    }

    public void Initialize(RectTransform parent, Player player) {
        _playerNumber.text = "P" + player.PlayerId.ToString();
		Player = player;
        _rectTransform.SetParent(parent);
        _rectTransform.localScale = new Vector3 (1, 1);
        _rectTransform.anchoredPosition = new Vector2 (_rectTransform.rect.width * (player.PlayerId - 1), -10.0f);
		UpdateStats ();
	}

	public void UpdateStats() {
		_goldAmount.text = Player.MoneyAmount.ToString();
		_generateAmount.text = Player.CalculateIncome().ToString();
	}
}
