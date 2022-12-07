using Cysharp.Threading.Tasks;

namespace RedRockStudio.SZD.Enemy
{
	public interface IRagdoll
	{
		bool IsActive { get; }
		
		void Activate();

		bool IsStopped();

		Orientation CalculateOrientation();

		UniTask Deactivate();
	}
}