using System.Collections;
using System.Collections.Generic;

namespace BoneBox.UI
{
	public interface IHideableUI
	{
		bool IsVisible { get; }

		void Show();
		void Hide();
	}
}
