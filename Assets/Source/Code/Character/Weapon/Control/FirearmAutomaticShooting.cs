using System;
using MoreMountains.Feedbacks;

namespace RedRockStudio.SZD.Character
{
	[Serializable]
	public class FirearmAutomaticShooting : IWeaponUsingState
	{
		private const int SecondsInMinute = 60;

		public event Action Finished;

		private readonly WeaponConfig _config;
		private readonly IWeaponGraphic _graphic;
		private readonly IShooting _shooting;
		private readonly Magazine _magazine;

		private float _shootDelay;
		private float _shootTimestamp;

		public FirearmAutomaticShooting(
			WeaponConfig config, IWeaponGraphic graphic, IShooting shooting, Magazine magazine)
		{
			_config = config;
			_graphic = graphic;
			_shooting = shooting;
			_magazine = magazine;
		}

		public void Enter()
		{
			_graphic.StartUsing();
			_shootDelay = _config.FireRatePerMinute / SecondsInMinute;
			_shootTimestamp = 0;
		}

		public void Process(float deltaTime)
		{
			_shootTimestamp += deltaTime;
			if (_shootTimestamp >= _shootDelay)
			{
				_graphic.Use();
				_shooting.Shoot(_config);
				_magazine.Spend();
				_shootTimestamp = 0;
			}
			if(_magazine.Count == 0)
				Finished?.Invoke();
		}

		public void Exit() =>
			_graphic.StopUsing();

	}
}