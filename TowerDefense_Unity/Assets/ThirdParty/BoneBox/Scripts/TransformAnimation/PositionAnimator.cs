using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BoneBox.Animation
{
    public class PositionAnimator : MonoBehaviour
    {
        private Action m_AnimationFinished;

        public event Action AnimationFinished
        {
            add {
                m_AnimationFinished += value;
            }
            remove {
                m_AnimationFinished -= value;
            }
        }

        [SerializeField] private float m_Speed;
        [SerializeField] private AnimationCurve m_Curve = null;

        private RectTransform m_RectTransform;

        private Coroutine m_CurrentAnimationRoutine;

        public bool IsUI => m_RectTransform != null;
        public bool IsAnimating => m_CurrentAnimationRoutine != null;

        public float Speed
        {
            get {
                return m_Speed;
            }

            set {
                m_Speed = value;
            }
        }

        public AnimationCurve Curve
        {
            get {
                return m_Curve;
            }

            set {
                m_Curve = value;
            }
        }

        private void Awake()
        {
            m_RectTransform = this.GetComponent<RectTransform>();
        }

        public void Animate(Vector3 from, Vector3 to, float speed, bool ignoreTimeScale = false)
        {
            if (IsAnimating)
            {
                Stop();
            }

            m_CurrentAnimationRoutine = StartCoroutine(AnimationRoutine(from, to, speed, ignoreTimeScale));
        }

        public void Animate(Vector3 from, Vector3 to)
        {
            Animate(from, to, m_Speed);
        }

        public void Animate(Vector3 to)
        {
            Animate(IsUI ? (Vector3) m_RectTransform.anchoredPosition : this.transform.position, to, m_Speed);
        }

        public void Stop()
        {
            StopCoroutine(m_CurrentAnimationRoutine);
            m_CurrentAnimationRoutine = null;
        }

        private IEnumerator AnimationRoutine(Vector3 from, Vector3 to, float speed, bool ignoreTimeScale)
        {
            float timer = 0.0f;

            while (Vector3.Distance((IsUI ? (Vector3) m_RectTransform.anchoredPosition : this.transform.position), to) > 0.1f)
            {
                timer += (ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime) * speed;
                if (IsUI)
                {
                    m_RectTransform.anchoredPosition = Vector2.Lerp(from, to, m_Curve.Evaluate(timer));
                }
                else
                {
                    this.transform.position = Vector3.Lerp(from, to, m_Curve.Evaluate(timer));
                }
                yield return null;
            }

            // Mathematical safety
            if (IsUI)
            {
                m_RectTransform.anchoredPosition = to;
            }
            else
            {
                this.transform.position = to;
            }

            OnAnimationFinished();

            m_CurrentAnimationRoutine = null;
        }

        protected virtual void OnAnimationFinished()
        {
            m_AnimationFinished?.Invoke();
        }
    }
}
