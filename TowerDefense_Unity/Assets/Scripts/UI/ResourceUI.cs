using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Game.UI
{
    /// <summary>
    /// This script makes sure the corresponding resource value in the backend gets properly displayed in the UI elements.
    /// </summary>
    public class ResourceUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _AmountText = null;
        [SerializeField] private int _ZeroPadding = 5;

        private int _Amount = 0;

        /// <summary>
        /// The amount being displayed. Call Refresh to update its visual after changing.
        /// </summary>
        public int Amount => _Amount;

        private void OnEnable()
        {
            //TODO: subscribe AmountChanged to backend here
        }

        private void OnDisable()
        {
            //TODO: unsubscribe AmountChanged from backend here
        }

        /// <summary>
        /// Event hook to be called when the resource value changes in backend.
        /// </summary>
        /// <param name="amount"></param>
        private void AmountChanged(int amount)
        {
            _Amount = amount;
            Refresh();
        }

        /// <summary>
        /// Refreshes the visuals according to their values.
        /// </summary>
        public void Refresh()
        {
            _AmountText.text = _Amount.ToString($"D{ _ZeroPadding }");
        }
    }
}
