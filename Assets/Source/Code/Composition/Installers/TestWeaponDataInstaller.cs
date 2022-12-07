using System;
using System.Linq;
using R1noff.RData;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace RedRockStudio.SZD.Composition
{
	[Serializable]
	public sealed class TestWeaponDataInstaller : Installer
	{
		[SerializeField] private DataProvider<WeaponData>[] _providers;

		public override void Install(IContainerBuilder builder) =>
			builder.RegisterInstance(_providers.Select(p => p.Read()).ToArray());
	}
}