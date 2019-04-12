using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Game.UI
{
	public class GameOverUI : MonoBehaviour
	{
		[SerializeField] private TMP_Text _WinTextRenderer = null;
		[SerializeField] private TMP_Text _LoseTextRenderer = null;
		[SerializeField] private float _TimeUntilRestart = 5f;

		private GameManager _GameManager;

		private void OnEnable()
		{
			_GameManager = GameManager.Instance;
			_GameManager.GameOver += OnGameOver;
		}

		private void Start()
		{
			Deactivate();
		}

		private void OnDisable()
		{
			if (!_GameManager)
				return;

			_GameManager.GameOver -= OnGameOver;
		}

		private void OnGameOver(GameOverEventArgs eventArgs)
		{
			StartCoroutine(GameOverRoutine(eventArgs));
		}

		private IEnumerator GameOverRoutine(GameOverEventArgs eventArgs)
		{
			Activate(eventArgs.HasWon);
			yield return new WaitForSeconds(_TimeUntilRestart);
			_GameManager.RestartGame();
		}

		public void Activate(bool hasWon)
		{
			_WinTextRenderer.enabled = hasWon;
			_LoseTextRenderer.enabled = !hasWon;
		}

		public void Deactivate()
		{
			_WinTextRenderer.enabled = false;
			_LoseTextRenderer.enabled = false;
		}
	}
}
