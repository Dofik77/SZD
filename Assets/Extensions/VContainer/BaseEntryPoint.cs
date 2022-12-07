using System;
using System.Collections.Generic;
using VContainer.Unity;

namespace Extensions.VContainer
{
	public abstract class BaseEntryPoint<T> : IPostStartable, IDisposable
	{
		private readonly IEnumerable<T> _components;

		protected BaseEntryPoint(IEnumerable<T> components) => 
			_components = components;

		public void PostStart()
		{
			foreach (T component in _components)
				Start(component);
		}

		public void Dispose()
		{
			foreach (T component in _components)
				Dispose(component);
		}

		protected abstract void Start(T value);

		protected abstract void Dispose(T value);
	}

	public abstract class TickableEntryPoint<T, TTickable> : BaseEntryPoint<T>, ITickable
	{
		private readonly IEnumerable<TTickable> _updaters;

		protected TickableEntryPoint(IEnumerable<T> components, IEnumerable<TTickable> updaters)
			: base(components) =>
			_updaters = updaters;

		public void Tick()
		{
			foreach (TTickable tickableComponent in _updaters)
				Tick(tickableComponent);
		}

		protected abstract void Tick(TTickable updater);
	}
}