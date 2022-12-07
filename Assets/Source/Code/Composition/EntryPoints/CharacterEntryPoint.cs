using System;
using System.Collections.Generic;
using Extensions.VContainer;
using RedRockStudio.SZD.Character;
using UnityEngine;
using VContainer.Unity;

namespace RedRockStudio.SZD.Composition
{
	public class CharacterEntryPoint : TickableEntryPoint<ICharacterComponent, ICharacterUpdater>
	{
		public CharacterEntryPoint(
			IEnumerable<ICharacterComponent> components,
			IEnumerable<ICharacterUpdater> updaters)
			: base(components, updaters) { }

		protected override void Start(ICharacterComponent value) => 
			value.Initialize();

		protected override void Dispose(ICharacterComponent value) => 
			value.Dispose();

		protected override void Tick(ICharacterUpdater updater) => 
			updater.Update(Time.deltaTime);
	}
}