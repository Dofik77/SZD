using System;

namespace RedRockStudio.SZD.Character
{
	[Serializable]
	public class ReloadingState : IReloadingState
	{
		private readonly WeaponConfig _config;
		private readonly IWeaponGraphic _graphic;
		private readonly Magazine _magazine;

		public event Action Finished;

		private float _timestamp;
		
		public ReloadingState(WeaponConfig config, IWeaponGraphic weapon, Magazine magazine)
		{
			_config = config;
			_graphic = weapon;
			_magazine = magazine;
		}

		public void Enter()
		{
			_graphic.ReloadWeapon();
			_timestamp = 0;
		}

		public void Process(float deltaTime)
		{
			_timestamp += deltaTime;
			if(_timestamp >= _config.ReloadingTime)
			{
				_magazine.Reload(_config.AmmoSize);
				Finished?.Invoke();
			}
		}

		public void Exit() { }
	}
}