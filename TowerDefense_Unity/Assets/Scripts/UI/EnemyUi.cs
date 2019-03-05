using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class EnemyUI : MonoBehaviour
    {
	    [SerializeField] private Image _HealthBarMainLayer;
        [SerializeField] private Image _HealthBarHealthLayer;
        [SerializeField] private Image _HealthBarDamageLayer;

        [SerializeField] private float _CurrentHealth;
        [SerializeField] private float _MaxHealth;

		/// <summary>
		/// Initializes the starting variables, making sure everything is here what this needs, and gives out errors if it doesn't know certain inforamtion
		/// </summary>
        private void Initialize()
        {
			//Checking if all of the health bars that have to be adjusted are known to the script, and if not, stop the script here.
			//If we don't do this then there will be more errors in the script later
			if(_HealthBarDamageLayer == null || _HealthBarDamageLayer == null)
            {
				if(_HealthBarMainLayer == null)
					Debug.LogError("Please select the Main Health Bar Layer in the inspector.", this.gameObject);
				if (_HealthBarDamageLayer == null)
                    Debug.LogError("Please select the Health Bar Damage Layer in the inspector.", this.gameObject);
                if(_HealthBarHealthLayer == null)
                    Debug.LogError("Please select the Health Bar Layer in the inspector.", this.gameObject);

                this.enabled = false;
                return;
            }

			//Show the health bars
			Activate(true);

			//Settings the health bars to their default fill amount
            _HealthBarHealthLayer.fillAmount = 1;
            _HealthBarDamageLayer.fillAmount = 0;
        }

        public void SetStartHealth(float health)
        {
            //TODO: Subscribe to the start event of the enemy to know how much health the enemy/wall has
            //Settings the health values
            _CurrentHealth = health;
            _MaxHealth = health;

			//Start initializing the rest of the script
            Initialize();
        }
        public void SetHealthDamage(float health)
        {
			//TODO: Subscribe to the hit event from the enemy/wall
			//Get the current position of the health bar and store it for later
	        Vector3 healthBarDamagePosition = _HealthBarDamageLayer.transform.localPosition;
			//Get the new health
			float newHealth = _CurrentHealth -= health;

			if(newHealth <= 0)
			{
				Activate(false);
				return;
			}

			//Set the new x position of the health bar where it will be moved to
			healthBarDamagePosition.x = (_HealthBarHealthLayer.rectTransform.sizeDelta.x / 100) * newHealth;
			//Apply it onto the actual position of the damage health bar
			_HealthBarDamageLayer.transform.localPosition = healthBarDamagePosition;
			//Change the fill amount from the damage health bar so it can start it's *animation*
			_HealthBarDamageLayer.fillAmount = (_HealthBarHealthLayer.rectTransform.sizeDelta.x - healthBarDamagePosition.x) * (_HealthBarHealthLayer.rectTransform.sizeDelta.x * _HealthBarHealthLayer.fillAmount);

			//Change the Health Bars fill amount behind the damage health bar
			_HealthBarHealthLayer.fillAmount = newHealth / 100;

			//Update the current health to the new health
			_CurrentHealth = newHealth;

			//Start the animation of the damage health bar through a Coroutine
			StartCoroutine(UpdateHealthBar());
        }

        private IEnumerator UpdateHealthBar()
        {
			//Loop through this loop and decrease the fill amount from the damage health bar until it reached 0
            while(_HealthBarDamageLayer.fillAmount > 0)
            {
	            _HealthBarDamageLayer.fillAmount -= 0.01f;

				//Wait a small amount before doing it again to make it smooth
                yield return new WaitForSeconds(0.01f);
            }
        }

		/// <summary>
		/// Activate/De-active the health bar layers
		/// </summary>
		/// <param name="state">Health bar layers active state</param>
		private void Activate(bool state)
		{
			_HealthBarMainLayer.enabled = state;
			_HealthBarDamageLayer.enabled = state;
			_HealthBarHealthLayer.enabled = state;
		}
	}
}
