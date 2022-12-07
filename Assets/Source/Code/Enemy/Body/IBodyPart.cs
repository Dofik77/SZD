using System;

namespace RedRockStudio.SZD.Enemy
{
	public interface IBodyPart : IEnemyComponent
	{
		event Action Damaged;
		
		Injuries Injuries { get; }
		
		bool HasBeenHalved(bool reset);
		
		bool HaveBeenKilled(bool reset);

		bool HaveAccumulated(int damage, bool reset);
	}
}