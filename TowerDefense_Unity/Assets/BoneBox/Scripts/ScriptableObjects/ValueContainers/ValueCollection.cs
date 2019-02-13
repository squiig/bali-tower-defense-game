using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BoneBox.ScriptableObjects.ValueContainers
{
    public abstract class ValueCollection<T> : ScriptableObject
    {
        [SerializeField] private string m_Label;

        public string Label
        {
            get {
                string name = string.IsNullOrEmpty(m_Label) ? Values.ToString() : m_Label;
                return name;
            }
        }

        public abstract List<T> Values { get; }
    }
}
