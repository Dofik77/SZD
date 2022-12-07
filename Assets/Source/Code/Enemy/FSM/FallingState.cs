using System;
using RedRockStudio.SZD.Enemy.Graphics;

namespace RedRockStudio.SZD.Enemy
{
	public class FallingState : IEnemyState
	{
		public event Action Finished;

		private const int Delay = 1;

		private readonly IRagdoll _ragdoll;
		private readonly IEnemyGraphics _graphics;

		private float _timestamp;

		public FallingState(IRagdoll ragdoll, IEnemyGraphics graphics)
		{
			_ragdoll = ragdoll;
			_graphics = graphics;
		}

		public void Enter()
		{
			_ragdoll.Activate();
			_graphics.Fall();
			_timestamp = 0;
		}

		public void EnterFromKneels() => 
			Enter();

		public void Process(float deltaTime)
		{
			if (_timestamp <= Delay)
			{
				_timestamp += deltaTime;
				return;
			}
			if (_ragdoll.IsStopped())
				Finished?.Invoke();
		}

		public void Exit() { }
	}
}