using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Game.UI
{
    public class ResourceUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _AmountText = null;

        private int _Amount = 0;

        public int Amount => _Amount;

        private void OnEnable()
        {
            // subscribe AmountChanged to backend here
        }

        private void OnDisable()
        {
            // unsubscribe AmountChanged from backend here
        }

        private void AmountChanged(int amount)
        {
            _Amount = amount;
            Refresh();
        }

        public void Refresh()
        {
            _AmountText.text = _Amount.ToString("D4");
        }
    }
}
