using UnityEngine;

namespace RedRockStudio.SZD.Character
{
	public interface IWeaponGraphic
	{
		void SetUp(WeaponGraphicSettings settings, float reloadingTime);

		void StartUsing();

		void StopUsing();

		void Use();

		void UseSecondHand();

		void ReloadWeapon();
	}
}