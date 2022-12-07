using System;

namespace Stateless
{
	public class EmptyState : IState
	{
		public event Action Finished
		{
			add => throw new NotSupportedException();
			remove { }
		}

		private readonly string _name;

		public EmptyState(string name)
		{
			_name = name;
		}

		public void Enter() { }

		public void Process(float _) { }

		public void Exit() { }

		public override string ToString() => _name;
	}
}