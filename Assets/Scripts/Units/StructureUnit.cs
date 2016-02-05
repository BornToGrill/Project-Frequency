using UnityEngine;
using System;

public class StructureUnit : BaseUnit {

    public GameObject[] BuildableUnits;

    internal override int StackSize {
		get { return 1; }
		set {
		    if (value > MaxUnitStack)
		        throw new ArgumentOutOfRangeException(string.Format("Structure can only have {0} stacked units", MaxUnitStack));
		}
	}

	public override int GetCost (Environment environment){
		if (Owner.StartEnvironment != environment)
			return DiscountCost;
		return Cost;
	}

	public int GetCost (Environment environment, Player owner){
		if (owner.StartEnvironment != environment)
			return DiscountCost;
		return Cost;
	}

	public override void DamageUnit(int damage, BaseUnit attacker) {
	    Health -= damage;
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        if (Health <= 0) {
			Animator a = GetComponent<Animator> ();	
			a.SetBool ("Alive", false);

		    WinCondition cond = GetComponent<WinCondition>();
            if(cond != null)
                cond.BaseDestroyed();
			return;
		}

		Animator anim = GetComponent<Animator> ();
		anim.Play ("Damage", 1);
        GameObject.Find("Board").GetComponent<GameController>().NextQueueItem = true;
    }

	public void DestroyStructure() {
		GameObject.Destroy (gameObject);
	    GameObject.Find("Board").GetComponent<GameController>().NextQueueItem = true;
	}
}