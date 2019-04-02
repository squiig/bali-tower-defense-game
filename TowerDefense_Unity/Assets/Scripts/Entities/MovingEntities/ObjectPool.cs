using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Entities.Interfaces;
using UnityEngine;

namespace Game.Entities.MovingEntities
{
	/// <inheritdoc cref="Singleton{T}"/>
	/// <summary>
	/// Object pool for generic objects.
	/// T is constrained to UnityEngine.Object
	/// Can be used as collection, indexer is get only;
	/// </summary>
	/// <typeparam name="T"> Class or object you wish to pool.</typeparam>
	public abstract class SceneObjectPool<T, U> : MonoBehaviourSingleton<U>, ICollection<T> where T : IPoolable where U : Component
	{
        protected readonly List<T> _objects = new List<T>();

		public T this[int index] => _objects[index];

		public int Count => _objects.Count;
		
		public int ActiveCount => _objects.Count(x => x.IsConducting());

		public bool IsReadOnly => false;

		public IEnumerator<T> GetEnumerator() => _objects.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Add(T item) => _objects.Add(item);

		public void Clear() => _objects.Clear();

		public bool Contains(T item) => _objects.Any(x => x.Equals(item));

		public void CopyTo(T[] array, int arrayIndex) => _objects.CopyTo(array, arrayIndex);

		public bool Remove(T item) => _objects.Remove(item);

		/// <summary>
		/// Activates the first object if any that is not active.
		/// Any poolable object with '<see cref="IPoolable.IsConducting"/>'
		/// set to false will be included.
		/// </summary>
		/// <param name="predicate">any predicate selecting what to enable.</param>
		public virtual T ActivateObject(Func<T, bool> predicate)
		{
			T obj = _objects.Where(x => !x.IsConducting()).FirstOrDefault(predicate);
			obj?.Activate();
			return obj;
        }		
	}

	/// <summary>
	/// Object pool for generic objects.
	/// T is constrained to UnityEngine.Object
	/// Can be used as collection, indexer is get only;
	/// </summary>
	/// <typeparam name="T"> Class or object you wish to pool.</typeparam>
	public class MemoryObjectPool<T> :  ICollection<T> where T : IPoolable
	{
		private static MemoryObjectPool<T> s_Instance;

		public static MemoryObjectPool<T> Instance => s_Instance ?? (s_Instance = new MemoryObjectPool<T>());

		private readonly List<T> _objects = new List<T>();

		public T this[int index] => _objects[index];

		public int Count => _objects.Count;

		public bool IsReadOnly => false;

		public IEnumerator<T> GetEnumerator() => _objects.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Add(T item) => _objects.Add(item);

		public void Clear() => _objects.Clear();

		public bool Contains(T item) => _objects.Any(x => x.Equals(item));

		public void CopyTo(T[] array, int arrayIndex) => _objects.CopyTo(array, arrayIndex);

		public bool Remove(T item) => _objects.Remove(item);

		/// <summary>
		/// Activates the first object if any that is not active.
		/// Any poolable object with '<see cref="IPoolable.IsConducting"/>'
		/// set to false will be included.
		/// </summary>
		/// <param name="predicate">any predicate selecting what to enable.</param>
		public void ActivateObject(Func<T, bool> predicate)
		{
			_objects.Where(x => !x.IsConducting()).FirstOrDefault(predicate)?.Activate();
		}
	}
}
