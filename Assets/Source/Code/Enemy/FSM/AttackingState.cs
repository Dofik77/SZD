using System;
using System.Threading.Tasks;
using RedRockStudio.SZD.Enemy.Graphics;

namespace RedRockStudio.SZD.Enemy
{
	public class AttackingState : IEnemyState
	{
		public event Action Finished;

		private readonly IBody _body;
		private readonly IEnemyGraphics _graphics;

		private bool _interrupted;
		private bool _kneeled;
		
		public AttackingState(IBody body, IEnemyGraphics graphics)
		{
			_body = body;
			_graphics = graphics;
		}
		
		public void Enter()
		{
			_kneeled = false;
			OnEntered();
		}

		public void EnterFromKneels()
		{
			_kneeled = true;
			OnEntered();
		}

		public void Process(float deltaTime) { }

		public void Exit() =>
			_interrupted = true;

		private async void OnEntered()
		{
			_interrupted = false;
			if (_body.Arms.Injuries == Injuries.Full)
				await _graphics.Bite(_kneeled);
			else
				await _graphics.Attack(_kneeled);
			if (!_interrupted)
				Finished?.Invoke();
		}
	}
}