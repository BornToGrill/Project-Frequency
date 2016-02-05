using UnityEngine;
using System;
using System.Collections;

public class LandUnit : BaseUnit {

    public int Damage;
    public int Range;

    private int _stackSize;
    internal int _stackHealth;
    internal int _stackDamage;
	internal BaseUnit _attackedBy;

    internal override int StackSize {
        get { return _stackSize; }
        set {
            int diff = value - _stackSize;
            _stackHealth += diff * Health;
            _stackDamage += diff * Damage;
            _stackSize = value;
			Animator anim = GetComponent<Animator> ();
			anim.SetInteger ("StackSize", _stackSize);
        }
    }

    public virtual bool CanMerge(BaseUnit unit) {
        return Owner == unit.Owner && StackSize + unit.StackSize <= MaxUnitStack && gameObject.name == unit.gameObject.name;
    }

    public virtual void Merge(BaseUnit unit) {
		StackSize += unit.StackSize;
        GameObject.Destroy(unit.gameObject);
    }

	public virtual void ReloadAnimation() {
		Animator anim = GetComponent<Animator> ();
		anim.SetInteger ("StackSize", _stackSize);
		anim.Update (Time.deltaTime);
	}

    public void Attack(BaseUnit unit) {
        if (unit.Owner != this.Owner)
        {
            unit.DamageUnit(_stackDamage, this);
        }
    }

    public override void DamageUnit(int damage, BaseUnit attacker) {
		_attackedBy = attacker;
        _stackHealth -= damage;
        AudioSource audio = GetComponent<AudioSource>();
        if(damage < 1000)
            audio.Play();
        if (_stackHealth <= 0) {
			GetComponent<SpriteRenderer> ().color = Color.white;
			StackSize = 0;
			GameObject.Destroy (gameObject, 2f); // Insurance in case it's not destroyed by explosion animation.
            return;
        }
        if(attacker != null)
            GameObject.Find("Board").GetComponent<GameController>().NextQueueItem = false;
        else
            GameObject.Find("Board").GetComponent<GameController>().NextQueueItem = true;

        Animator anim = GetComponent<Animator> ();
		anim.Play ("Damage", 1);

        _stackSize = Mathf.CeilToInt((float)_stackHealth / Health);
        _stackDamage = Damage * _stackSize;
        ReloadAnimation();
    }

	public void DestroyUnit() {
		GameObject.Destroy (gameObject);
	    GameObject.Find("Board").GetComponent<GameController>().NextQueueItem = true;
	}

	public virtual void Retaliate() {
	    if (_attackedBy != null && gameObject != null)
	        _attackedBy.DamageUnit(_stackDamage, null);
	    else
	        GameObject.Find("Board").GetComponent<GameController>().NextQueueItem = true;
		_attackedBy = null;
	}
}