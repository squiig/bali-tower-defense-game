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

		private GameManager _GameManager;

		private void OnEnable()
		{
			_GameManager = GameManager.Instance;
			_GameManager.GameOver += OnGameOver;
		}

		private void OnDisable()
		{
			if (!_GameManager)
				return;

			_GameManager.GameOver -= OnGameOver;
		}

		private void OnGameOver(GameOverEventArgs eventArgs)
		{
			if (eventArgs.HasWon)
			{
				_WinTextRenderer.enabled = true;
				_LoseTextRenderer.enabled = false;
			}
			else
			{
				_LoseTextRenderer.enabled = true;
				_WinTextRenderer.enabled = false;
			}
		}
	}
}
