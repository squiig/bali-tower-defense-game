using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
	public class HealthBarMinionUI : HealthBarParentUI
	{
		protected override void OnEnable()
		{
			_DamageInterface = GetComponent<Game.Entities.MovingEntities.Minion>();
			base.OnEnable();
		}
	}
}
