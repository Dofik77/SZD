namespace RedRockStudio.SZD.Character
{
	public interface IWeapon : ICharacterComponent, ICharacterUpdater
	{
		WeaponConfig Config { get; }
		WeaponGraphicSettings GraphicSettings { get; }
	}
}