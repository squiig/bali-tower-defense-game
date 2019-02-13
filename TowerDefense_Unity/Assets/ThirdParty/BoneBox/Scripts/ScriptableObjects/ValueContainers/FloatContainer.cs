using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BoneBox.ScriptableObjects.ValueContainers
{
    [CreateAssetMenu(menuName = BoneBox.ASSET_MENU_ROOT + "/Basic Value Container/Float Container")]
    public class FloatContainer : ValueContainer<float>
    {
        [SerializeField] private float m_Value;

        public override float Value => m_Value;
    }
}
