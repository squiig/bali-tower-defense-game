using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
	public class HealthBarMinionUI : HealthBarBaseUI
	{
		[SerializeField] private Game.Entities.MovingEntities.Minion _MinionScript;

		protected override void OnEnable()
		{
			StartMinionHealthBar();
		}

		public void StartMinionHealthBar()
		{
			//Check if the interface is null. If it is, then it shouldn't be allowed to go further then this
			if(_MinionScript == null)
			{
				Debug.LogError("Please select the Minion Script in the inspector.", this.gameObject);

				this.enabled = false;
				return;
			}

			_DamageInterface = _MinionScript;

			base.OnEnable();
		}
	}
}
