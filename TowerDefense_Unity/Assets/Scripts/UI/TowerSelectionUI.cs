using UnityEngine;
using UnityEngine.UI;

public class TowerSelectionUI : MonoBehaviour
{
	[SerializeField] private Animator _Animator;
	[SerializeField] private Button _ButtonMenu;

	[SerializeField] private bool _Active;

	private void Awake()
	{
		if (_ButtonMenu == null)
		{
			Debug.LogError("Please assing the Button Menu button in the inspector");

			this.enabled = false;
			return;
		}

		_Animator = GetComponent<Animator>();

		_Active = false;
		SetButtonOnClickState();
	}

	#region Animation Events
	public void SetButtonOnClickState()
	{
		//Reset the onClick from the buttons
		_ButtonMenu.onClick.RemoveAllListeners();

		//Has the Tower Selection UI been activated yet? If so, set the right onCLick event to the button
		if(_Active)
			_ButtonMenu.onClick.AddListener(DeactivateTowerSelectionUI);
		else if (!_Active)
			_ButtonMenu.onClick.AddListener(ActivateTowerSelectionUI);
	}

	public void ActivateTowerSelectionUI()
	{
		_Animator.SetTrigger("FadeIn");
		_Animator.SetBool("Active", true);

		_Active = true;
	}
	public void DeactivateTowerSelectionUI()
	{
		_Animator.SetBool("Active", false);

		_Active = false;
	}
	#endregion
}
