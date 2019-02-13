using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BoneBox.ScriptableObjects.ValueContainers
{
    [CreateAssetMenu(menuName = BoneBox.ASSET_MENU_ROOT + "/Basic Value Container/String Container")]
    public class StringContainer : ValueContainer<string>
    {
        [SerializeField, TextArea(1, 40)] private string m_Value;

        public override string Value => m_Value;
    }
}
