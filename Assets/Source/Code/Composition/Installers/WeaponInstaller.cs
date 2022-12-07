using RedRockStudio.SZD.Character;
using VContainer;
using VContainer.Unity;

namespace RedRockStudio.SZD.Composition
{
	public sealed class WeaponInstaller : IInstaller
	{
		private readonly WeaponData _data;
		
		public WeaponInstaller(WeaponData data) => 
			_data = data;

		public void Install(IContainerBuilder builder)
		{
			WeaponConfig config = _data.WeaponConfigProvider.Read();
			builder.RegisterInstance(config);
			builder.RegisterInstance(_data.GraphicSettings);
			builder.Register(_data.UsingType.Type, Lifetime.Singleton).As<IWeaponUsingState>();
			builder.Register(_data.ReloadingType.Type, Lifetime.Singleton).As<IReloadingState>();
			builder.Register<WeaponStateMachine>(Lifetime.Singleton).As<IWeaponControl>();
			builder.Register<WeaponControlTriggers>(Lifetime.Singleton).As<IWeaponTriggers>();
			builder.Register<Magazine>(Lifetime.Singleton).WithParameter(config.AmmoSize);
			builder.Register<Weapon>(Lifetime.Singleton).As<IWeapon>();
		}
	}
}