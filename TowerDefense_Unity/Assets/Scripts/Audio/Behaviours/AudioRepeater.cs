using System.Collections;
using Game.Utils;
using UnityEngine;

namespace Game.Audio.Bahaviours
{
	public class AudioRepeater : MonoBehaviour
	{
		private Coroutine _Coroutine;
		[SerializeField] private MinMaxFloat MinMaxFloat;
		[SerializeField] private string Identifier = "";

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
			while (true)
			{
				yield return new WaitForSeconds(MinMaxFloat.GetRandom());
				Audio.SendEvent(new AudioEvent(this, AudioCommands.PLAY, Identifier, followTransform:transform));
			}
		}
	}
}
