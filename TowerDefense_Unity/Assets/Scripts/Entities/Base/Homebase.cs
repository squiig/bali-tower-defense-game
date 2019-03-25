using System;
using System.Linq;
using Game.Entities.EventContainers;
using Game.Entities.Interfaces;
using Game.Entities.MovingEntities;
using UnityEngine;

namespace Game.Entities.Base
{
	public class HomeBase : Entity, IDamageable
	{	
		[SerializeField] private float _Health;

		public bool IsConducting() => true;

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

		public Vector3 GetPosition() => transform.position;

		public int GetPriority() => int.MaxValue;

		public Allegiance GetAllegiance() => Allegiance.FRIENDLY;

		public GameObject GetEntity() => GetInstance();

		private void Awake()
		{
			MemoryObjectPool<IDamageable>.Instance.Add(this);
		}

		/// <inheritdoc />
		/// <summary>
		/// Used when this instance gets hit.
		/// Will apply the contents of the <see cref="T:Game.Entities.OnHitEffects" />
		/// Will fire the events.
		/// </summary>
		/// <param name="onHitEffects"> Class containing data about the attack</param>
		public void ApplyOnHitEffects(in OnHitEffects onHitEffects)
		{
			float previousHealth = _Health;
			_Health -= onHitEffects.GetDamage();

			EntityDamaged payload = new EntityDamaged(this, _Health, previousHealth);

			OnHit?.Invoke(this, payload);

			if (_Health <= 0)
				OnDeath?.Invoke(this, payload);

			if (onHitEffects.GetStatusEffects().Any(x => x == StatusEffects.SLOWED))
				throw new NotImplementedException("Have not implemented movement impairing effects yet.");
		}

		/// <inheritdoc />
		/// <summary>
		/// Used to get the current health of this instance
		/// When first spawned will always indicate max health.
		/// </summary>
		/// <returns> The current health of this instance. </returns>
		public float GetHealth() => _Health;
	}
}
