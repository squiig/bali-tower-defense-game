using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Game.UI
{
	/// <summary>
	/// This component allows for dynamically displaying an amount in UI text, with customizable zero padding.
	/// </summary>
    public class ResourceUI : MonoBehaviour
    {
        private const string ZERO_PADDING_TOOLTIP = "How many digits are used for the amount, where any leftover digits will be zeroes (e.g. 00042 is a padding of five).";

        [SerializeField] private TMP_Text _AmountText = null;
        [SerializeField, Tooltip(ZERO_PADDING_TOOLTIP)] private int _ZeroPadding = 5;

		/// <summary>
		/// Sets the visual number to the desired amount.
		/// </summary>
        public void SetAmount(int amount)
        {
            _AmountText.text = amount.ToString($"D{ _ZeroPadding }");
        }
    }
}
