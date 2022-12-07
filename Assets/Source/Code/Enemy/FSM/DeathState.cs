using System;

namespace RedRockStudio.SZD.Enemy
{
	public class DeathState : IEnemyState
	{
		private readonly IRagdoll _ragdoll;

		public event Action Finished
		{
			add => throw new NotSupportedException();
			remove { }
		}

		public DeathState(IRagdoll ragdoll) =>
			_ragdoll = ragdoll;

		public void Enter()
		{
			if(!_ragdoll.IsActive)
				_ragdoll.Activate();
		}

		public void EnterFromKneels() =>
			Enter();

		public void Process(float deltaTime) { }

		public void Exit() => 
			throw new NotSupportedException();
	}
}