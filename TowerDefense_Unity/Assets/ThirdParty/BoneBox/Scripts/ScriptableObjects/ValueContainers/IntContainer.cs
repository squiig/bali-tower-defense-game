using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BoneBox.ScriptableObjects.ValueContainers
{
    [CreateAssetMenu(menuName = BoneBox.ASSET_MENU_ROOT + "/Basic Value Container/Int Container")]
    public class IntContainer : ValueContainer<int>
    {
        [SerializeField] private int m_Value;

        public override int Value => m_Value;
    }
}
