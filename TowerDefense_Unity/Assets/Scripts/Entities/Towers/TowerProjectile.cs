using Game.Entities.Interfaces;
using UnityEngine;

namespace Game.Entities.Towers
{
	public class TowerProjectile : MonoBehaviour , IPoolable
	{
		private IDamageable _Target;

		private IAttack _Attack;

		private Vector3 _StartPosition;

		[SerializeField] private float _Speed = 15.0f;

		private bool _IsConducting = false;

		/// <inheritdoc />
		/// <summary>
		/// Used to check whether this object is currently in the game
		/// or is only available in the pool.
		/// [TRUE] if in scene, [FALSE] if only available from pool.
		/// </summary>
		/// <returns> [TRUE] if in scene, [FALSE] if only available from pool.</returns>
		public bool IsConducting() => _IsConducting;

		public void InitializeAndActivate(Vector3 position, IDamageable target, IAttack attack)
		{
			_StartPosition = position;
			_Target = target;
			_Attack = attack;
			
			Activate();
		}

		/// <inheritdoc />
		/// <summary>
		/// Activate this instance. Called when an object has been requested from
		/// the pool.
		/// </summary>
		public void Activate()
		{
			if (_Target == null || _Attack == null)
			{
				Debug.LogError("Initialize the projectile with the parameters before activating it");
				return;
			}

			_IsConducting = true;
			gameObject.SetActive(true);
		}

		private void Update() => GoToTarget();

		private void GoToTarget()
		{
			if (!_IsConducting)
				return;

			transform.position = Vector3.MoveTowards(transform.position, _Target.GetEntity().GetLocation(), _Speed * Time.deltaTime);
			transform.LookAt(_Target.GetEntity().GetLocation());


			if (Vector3.Distance(transform.position, _Target.GetEntity().GetLocation()) > 1.0f)
				return;

			TargetHit();

		}

		private void TargetHit()
		{
			_Attack.ExecuteAttack(_Target, null);
			ReleaseOwnership();
		}

		/// <inheritdoc />
		/// <summary>
		/// Releases ownership of this object and returns it to the pool.
		/// </summary>
		public void ReleaseOwnership()
		{
			_IsConducting = false;

			transform.position = Vector3.zero;
			gameObject.SetActive(false);

			_Target = null;
			_Attack = null;
		}
	}
}
