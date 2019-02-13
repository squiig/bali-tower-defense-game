using System;
using System.Collections.Generic;

using UnityEngine;

namespace BoneBox
{
    public abstract class CollectionEnumerator<T> : MonoBehaviour
    {
        private Action<T> m_ValueChanged;
        private Action m_Activating;
        private Action m_Deactivating;

        public event Action<T> ValueChanged
        {
            add {
                m_ValueChanged += value;
            }
            remove {
                m_ValueChanged -= value;
            }
        }

        public event Action Activating
        {
            add {
                m_Activating += value;
            }
            remove {
                m_Activating -= value;
            }
        }

        public event Action Deactivating
        {
            add {
                m_Deactivating += value;
            }
            remove {
                m_Deactivating -= value;
            }
        }

        [SerializeField] private List<T> m_Values;

        private bool m_IsLocked;
        private int m_CurrentIndex = 0;

        public int CurrentIndex => m_CurrentIndex;
        public T CurrentValue => m_Values[m_CurrentIndex];

        public bool IsLocked
        {
            get {
                return m_IsLocked;
            }

            set {
                m_IsLocked = value;
            }
        }

        public List<T> Values
        {
            get {
                return m_Values;
            }

            set {
                m_Values = value;
            }
        }

        public void Next()
        {
            if (m_IsLocked)
                return;

            ++m_CurrentIndex;

            // Loop around when out of bounds
            if (m_CurrentIndex >= m_Values.Count)
                m_CurrentIndex = 0;

            OnValueChanged();
        }

        public void Previous()
        {
            if (m_IsLocked)
                return;

            --m_CurrentIndex;

            // Loop around when out of bounds
            if (m_CurrentIndex < 0)
                m_CurrentIndex = m_Values.Count - 1;

            OnValueChanged();
        }

        public void Goto(int index)
        {
            if (m_IsLocked)
                return;

            m_CurrentIndex = Mathf.Clamp(index, 0, m_Values.Count);
            OnValueChanged();
        }

        public void GotoRandom()
        {
            Goto(UnityEngine.Random.Range(0, m_Values.Count));
        }

        public void SetActive(bool active)
        {
            if (active)
            {
                OnEnumeratorActivate();
            }
            else
            {
                OnEnumeratorDeactivate();
            }

            IsLocked = !active;
        }

        protected virtual void OnEnumeratorActivate()
        {
            m_Activating?.Invoke();
        }

        protected virtual void OnEnumeratorDeactivate()
        {
            m_Deactivating?.Invoke();
        }

        protected virtual void OnValueChanged()
        {
            m_ValueChanged?.Invoke(CurrentValue);
        }
    }
}
