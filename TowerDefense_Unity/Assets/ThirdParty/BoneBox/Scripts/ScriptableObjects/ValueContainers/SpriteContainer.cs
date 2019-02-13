using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BoneBox.ScriptableObjects.ValueContainers
{
    [CreateAssetMenu(menuName = BoneBox.ASSET_MENU_ROOT + "/Basic Value Container/Sprite Container")]
    public class SpriteContainer : ValueContainer<Sprite>
    {
        [SerializeField] private Sprite m_Value;

        public override Sprite Value => m_Value;
    }
}
