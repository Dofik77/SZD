namespace RedRockStudio.SZD.Character
{
	public class Weapon : IWeapon
	{
		private readonly IWeaponControl _control;
		private readonly IWeaponTriggers _triggers;

		public WeaponConfig Config { get; }
		
		public WeaponGraphicSettings GraphicSettings { get; }

		public Weapon(
			WeaponConfig config,
			WeaponGraphicSettings graphicSettings,
			IWeaponControl control,
			IWeaponTriggers triggers)
		{
			_control = control;
			_triggers = triggers;
			Config = config;
			GraphicSettings = graphicSettings;
		}

		public void Initialize()
		{
			_control.Initialize();
			_triggers.Initialize();
		}

		public void Update(float deltaTime)
		{
			_control.Update(deltaTime);
			_triggers.Update(deltaTime);
		}

		public void Dispose()
		{
			_control.Dispose();
			_triggers.Dispose();
		}
	}
}