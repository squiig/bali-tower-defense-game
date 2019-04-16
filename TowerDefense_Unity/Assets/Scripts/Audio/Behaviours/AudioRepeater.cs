using System.Collections;
using Game.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Audio.Bahaviours
{
	public class AudioRepeater : MonoBehaviour
	{
		private Coroutine _Coroutine;
		[SerializeField] private MinMaxFloat _MinMaxFloat;
		[SerializeField] private string _Identifier = "";
		[SerializeField] private bool _PlayFasterOnStart = true;

		void OnEnable()
		{
			_Coroutine = StartCoroutine(RepeatRoutine());
		}

		void OnDisable()
		{
			StopCoroutine(_Coroutine);
		}

		private IEnumerator RepeatRoutine()
		{
			if (_PlayFasterOnStart)
			{
				yield return new WaitForSeconds(Random.Range(0, _MinMaxFloat.Max));
				Audio.SendEvent(new AudioEvent(this, AudioCommands.PLAY, _Identifier, followTransform: transform));
			}

			while (true)
			{
				yield return new WaitForSeconds(_MinMaxFloat.GetRandom());
				Audio.SendEvent(new AudioEvent(this, AudioCommands.PLAY, _Identifier, followTransform:transform));
			}
		}
	}
}
