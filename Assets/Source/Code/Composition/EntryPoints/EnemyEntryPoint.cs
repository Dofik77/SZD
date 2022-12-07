using System.Collections.Generic;
using Extensions.VContainer;
using RedRockStudio.SZD.Enemy;
using UnityEngine;
// ReSharper disable PossibleMultipleEnumeration

namespace RedRockStudio.SZD.Composition
{
	public class EnemyEntryPoint : TickableEntryPoint<IEnemyComponent, IEnemyUpdater>
	{
		public EnemyEntryPoint(
			IEnumerable<IEnemyComponent> components,
			IEnumerable<IEnemyUpdater> updaters)
			: base(components, updaters) { }

		protected override void Start(IEnemyComponent value) =>
			value.Initialize();

		protected override void Dispose(IEnemyComponent value) =>
			value.Dispose();

		protected override void Tick(IEnemyUpdater updater) =>
			updater.Update(Time.deltaTime);
	}
}