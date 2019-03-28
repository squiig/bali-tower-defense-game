using UnityEngine;
using Game.Entities.Interfaces;
using Game.Entities.EventContainers;
using Game.Entities;


public class PlayerCore : Entity, IDamageable
{
	float _MaxHealth = 100;
	float _Health = 100;


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
		OnDeath.Invoke(this, new EntityDamaged(this, 0, 0));
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
		float newHealth = onHitEffects.GetDamage();
		OnHit.Invoke(this, new EntityDamaged(this, newHealth, _Health));

		_Health = newHealth;

		if (_Health <= 0)
		{
			ReleaseOwnership();
		}
    }

    public float GetHealth()
    {
	    return _Health;
    }

	Entity IDamageable.GetEntity()
	{
		return (Entity)this;
	}
}
