using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	public class HealthBarParentUI : MonoBehaviour
	{
		[Header("Required variables: ")]
		[SerializeField] protected Image _HealthBarMainLayer;
		[SerializeField] protected Image _HealthBarHealthLayer;
		[SerializeField] protected Image _HealthBarDamageLayer;

		[SerializeField] protected Game.Entities.Interfaces.IDamageable _DamageInterface;

		[SerializeField] protected float _DecreaseDamageBarSpeed = .5f;

		[Header("Debug Variables (set by script):")]
		[SerializeField] protected float _CurrentHealth;
		[SerializeField] protected float _MaxHealth;

		[SerializeField] protected Coroutine _HealthBarRoutine;

		/// <summary>
		/// Gets called every time the game object gets turned off
		/// </summary>
		protected void OnDisable()
		{
			//Check if the co routine exists. If it does, then force stop it, since the health bar won't exists anymore.
			if(_HealthBarRoutine != null)
			{
				StopCoroutine(_HealthBarRoutine);
				_HealthBarRoutine = null;
			}
		}
		/// <summary>
		/// Gets called every time the game object gets turned on
		/// </summary>
		protected virtual void OnEnable()
		{
			//Check if the interface is null. If it is, then it shouldn't be allowed to go further then this
			if(_DamageInterface == null)
			{
				Debug.LogError("Please select the Entity Script in the inspector.", this.gameObject);
				this.enabled = false;
				return;
			}

			//Setup the script
			SetStartHealth(_DamageInterface.GetHealth());
		}
		/// <summary>
		/// Initializes the starting variables, making sure everything is here what this needs, and gives out errors if it doesn't know certain information
		/// </summary>
		protected void Initialize()
		{
			//Checking if all of the health bars that have to be adjusted are known to the script, and if not, stop the script here.
			//If we don't do this then there will be more errors in the script later
			if(_HealthBarDamageLayer == null || _HealthBarDamageLayer == null || _HealthBarMainLayer == null)
			{
				if(_HealthBarMainLayer == null)
					Debug.LogError("Please select the Main Health Bar Layer in the inspector.", this.gameObject);
				if(_HealthBarDamageLayer == null)
					Debug.LogError("Please select the Health Bar Damage Layer in the inspector.", this.gameObject);
				if(_HealthBarHealthLayer == null)
					Debug.LogError("Please select the Health Bar Layer in the inspector.", this.gameObject);

				this.enabled = false;
				return;
			}

			//Subscribe our event to its onHit event so we can update if the entity has been hit
			_DamageInterface.OnHit += GetDamageFromEntity;

			//Show the health bars
			ActivateHealthBarUI(true);

			//Settings the health bars to their default fill amount
			_HealthBarHealthLayer.fillAmount = 1;
			_HealthBarDamageLayer.fillAmount = 0;
		}

		protected void GetDamageFromEntity(in Entities.Interfaces.IDamageable sender, in Entities.EventContainers.EntityDamaged payload)
		{
			//Set the damage and start the damage process
			SetDamage(payload.DamageNumber);
		}

		public void SetStartHealth(float health)
		{
			//Settings the health values
			_CurrentHealth = health;
			_MaxHealth = health;

			//Start initializing the rest of the script
			Initialize();
		}
		public void SetDamage(float health)
		{
			//Get the current position of the health bar and store it for later
			Vector3 healthBarDamagePosition = _HealthBarDamageLayer.transform.localPosition;
			//Get the new health
			float newHealth = _CurrentHealth -= health;

			//Set the new x position of the health bar where it will be moved to
			healthBarDamagePosition.x = (_HealthBarHealthLayer.rectTransform.sizeDelta.x / 100) * newHealth;
			//Apply it onto the actual position of the damage health bar
			_HealthBarDamageLayer.transform.localPosition = healthBarDamagePosition;
			//Change the fill amount from the damage health bar so it can start it's *animation*
			_HealthBarDamageLayer.fillAmount = _HealthBarHealthLayer.fillAmount - (newHealth / 100);
			//Change the Health Bars fill amount behind the damage health bar
			_HealthBarHealthLayer.fillAmount = newHealth / 100;

			//Update the current health to the new health
			_CurrentHealth = newHealth;

			//Start the animation of the damage health bar through a Coroutine if it isn't running already
			if(_HealthBarRoutine == null)
				_HealthBarRoutine = StartCoroutine(UpdateHealthBar());
		}

		protected IEnumerator UpdateHealthBar()
		{
			//Loop through this loop and decrease the fill amount from the damage health bar until it reached 0
			while(_HealthBarDamageLayer.fillAmount > 0)
			{
				_HealthBarDamageLayer.fillAmount -= _DecreaseDamageBarSpeed * Time.deltaTime;

				//Wait a small amount before doing it again to make it smooth
				yield return null;
			}

			//Lets the script know that the coroutine is done with running, so it can start a new coroutine
			_HealthBarRoutine = null;
		}

		/// <summary>
		/// Activate/De-active the health bar layers
		/// </summary>
		/// <param name="state">Health bar layers active state</param>
		protected void ActivateHealthBarUI(bool state)
		{
			_HealthBarMainLayer.enabled = state;
			_HealthBarDamageLayer.enabled = state;
			_HealthBarHealthLayer.enabled = state;
		}
	}
}
