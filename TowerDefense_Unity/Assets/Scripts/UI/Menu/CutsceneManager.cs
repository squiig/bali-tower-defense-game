using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayableDirector))]
public class CutsceneManager : MonoBehaviour
{
	[SerializeField] private PlayableDirector _TimeLine;

	private void OnEnable()
	{
		_TimeLine = GetComponent<PlayableDirector>();

		StartCoroutine(TimelineCoRoutine());
	}

	private IEnumerator TimelineCoRoutine()
	{
		_TimeLine.Play();

		while(_TimeLine.state == PlayState.Playing)
			yield return null;

		Game.Utils.SceneUtility.LoadNext();
	}
}
