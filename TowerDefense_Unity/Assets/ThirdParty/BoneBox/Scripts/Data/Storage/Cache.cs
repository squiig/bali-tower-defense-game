using System.Collections;
using System.Collections.Generic;

namespace BoneBox.Data.Storage
{
	/// <summary>
	/// Use this class for storing variables that will be remembered during runtime but will be forgotten when exiting.
	/// </summary>
	public class Cache
	{
		private Dictionary<string, object> m_Cache;

		public static Cache Global { get; } = new Cache();

		public Cache()
		{
			m_Cache = new Dictionary<string, object>();
		}

		#region Public Methods

		public void Clear()
		{
			m_Cache.Clear();
		}

		public void Remove(string key)
		{
			m_Cache.Remove(key);
		}

		public void Save(string key, object value)
		{
			if (m_Cache.ContainsKey(key))
			{
				m_Cache.Remove(key);
			}

			m_Cache.Add(key, value);
		}

		public object Load(string key)
		{
			object obj = null;
			m_Cache.TryGetValue(key, out obj);
			return obj;
		}

		public bool Contains(string key)
		{
			return m_Cache.ContainsKey(key);
		}

		#endregion
	}
}