using System;
using MoreMountains.Feedbacks;

namespace RedRockStudio.SZD.Character
{
	[Serializable]
	public class FirearmSingleShooting : IWeaponUsingState
	{
		public event Action Finished;
		
		private readonly IWeaponGraphic _graphic;
		private readonly IShooting _shooting;
		private readonly WeaponConfig _config;
		private readonly Magazine _magazine;

		public FirearmSingleShooting(IWeaponGraphic graphic, IShooting shooting, WeaponConfig config, Magazine magazine)
		{
			_graphic = graphic;
			_shooting = shooting;
			_config = config;
			_magazine = magazine;
		}

		public void Enter()
		{
			_graphic.StartUsing();
			_graphic.Use();
			_shooting.Shoot(_config);
			_magazine.Spend();
			_graphic.StopUsing();
			Finished?.Invoke();
		}

		public void Process(float _) { }

		public void Exit() { }
	}
}