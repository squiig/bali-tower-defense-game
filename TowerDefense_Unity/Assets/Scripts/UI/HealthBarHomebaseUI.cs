using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
	public class HealthBarHomebaseUI : HealthBarBaseUI
	{
		[SerializeField] private Game.Entities.Base.HomeBase _HomebaseScript;

		protected override void OnEnable()
		{
			//Check if the interface is null. If it is, then it shouldn't be allowed to go further then this
			if(_HomebaseScript == null)
			{
				Debug.LogError("Please select the HomeBase Script in the inspector.", this.gameObject);

				this.enabled = false;
				return;
			}

			_DamageInterface = _HomebaseScript;

			base.OnEnable();
		}
	}
}
