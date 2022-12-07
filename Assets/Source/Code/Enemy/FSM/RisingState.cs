using System;
using RedRockStudio.SZD.Enemy.Graphics;

namespace RedRockStudio.SZD.Enemy
{
	public sealed class RisingState : IEnemyState
	{
		public event Action Finished;

		private readonly IEnemyGraphics _graphics;
		private readonly IRagdoll _ragdoll;

		private bool _interrupted;
		
		public RisingState(IEnemyGraphics graphics, IRagdoll ragdoll)
		{
			_graphics = graphics;
			_ragdoll = ragdoll;
		}

		public async void Enter()
		{
			Orientation orientation = _ragdoll.CalculateOrientation();
			if (orientation == Orientation.Straight)
			{
				_graphics.SetMovingSpeed(10);
				_graphics.Kneel();
			}
			else
			{
				_graphics.FalledInDirection(orientation == Orientation.Prostrate);
			}
			await _ragdoll.Deactivate();
			EnterFromKneels();
		}

		public async void EnterFromKneels()
		{
			_interrupted = false;
			await _graphics.Rise();
			if(!_interrupted)
				Finished?.Invoke();
		}

		public void Process(float _) { }

		public void Exit() =>
			_interrupted = true;
	}
}