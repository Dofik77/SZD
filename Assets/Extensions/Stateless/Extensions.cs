using System;

namespace Stateless
{
	public static class Extensions
	{
		public static StateMachine<TState, TTrigger>.StateConfiguration OnFinished<TState, TTrigger>(
			this StateMachine<TState, TTrigger>.StateConfiguration stateConfiguration, Action action)
			where TState : IState
		{
			return stateConfiguration.OnEntry(() => stateConfiguration.State.Finished += action, "AddOnFinished")
			                         .OnExit(() => stateConfiguration.State.Finished -= action, "RemoveOnFinished");
		}

		public static StateMachine<TState, TTrigger>.StateConfiguration OnFinished<TState, TTrigger>(
			this StateMachine<TState, TTrigger>.StateConfiguration stateConfiguration, TTrigger trigger) 
			where TState : IState
		{
			return stateConfiguration.OnFinished(() => stateConfiguration.Machine.Fire(trigger));
		}

		public static StateMachine<TState, TTrigger>.StateConfiguration ConnectEntryExit<TState, TTrigger>(
			this StateMachine<TState, TTrigger>.StateConfiguration stateConfiguration) where TState : IState =>
			stateConfiguration.OnEntry(stateConfiguration.State.Enter).OnExit(stateConfiguration.State.Exit);

		public static StateMachine<TState, TTrigger>.StateConfiguration Ignore<TState, TTrigger>(
			this StateMachine<TState, TTrigger>.StateConfiguration stateConfiguration,
			params TTrigger[] triggers)
		{
			foreach (TTrigger trigger in triggers)
				stateConfiguration = stateConfiguration.Ignore(trigger);
			return stateConfiguration;
		}
	}
}