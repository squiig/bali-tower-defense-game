using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayableDirector))]
public class CutsceneManager : MonoBehaviour
{
	[SerializeField] private PlayableDirector _timeLine;

	private void OnEnable()
	{
		_timeLine = GetComponent<PlayableDirector>();

		StartCoroutine(TimelineCoRoutine());
	}

	private IEnumerator TimelineCoRoutine()
	{
		_timeLine.Play();

		while(_timeLine.state == PlayState.Playing)
			yield return null;

		//TODO: Replace this with Stans scene manager
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
