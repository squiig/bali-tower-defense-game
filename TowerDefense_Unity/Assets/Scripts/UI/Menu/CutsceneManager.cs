using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class CutsceneManager : MonoBehaviour
{
	[SerializeField] private PlayableDirector _TimeLine;
	[SerializeField] private TMPro.TextMeshProUGUI _CutsceneText;
	private Coroutine _CoRoutine;

	private void OnEnable()
	{
		_TimeLine = GetComponent<PlayableDirector>();

		_CoRoutine = StartCoroutine(TimelineCoRoutine());
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			StopCoroutine(_CoRoutine);

			_CutsceneText.enabled = false;

			Game.Utils.SceneUtility.LoadNext();
		}
	}

	private IEnumerator TimelineCoRoutine()
	{
		_TimeLine.Play();

		while(_TimeLine.state == PlayState.Playing)
			yield return null;

		Game.Utils.SceneUtility.LoadNext();
	}
}
