using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BoneBox.ScriptableObjects.ValueContainers
{
    public abstract class ValueContainer<T> : ScriptableObject
    {
        [SerializeField] private string m_Label;

        public string Label
        {
            get {
                string name = string.IsNullOrEmpty(m_Label) ? Value.ToString() : m_Label;
                return name;
            }
        }

        public abstract T Value { get; }
    }
}
