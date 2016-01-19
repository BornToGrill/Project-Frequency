﻿using UnityEngine;
using System;

public class StructureUnit : BaseUnit {

	private int _stackSize;
	private int _stackHealth;
	private int _stackDamage;

	internal override int StackSize {
		get { return _stackSize; }
		set {
			int diff = Mathf.Abs(value - _stackSize);
			_stackHealth += diff * Health;
			_stackDamage = 0;
			_stackSize = value;
		}
	}

	public override int GetCost (Environment environment){
		if (Owner.GetComponent<Player> ().StartEnvironment != environment)
			return DiscountCost;
		return Cost;
	}

	public int checkEnoughMoney() {
		int cost = GetCost(Owner.GetComponent<Player> ().StartEnvironment);
		if (Owner.GetComponent<Player> ().MoneyAmount >= cost)
			return cost;
		return 0;
	}

	private void withdrawMoney(int cost) {
		Owner.GetComponent<Player> ().MoneyAmount -= cost;
	}

	public void drawStructure() {

	}

	public void createStructure(string structure) {
		int check = checkEnoughMoney();
		if (check != 0) {
			if (withdrawMoney (check))
				drawStructure ();
		} else {
			throw new InvalidOperationException("Not enough money to build a barrack");
		}
	}

	public override void DamageUnit(int damage) {
		_stackHealth -= damage;
		if (_stackHealth <= 0) {
			GameObject.Destroy(gameObject);
			return;
		}
		_stackSize = Mathf.CeilToInt(_stackHealth / Health);
		_stackDamage = 0;
	}
}