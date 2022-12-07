using Extensions.VContainer;
using RedRockStudio.SZD.Character;
using VContainer;
using VContainer.Unity;

namespace RedRockStudio.SZD.Composition.Factories
{
	public sealed class WeaponFactory : IFactory<WeaponData, IWeapon>
	{
		private readonly LifetimeScope _parentScope;

		public WeaponFactory(LifetimeScope parentScope) => 
			_parentScope = parentScope;

		public IWeapon Create(WeaponData data)
		{
			LifetimeScope weaponScope = _parentScope.CreateChild(new WeaponInstaller(data));
			return weaponScope.Container.Resolve<IWeapon>();
		}
	}
}