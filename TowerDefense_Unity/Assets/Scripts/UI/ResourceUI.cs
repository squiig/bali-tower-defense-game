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
        private const string ZERO_PADDING_TOOLTIP = "How many digits are used for the amount, where any leftover digits will be zeroes (e.g. 00042 is a padding of five).";

        [SerializeField] private TMP_Text _AmountText = null;
        [SerializeField, Tooltip(ZERO_PADDING_TOOLTIP)] private int _ZeroPadding = 5;

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
            SetAmount(amount);
        }

		/// <summary>
		/// Sets the visual number to the desired amount.
		/// </summary>
        public void SetAmount(int amount)
        {
            _AmountText.text = amount.ToString($"D{ _ZeroPadding }");
        }
    }
}
