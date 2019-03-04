using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class EnemyUI : MonoBehaviour
    {
        [SerializeField] private Image _HealthBarHealthLayer;
        [SerializeField] private Image _HealthBarDamageLayer;

        [SerializeField] private float _CurrentHealth;
        [SerializeField] private float _MaxHealth;

        private void Start()
        {
            if(_HealthBarDamageLayer == null || _HealthBarDamageLayer == null)
            {
                if (_HealthBarDamageLayer == null)
                    Debug.LogError("Please select the Health Bar Damage Layer in the inspector.", this.gameObject);
                if(_HealthBarHealthLayer == null)
                    Debug.LogError("Please select the Health Bar Layer in the inspector.", this.gameObject);

                this.enabled = false;
                return;
            }

            _HealthBarHealthLayer.fillAmount = 1;
            _HealthBarDamageLayer.fillAmount = 0;
        }

        public void SetStartHealth(float health)
        {
            //TODO: Subscribe to the enemy script to know when the damage is done
            
            _CurrentHealth = health;
            _MaxHealth = health;
        }
        public void SetHealthDamage(float health)
        {
            _CurrentHealth -= health;

            StartCoroutine(UpdateHealthBar());
        }

        private IEnumerator UpdateHealthBar()
        {
            float healthPercentage = _CurrentHealth / 100;

            Debug.Log(_HealthBarHealthLayer.fillAmount);
            Debug.Log(healthPercentage);

            while(_HealthBarHealthLayer.fillAmount >= healthPercentage)
            {
                _HealthBarHealthLayer.fillAmount -= 0.01f;

                yield return new WaitForSeconds(0.01f);
            }
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                SetHealthDamage(10f);
            }
        }
    }
}
