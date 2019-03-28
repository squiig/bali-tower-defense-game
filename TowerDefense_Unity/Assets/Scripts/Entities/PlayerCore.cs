using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Entities.Interfaces;
using Game.Entities.EventContainers;
using Game.Entities;


public class PlayerCore : Entity, IDamageable
{

    public bool IsConducting()
    {
	    throw new System.NotImplementedException();
    }

    public void Activate()
    {
	    throw new System.NotImplementedException();
    }

    public void ReleaseOwnership()
    {
	    throw new System.NotImplementedException();
    }

    public event TypedEventHandler<IDamageable, EntityDamaged> OnHit;
    public event TypedEventHandler<IDamageable, EntityDamaged> OnDeath;

    public Vector3 GetPosition()
    {
	    return transform.position;
    }

    public int GetPriority()
    {
	    return 0;
    }

    public Allegiance GetAllegiance()
    {
	    return Allegiance.FRIENDLY;
    }

    public GameObject GetEntity()
    {
		return gameObject;
    }

    public void ApplyOnHitEffects(in OnHitEffects onHitEffects)
    {
	    Debug.Log(name + " got hit!");
    }

    public float GetHealth()
    {
	    return 100;
    }

	Entity IDamageable.GetEntity()
	{
		return (Entity)this;
	}
}
