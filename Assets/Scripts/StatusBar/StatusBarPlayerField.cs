using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatusBarPlayerField : MonoBehaviour {

    private RectTransform _rectTransform;
    private Text _playerNumber;
    private Text _goldAmount;
	private Text _generateAmount;
	private Image _background;
	private GameController _gameController;
	private StatusBarController _statusBarController;

	public Player Player;

    void Awake() {
        _rectTransform = gameObject.GetComponent<RectTransform>();
        _playerNumber = transform.Find("PlayerNumber").GetComponent<Text>();
        _goldAmount = transform.Find("GoldAmount").GetComponent<Text>();
        _generateAmount = transform.Find("GenerateAmount").GetComponent<Text>();
		_background = gameObject.GetComponent<Image> ();
		_gameController = GameObject.Find ("Board").GetComponent<GameController> ();
    }

	public void Initialize(Player player, float offset, StatusBarController controller) {
		Player = player;
		_statusBarController = controller;
        _playerNumber.text = Player.Name;
		_playerNumber.color = Player.Color;
        _rectTransform.localScale = new Vector3 (1, 1);
        _rectTransform.anchoredPosition = new Vector2 (offset, -10.0f);
		UpdateStats ();
	}

	public void UpdateStats() {
		_goldAmount.text = Player.MoneyAmount.ToString();
		_generateAmount.text = Player.CalculateIncome().ToString();
	}

	void Update()
	{
        if (Player.IsCurrentPlayer)
        {
            _background.color = new Color(0f, 0f, 0f, 0.5f);
        }
        else
        {
            _background.color = Color.clear;
        }

		if (!Player.IsAlive)
		{
			_statusBarController.DeletePlayerField (this);
		}
	}
}
