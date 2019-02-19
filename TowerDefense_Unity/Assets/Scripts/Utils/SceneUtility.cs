using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Utils
{
    public class SceneUtility : MonoBehaviour
    {
#if UNITY_EDITOR || DEBUG_ENABLED
        [SerializeField] private KeyCode m_ReloadKey = KeyCode.F1;
        [SerializeField] private KeyCode m_NextKey = KeyCode.F2;

        private void Update()
        {
            if (Input.GetKeyUp(m_ReloadKey))
            {
                ReloadActive();
            }

            if (Input.GetKeyUp(m_NextKey))
            {
                LoadNext();
            }
        }
#endif

        public static void ReloadActive()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public static void LoadPrevious()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }

        public static void LoadNext()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}