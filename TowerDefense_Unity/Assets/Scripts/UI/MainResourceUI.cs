using System.Collections;
using System.Collections.Generic;
using Game.Entities;
using UnityEngine;

namespace Game.UI
{
	/// <summary>
	/// This component makes sure the corresponding main resource value in the backend gets properly displayed in the UI elements.
	/// </summary>
	public class MainResourceUI : ResourceUI
	{
		private void OnEnable()
		{
			ResourceSystem.Instance.OnTransaction += ResourceSystem_OnTransaction;
		}

		private void OnDisable()
		{
			ResourceSystem.Instance.OnTransaction -= ResourceSystem_OnTransaction;
		}

		private void ResourceSystem_OnTransaction(in ResourceSystem sender, in Entities.EventContainers.TransactionResult result)
		{
			if (result.HasTransactionFailed)
				return;

			SetAmount(result.ResourceCount);
		}
	}
}
