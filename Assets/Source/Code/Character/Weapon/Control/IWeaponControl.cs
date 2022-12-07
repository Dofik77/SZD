namespace RedRockStudio.SZD.Character
{
	public interface IWeaponControl : ICharacterComponent, ICharacterUpdater
	{
		void Block();

		void Unblock();

		void Reload();

		void StartUsing();

		void StopUsing();

		bool Blocked { get; }
	}
}