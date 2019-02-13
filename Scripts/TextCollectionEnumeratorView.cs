using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using TMPro;

namespace BoneBox
{
    using ScriptableObjects.ValueContainers;

    public class TextCollectionEnumeratorView : MonoBehaviour
    {
        [SerializeField] private CollectionEnumerator<StringContainer> m_CollectionEnumerator = null;

        [SerializeField] private TextMeshProUGUI m_TextRenderer = null;

        private void OnEnable()
        {
            if (m_CollectionEnumerator == null)
            {
                m_CollectionEnumerator = this.GetComponent<CollectionEnumerator<StringContainer>>();
            }

            m_CollectionEnumerator.ValueChanged += OnValueChanged;
        }

        private void OnDisable()
        {
            m_CollectionEnumerator.ValueChanged -= OnValueChanged;
        }

        private void OnValueChanged(StringContainer container)
        {
            m_TextRenderer.text = container.Value;
        }
    }
}
