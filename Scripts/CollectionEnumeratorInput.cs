using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace BoneBox
{
    public abstract class CollectionEnumeratorInput<T> : MonoBehaviour
    {
        [SerializeField] private CollectionEnumerator<T> m_CollectionEnumerator = null;

        [Header("Buttons")]
        [SerializeField] private Button m_PreviousButton = null;
        [SerializeField] private Button m_NextButton = null;

        public Button PreviousButton
        {
            get {
                return m_PreviousButton;
            }

            set {
                m_PreviousButton = value;
            }
        }

        public Button NextButton
        {
            get {
                return m_NextButton;
            }

            set {
                m_NextButton = value;
            }
        }

        private void OnEnable()
        {
            if (m_CollectionEnumerator == null)
            {
                m_CollectionEnumerator = this.GetComponent<CollectionEnumerator<T>>();
            }

            HookCollectionEnumerator();
        }

        private void OnDisable()
        {
            UnhookButtons();
            UnhookCollectionEnumerator();
        }

        private void OnNextButtonClicked()
        {
            m_CollectionEnumerator.Next();
        }

        private void OnPreviousButtonClicked()
        {
            m_CollectionEnumerator.Previous();
        }

        private void OnEnumeratorActivating()
        {
            SetButtonsActive(true);
        }

        private void OnEnumeratorDeactivating()
        {
            SetButtonsActive(false);
        }

        private void SetButtonsActive(bool active)
        {
            if (active)
            {
                HookButtons();
            }
            else
            {
                UnhookButtons();
            }

            m_NextButton.gameObject.SetActive(active);
            m_PreviousButton.gameObject.SetActive(active);
        }

        private void HookCollectionEnumerator()
        {
            m_CollectionEnumerator.Activating += OnEnumeratorActivating;
            m_CollectionEnumerator.Deactivating += OnEnumeratorDeactivating;
        }

        private void UnhookCollectionEnumerator()
        {
            m_CollectionEnumerator.Activating -= OnEnumeratorActivating;
            m_CollectionEnumerator.Deactivating -= OnEnumeratorDeactivating;
        }

        private void HookButtons()
        {
            m_PreviousButton.onClick.AddListener(OnPreviousButtonClicked);
            m_NextButton.onClick.AddListener(OnNextButtonClicked);
        }

        private void UnhookButtons()
        {
            m_PreviousButton.onClick.RemoveListener(OnPreviousButtonClicked);
            m_NextButton.onClick.RemoveListener(OnNextButtonClicked);
        }
    }
}
