using System;
using UnityEngine;

namespace R1noff.RData
{
	public abstract class DataProvider<TData> : ScriptableObject
	{
		[field: SerializeField] protected TData Value { get; private set; }

		public virtual TData Read() =>
			Value;
	}

	public abstract class MutableDataProvider<TData> : DataProvider<TData>
	{
		public event Action Changed;
		
		private TData _override;

		public override TData Read() =>
			_override ?? Value;

		public virtual void Write(TData value)
		{
			_override = value;
			Changed?.Invoke();
		}
	}

	public abstract class SavedDataProvider<TData> : MutableDataProvider<TData>
	{
		[SerializeField] private SaveService _saveService;
		[SerializeField] private string _key;

		private TData _cached;

		public override TData Read()
		{
			_cached = _saveService.Load(_key, Value);
			return _cached;
		}

		public override void Write(TData value)
		{
			_cached = value;
			_saveService.Save(_key, value);
			base.Write(value);
		}
	}

	public abstract class SaveService : ScriptableObject
	{
		public abstract TData Load<TData>(string key, TData defaultData);

		public abstract void Save<TData>(string key, TData value);
	}
}