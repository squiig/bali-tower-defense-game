using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace BoneBox
{
    using ScriptableObjects.ValueContainers;

    public class ImageCollectionEnumeratorView : MonoBehaviour
    {
        [SerializeField] private CollectionEnumerator<SpriteContainer> m_CollectionEnumerator = null;

        [SerializeField] private Image m_ImageRenderer = null;

        private void OnEnable()
        {
            if (m_CollectionEnumerator == null)
            {
                m_CollectionEnumerator = this.GetComponent<CollectionEnumerator<SpriteContainer>>();
            }

            m_CollectionEnumerator.ValueChanged += OnValueChanged;
        }

        private void OnDisable()
        {
            m_CollectionEnumerator.ValueChanged -= OnValueChanged;
        }

        private void OnValueChanged(SpriteContainer container)
        {
            m_ImageRenderer.sprite = container.Value;
        }
    }
}
