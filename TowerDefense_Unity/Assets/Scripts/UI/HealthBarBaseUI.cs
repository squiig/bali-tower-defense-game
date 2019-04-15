using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	public class HealthBarBaseUI : MonoBehaviour
	{
		[Header("Required variables: ")]
		[SerializeField] protected Image _HealthBarMainLayer;
		[SerializeField] protected Image _HealthBarHealthLayer;
		[SerializeField] protected Image _HealthBarDamageLayer;

		[SerializeField] protected float _DecreaseDamageBarSpeed = .5f;

		protected float _CurrentHealth;
		protected float _MaxHealth;

		protected Entities.Interfaces.IDamageable _DamageInterface;

		protected Coroutine _HealthBarRoutine;

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
			//Subscribe our event to its onHit event so we can update if the entity has been hit
			_DamageInterface.OnHit += GetDamageFromEntity;

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
			if(_HealthBarDamageLayer == null || _HealthBarDamageLayer == null || _HealthBarMainLayer == null/* || _DamageTextObject == null*/)
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

			//Show the health bars
			ActivateHealthBarUI(true);

			//Settings the health bars to their default fill amount
			_HealthBarHealthLayer.fillAmount = 1;
			_HealthBarDamageLayer.fillAmount = 0;
		}

		protected void GetDamageFromEntity(in Entities.Interfaces.IDamageable sender, in Entities.EventContainers.EntityDamaged payload)
		{
			//Set the damage and start the damage process
			_CurrentHealth = payload.Health;
			SetDamage();
		}

		public void SetStartHealth(float health)
		{
			//Settings the health values
			_CurrentHealth = health;
			_MaxHealth = health;

			//Start initializing the rest of the script
			Initialize();
		}
		public void SetDamage()
		{
			//Making sure its impossible to enter this when the health is (below) zero
			if (_CurrentHealth <= 0)
			{
				Debug.LogError("This should not happen.");
				this.enabled = false;
				return;
			}

			//Get the current position of the health bar and store it for later
			Vector3 healthBarDamagePosition = _HealthBarDamageLayer.transform.localPosition;

			//Set the new x position of the health bar where it will be moved to
			healthBarDamagePosition.x = (_HealthBarHealthLayer.rectTransform.sizeDelta.x / _MaxHealth) * _CurrentHealth;
			//Apply it onto the actual position of the damage health bar
			_HealthBarDamageLayer.transform.localPosition = healthBarDamagePosition;
			//Change the fill amount from the damage health bar so it can start it's *animation*
			_HealthBarDamageLayer.fillAmount = _HealthBarHealthLayer.fillAmount - (_CurrentHealth / _MaxHealth);
			//Change the Health Bars fill amount behind the damage health bar
			_HealthBarHealthLayer.fillAmount = _CurrentHealth / _MaxHealth;

			//Start the animation of the damage health bar through a Coroutine if it isn't running already
			if(_HealthBarRoutine == null)
				_HealthBarRoutine = StartCoroutine(UpdateHealthBar());
		}
		public void SetDamage(float damage)
		{
			//Only here for debugging reasons, so it shouldn't run outside of the editor
			if(!Application.isEditor)
				return;

			//Making sure its impossible to enter this when the health is (below) zero
			if(_CurrentHealth <= 0)
			{
				Debug.LogError("This should not happen.");
				this.enabled = false;
				return;
			}

			_CurrentHealth = _CurrentHealth - damage;

			//Get the current position of the health bar and store it for later
			Vector3 healthBarDamagePosition = _HealthBarDamageLayer.transform.localPosition;

			//Set the new x position of the health bar where it will be moved to
			healthBarDamagePosition.x = (_HealthBarHealthLayer.rectTransform.sizeDelta.x / _MaxHealth) * _CurrentHealth;
			//Apply it onto the actual position of the damage health bar
			_HealthBarDamageLayer.transform.localPosition = healthBarDamagePosition;
			//Change the fill amount from the damage health bar so it can start it's *animation*
			_HealthBarDamageLayer.fillAmount = _HealthBarHealthLayer.fillAmount - (_CurrentHealth / _MaxHealth);
			//Change the Health Bars fill amount behind the damage health bar
			_HealthBarHealthLayer.fillAmount = _CurrentHealth / _MaxHealth;

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
