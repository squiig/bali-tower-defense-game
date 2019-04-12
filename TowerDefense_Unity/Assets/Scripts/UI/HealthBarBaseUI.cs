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

		protected GameObject _DamageTextObject;

		[SerializeField] protected float _DecreaseDamageBarSpeed = .5f;

		protected float _CurrentHealth;
		protected float _MaxHealth;

		protected Entities.Interfaces.IDamageable _DamageInterface;

		protected Collider _Collider;

		protected Coroutine _HealthBarRoutine;
		protected Coroutine _DamageTextRoutine;

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
				//if (_DamageTextObject == null)
				//	Debug.LogError("Please select the Damage Text Object in the inspector", this.gameObject);

				this.enabled = false;
				return;
			}
			
			//Get the collider the get the most acurate bounds
			_Collider = RecursiveParentFindCollider(transform);

			//Show the health bars
			ActivateHealthBarUI(true);

			_DamageTextObject.SetActive(false);

			//Settings the health bars to their default fill amount
			_HealthBarHealthLayer.fillAmount = 1;
			_HealthBarDamageLayer.fillAmount = 0;
		}

		protected Collider RecursiveParentFindCollider(Transform transform)
		{
			Collider collider = transform.GetComponent<Collider>();

			if (collider)
				return collider;

			if (transform == transform.root)
			{
				return null;
			}
			else
			{
				return RecursiveParentFindCollider(transform.parent);
			}
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
		public void SetDamage(float damage)
		{
			//Get the current position of the health bar and store it for later
			Vector3 healthBarDamagePosition = _HealthBarDamageLayer.transform.localPosition;
			//Get the new health
			float newHealth = _CurrentHealth -= damage;

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

			//if (_DamageTextRoutine == null)
			//	_DamageTextRoutine = StartCoroutine(ShowFloatingDamageText(damage));
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
		protected IEnumerator ShowFloatingDamageText(float damage)
		{
			_DamageTextObject.SetActive(true);

			float time = Random.Range(_Collider.bounds.min.x, _Collider.bounds.max.x);

			//Debug.Log(_DamageTextObject.GetComponent<MeshRenderer>().bounds.min.x);
			//Debug.Log(_DamageTextObject.GetComponent<MeshRenderer>().bounds.max.x);
			//Debug.Log(time);

			Animator animator = null;
			Vector2 randomPosition = new Vector2(time, _DamageTextObject.transform.position.y);
			_DamageTextObject.transform.position = randomPosition;

			animator = _DamageTextObject.GetComponent<Animator>();

			_DamageTextObject.GetComponent<TMPro.TextMeshPro>().text = damage.ToString();

			yield return new WaitForSeconds(.5f);

			_DamageTextObject.SetActive(false);

			_DamageTextRoutine = null;
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
