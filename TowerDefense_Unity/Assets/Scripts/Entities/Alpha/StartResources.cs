using System.Collections;
using System.Collections.Generic;
using System.Data;
using Game.Entities;
using Game.Entities.EventContainers;
using UnityEngine;

public class StartResources : MonoBehaviour
{
	[SerializeField] private int _StartResource;
	[Space]
	[SerializeField] private float _TickInterval;
	[SerializeField] private int _TickAmount;

	private float _TickTime;

	private void Awake()
	{
		if(!ResourceSystem.Instance.RunTransaction(_StartResource))
			throw new DataException($"Failed to create start resource with count of {_StartResource}");

		ResourceSystem.Instance.OnTransaction += InstanceOnOnTransaction;
		_TickTime = _TickInterval;
	}

	private void InstanceOnOnTransaction(in ResourceSystem sender, in TransactionResult payload)
	{
		Debug.Log($"User now has {payload.ResourceCount}");
	}

	private void Update()
	{
		if (_TickTime > 0)
			_TickTime -= Time.deltaTime;

		if(_TickTime > 0)
			return;

		_TickTime = _TickInterval;
		ResourceSystem.Instance.RunTransaction(_TickAmount);
	}
}
