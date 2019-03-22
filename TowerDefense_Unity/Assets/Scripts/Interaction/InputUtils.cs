using UnityEngine;

public static class InputUtils
{
	public static float GetTouchDistanceDelta(Touch touch1, Touch touch2)
	{
		Vector2 pos1 = touch1.position;
		Vector2 pos2 = touch2.position;
		Vector2 del1 = touch1.deltaPosition;
		Vector2 del2 = touch2.deltaPosition;

		float DistBefore = Vector2.Distance(pos1 - del1, pos2 - del2);
		float DistAfter = Vector2.Distance(pos1, pos2);
		return DistBefore - DistAfter;
	}
}
