namespace RedRockStudio.SZD.Character
{
	public interface ICharacterComponent
	{
		void Initialize();

		void Dispose();
	}

	public interface ICharacterUpdater
	{
		void Update(float deltaTime);
	}
}