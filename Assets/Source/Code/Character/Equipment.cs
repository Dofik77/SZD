using System;
using RedRockStudio.SZD.Services.Input;

namespace RedRockStudio.SZD.Character
{
	public sealed class Equipment : ICharacterComponent, ICharacterUpdater
	{
		private readonly WeaponData[] _weaponsData;
		private readonly Func<WeaponData, IWeapon> _factory;
		private readonly IKeysInput _keysInput;
		private readonly IAiming _aiming;
		private readonly IWeaponGraphic _graphic;

		private IWeapon[] _weapons;
		private int _current;
		private bool _isSwitching;

		public Equipment(
			WeaponData[] weaponsData,
			Func<WeaponData, IWeapon> factory,
			IKeysInput keysInput,
			IAiming aiming,
			IWeaponGraphic graphic)
		{
			_weaponsData = weaponsData;
			_factory = factory;
			_keysInput = keysInput;
			_aiming = aiming;
			_graphic = graphic;
		}

		public void Initialize()
		{
			_weapons = new IWeapon[_weaponsData.Length];
			for (var i = 0; i < _weaponsData.Length; i++)
				_weapons[i] = _factory(_weaponsData[i]);
			_keysInput.SwitchedWeapon += OnWeaponSwitched;
			SetUpGraphic();
			_weapons[_current].Initialize();
		}

		public void Update(float deltaTime)
		{
			if(!_isSwitching)
				_weapons[_current].Update(deltaTime);
		}

		public void Dispose()
		{
			_keysInput.SwitchedWeapon -= OnWeaponSwitched;
			_weapons[_current].Dispose();
		}

		private void OnWeaponSwitched()
		{
			_isSwitching = true;
			_weapons[_current].Dispose();
			_current = ++_current % _weapons.Length;
			_aiming.AnimateSwitching(SetUpGraphic, SwitchWeapon);
		}

		private void SetUpGraphic() => 
			_graphic.SetUp(_weapons[_current].GraphicSettings, _weapons[_current].Config.ReloadingTime);

		private void SwitchWeapon()
		{
			_weapons[_current].Initialize();
			_isSwitching = false;
		}
	}
}