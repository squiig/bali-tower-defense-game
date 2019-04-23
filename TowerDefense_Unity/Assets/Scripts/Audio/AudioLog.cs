using System.Diagnostics;
using Debug = UnityEngine.Debug;


namespace Game.Audio
{
	public static class AudioLog
	{
		private const string ASSERT_FAIL_MSG = "Assertion failed";
		private const string TAG = "[Audio]";

		public static void Log(string msg) => Debug.Log($"{TAG} {msg}");
		public static void Warning(string msg) => Debug.LogWarning($"{TAG} {msg}");
		public static void Error(string msg) => Debug.LogError($"{TAG} {msg}");
		public static void Assertion(string msg) => Debug.LogAssertion($"{TAG} {msg}");

		public static void Assert(bool expr) => Assert(expr, ASSERT_FAIL_MSG);
		public static void Assert(bool expr, string msg)
		{
			if (!expr)
				Assertion(msg);
		}
		
		public static void InvAssert(bool expr) => InvAssert(expr, ASSERT_FAIL_MSG);
		public static void InvAssert(bool expr, string msg)
		{
			if (expr) Assertion(msg);
		}

		public static void HardAssert(bool expr) => HardAssert(expr, ASSERT_FAIL_MSG);
		public static void HardAssert(bool expr, string msg)
		{
			if (!expr)
				throw new AudioSysException(msg);
		}

		public static void HardInvAssert(bool expr) => HardInvAssert(expr, ASSERT_FAIL_MSG);
		public static void HardInvAssert(bool expr, string msg)
		{
			if (expr)
				throw new AudioSysException(msg);
		}
	}
}
