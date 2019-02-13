using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BoneBox
{
    public class Slot : MonoBehaviour
    {
        private Action<Slot, Slottable2D> m_Attached;
        private Action<Slot, Slottable2D> m_Detached;
        private Action m_Locked;
        private Action m_Unlocked;

        public event Action<Slot, Slottable2D> Attached
        {
            add { m_Attached += value; }
            remove { m_Attached -= value; }
        }

        public event Action<Slot, Slottable2D> Detached
        {
            add { m_Detached += value; }
            remove { m_Detached -= value; }
        }

        public event Action Locked
        {
            add { m_Locked += value; }
            remove { m_Locked -= value; }
        }

        private Slottable2D m_Value = null;
        [SerializeField] private bool m_IsLocked;

        public Slottable2D Value => m_Value;
        public bool IsLocked => m_IsLocked;
        public bool IsEmpty => Value == null;
        
        public virtual bool TryAttach(Slottable2D slottable)
        {
            if (m_IsLocked)
                return false;

            if (!IsEmpty)
                Detach();

            OnAttach(slottable);
            return true;
        }

        public virtual void Detach()
        {
            if (m_IsLocked || IsEmpty)
                return;
            
            OnDetach();
        }

        public void Lock()
        {
            m_IsLocked = true;
            OnLock();
        }

        public void Unlock()
        {
            m_IsLocked = false;
            OnUnlock();
        }

        protected virtual void OnAttach(Slottable2D slottable)
        {
            m_Value = slottable;
            m_Attached?.Invoke(this, slottable);
        }

        protected virtual void OnDetach()
        {
            m_Value = null;
            m_Detached?.Invoke(this, m_Value);
        }

        protected virtual void OnLock()
        {
            m_Locked?.Invoke();
        }

        protected virtual void OnUnlock()
        {
            m_Unlocked?.Invoke();
        }
    }
}
