using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BoneBox.ScriptableObjects.ValueContainers
{
    [CreateAssetMenu(menuName = BoneBox.ASSET_MENU_ROOT + "/Basic Value Container/String Collection")]
    public class StringCollection : ValueCollection<string>
    {
        [SerializeField] private List<string> m_Values = null;

        public override List<string> Values => m_Values;
    }
}
