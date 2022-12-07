namespace RedRockStudio.SZD.Services
{
	public interface IService
	{
		void Initialize();

		void Dispose();
	}

	public interface IUpdateService
	{
		void Update(float deltaTime);
	}
}