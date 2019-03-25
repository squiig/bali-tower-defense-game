using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
	public class HealthBarBaseUI : HealthBarParentUI
	{
		protected override void OnEnable()
		{
			_DamageInterface = GetComponent<Game.Entities.Base.HomeBase>();
			base.OnEnable();
		}
	}
}
