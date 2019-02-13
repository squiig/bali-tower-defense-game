using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

namespace BoneBox
{
    public class Draggable : Selectable, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] private RectTransform m_Canvas;
        [SerializeField] private bool m_ResetOnRelease;

        private Vector3 m_OriginalPosition;
        private Vector3 m_SnapBackPosition;

        public RectTransform Canvas => m_Canvas;

        /// <summary>
        /// The position that the draggable resets to if ResetOnRelease is true.
        /// </summary>
        public Vector3 SnapBackPosition
        {
            get {
                return m_SnapBackPosition;
            }

            set {
                m_SnapBackPosition = value;
            }
        }

        public bool ResetOnRelease
        {
            get {
                return m_ResetOnRelease;
            }

            set {
                m_ResetOnRelease = value;
            }
        }

        void Start()
        {
            m_OriginalPosition = gameObject.transform.position;
            m_SnapBackPosition = m_OriginalPosition;
        }

        public void UpdatePosition()
        {
            this.transform.position = GetConvertedMousePosition();
        }

        public void ResetPosition()
        {
            this.transform.position = m_SnapBackPosition;
        }

        private Vector3 GetConvertedMousePosition()
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_Canvas, Input.mousePosition, null, out localPoint);
            return m_Canvas.TransformPoint(localPoint);
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (!IsSelected)
                return;

            UpdatePosition();
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (!IsSelected)
                return;

            if (m_ResetOnRelease)
            {
                ResetPosition();
            }

            Deselect();
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            Select();
        }
    }
}
