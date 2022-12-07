using RedRockStudio.SZD.Services.Input;

namespace RedRockStudio.SZD.Character
{
	public class WeaponControlTriggers : IWeaponTriggers 
	{
		private readonly IWeaponControl _control;
		private readonly IKeysInput _keysInput;
		private readonly IPointerInput _pointerInput;
		private readonly IMovement _movement;

		public WeaponControlTriggers(
			IWeaponControl control, IKeysInput keysInput, IPointerInput pointerInput, IMovement movement)
		{
			_control = control;
			_keysInput = keysInput;
			_pointerInput = pointerInput;
			_movement = movement;
		}

		public void Initialize()
		{
			_pointerInput.ShootingChanged += OnShootingChanged;
			_keysInput.Reloaded += _control.Reload;
		}

		private void OnShootingChanged(bool newState)
		{
			if(newState)
				_control.StartUsing();
			else
				_control.StopUsing();
		}

		public void Update(float _)
		{
			bool isKicking = _movement.IsKicking;
			if (isKicking != _control.Blocked)
			{
				if(isKicking)
					_control.Block();
				else
					_control.Unblock();
			}
		}

		public void Dispose()
		{
			_pointerInput.ShootingChanged -= OnShootingChanged;
			_keysInput.Reloaded -= _control.Reload;
		}
	}
}