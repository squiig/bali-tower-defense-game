using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
	public class HealthBarDebugUI : HealthBarBaseUI
	{
		private void Start()
		{
			SetStartHealth(100f);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
				SetDamage(20f);
		}
	}
}
