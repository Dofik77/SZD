namespace RedRockStudio.SZD.Enemy
{
	public interface IEnemyComponent
	{
		void Initialize();

		void Dispose();
	}
	
	public interface IEnemyUpdater
	{
		void Update(float deltaTime);
	}
}