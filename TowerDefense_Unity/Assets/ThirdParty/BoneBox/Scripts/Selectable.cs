using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BoneBox
{
    public class Selectable : MonoBehaviour
    {
        private Action m_Selected;
        private Action m_Deselected;

        public event Action Selected
        {
            add { m_Selected += value; }
            remove { m_Selected -= value; }
        }

        public event Action Deselected
        {
            add { m_Deselected += value; }
            remove { m_Deselected -= value; }
        }

        [SerializeField] private bool m_IsSelected;

        public bool IsSelected => m_IsSelected;

        public virtual void Select()
        {
            m_IsSelected = true;
            OnSelect();
        }

        public virtual void Deselect()
        {
            m_IsSelected = false;
            OnDeselect();
        }

        protected virtual void OnSelect()
        {
            m_Selected?.Invoke();
        }

        protected virtual void OnDeselect()
        {
            m_Deselected?.Invoke();
        }
    }
}
