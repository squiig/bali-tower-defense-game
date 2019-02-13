using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

namespace BoneBox
{
    public class Slottable2D : Draggable
    {
        private Slot m_HoveredSlot;
        private Slot m_CurrentSlot;

        public bool IsAttached => m_CurrentSlot != null;
        public bool IsSlotAvailable => m_HoveredSlot != null;
        
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (!IsSelected)
                return;

            m_HoveredSlot = collision.gameObject.GetComponent<Slot>();
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (!IsSelected)
                return;

            m_HoveredSlot = null;
        }
        
        public override void OnPointerUp(PointerEventData eventData)
        {
            if (!IsSelected)
                return;

            if (IsSlotAvailable && m_HoveredSlot.TryAttach(this))
            {
                this.transform.position = m_HoveredSlot.transform.position;
                SnapBackPosition = m_HoveredSlot.transform.position;
            }
            else
            {
                if (ResetOnRelease)
                {
                    ResetPosition();
                }
            }

            m_HoveredSlot = null;

            Deselect();
        }
    }
}
