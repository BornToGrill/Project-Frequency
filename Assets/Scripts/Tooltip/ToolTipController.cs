﻿using UnityEngine;
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

	void Start () {
		rectTransform = transform as RectTransform;
		rectTransform.anchoredPosition = Input.mousePosition;
		//Warning.enabled = false;
		//Cost.enabled = false;
		//Image.enabled = false;
	}

	void Update() {
		//rectTransform.anchoredPosition = Input.mousePosition;
	}

	void FixedUpdate() {
		rectTransform.anchoredPosition = Input.mousePosition;
	}

	public void Initialize(string title, string subtitle, string main) {
		Title.text = title;
		Subtitle.text = subtitle;
		Main.text = main;
	}

	public void SetCost(string cost, bool enoughMoney) {
		Cost.enabled = true;
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
		Warning.enabled = true;
		Warning.text = warning;
	}
}
