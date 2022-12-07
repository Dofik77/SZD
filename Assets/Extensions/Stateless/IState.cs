using System;

namespace Stateless
{
	public interface IState
	{
		event Action Finished;
		
		void Enter();

		void Process(float deltaTime);
		
		void Exit();
	}

	public interface IPayloadedState<in TPayload> : IState
	{
		void Enter(TPayload payload);
	}
}