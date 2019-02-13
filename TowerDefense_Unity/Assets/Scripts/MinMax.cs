using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    [Serializable]
    public struct MinMaxFloat
    {
        [SerializeField] private float m_Min;
        [SerializeField] private float m_Max;

        public float Min { get { return m_Min; } set { m_Min = value; } }
        public float Max { get { return m_Max; } set { m_Max = value; } }

        public MinMaxFloat(float min, float max)
        {
            m_Min = min;
            m_Max = max;
        }

        public float GetRandom()
        {
            return Random.Range(Min, Max);
        }
    }

    [Serializable]
    public struct MinMaxInt
    {
        [SerializeField] private int m_Min;
        [SerializeField] private int m_Max;

        public int Min { get { return m_Min; } set { m_Min = value; } }
        public int Max { get { return m_Max; } set { m_Max = value; } }

        public MinMaxInt(int min, int max)
        {
            m_Min = min;
            m_Max = max;
        }

        public int GetRandom()
        {
            return Random.Range(Min, Max);
        }
    }
}
