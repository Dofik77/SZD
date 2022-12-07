using Cysharp.Threading.Tasks;

namespace RedRockStudio.SZD.Enemy.Graphics
{
	public interface IEnemyGraphics
	{
		void Fall();

		UniTask Attack(bool kneeled);

		UniTask Bite(bool kneeled);

		UniTask Rise();

		void Kneel();

		void FalledInDirection(bool forward);

		void SetMovingSpeed(float value);

		UniTask ChangeMoving(bool value);
		
		void ChangeRunning(bool value);
	}
}