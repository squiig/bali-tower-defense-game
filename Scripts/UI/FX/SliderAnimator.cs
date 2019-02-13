using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace BoneBox.UI.FX
{
    [DisallowMultipleComponent, RequireComponent(typeof(Slider))]
    public class SliderAnimator : MonoBehaviour
    {
        private Action m_Finished;

        public event Action FinishedEvent
        {
            add {
                m_Finished += value;
            }
            remove {
                m_Finished -= value;
            }
        }

        [SerializeField] private float m_Speed;
        [SerializeField] private AnimationCurve m_Curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        private Coroutine m_CurrentAnimationRoutine;
        private Slider m_Slider;
        private float m_OriginalValue;

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

        public Slider Slider
        {
            get {
                return m_Slider;
            }

            set {
                m_Slider = value;
            }
        }

        private void Awake()
        {
            m_Slider = this.GetComponent<Slider>();
            m_OriginalValue = m_Slider.value;
        }

        public void Animate(float from, float to, float speed, bool ignoreTimeScale = false)
        {
            if (IsAnimating)
            {
                Stop();
            }

            m_CurrentAnimationRoutine = StartCoroutine(AnimationRoutine(from, to, speed, ignoreTimeScale));
        }

        public void Animate(float from, float to)
        {
            Animate(from, to, m_Speed);
        }

        public void Animate(float to)
        {
            Animate(m_Slider.value, to, m_Speed);
        }

        public void Stop()
        {
            StopCoroutine(m_CurrentAnimationRoutine);
            m_CurrentAnimationRoutine = null;
        }

        public void ResetValue()
        {
            m_Slider.value = m_OriginalValue;
        }

        private IEnumerator AnimationRoutine(float from, float to, float speed, bool ignoreTimeScale)
        {
            float timer = 0.0f;

            m_Slider.value = from;

            while (!Mathf.Approximately(m_Slider.value, to))
            {
                timer += (ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime) * speed;

                m_Slider.value = Mathf.Lerp(from, to, m_Curve.Evaluate(timer));

                yield return null;
            }

            // Mathematical safety
            m_Slider.value = to;

            OnAnimationFinished();

            m_CurrentAnimationRoutine = null;
        }

        protected virtual void OnAnimationFinished()
        {
            m_Finished?.Invoke();
        }
    }
}
