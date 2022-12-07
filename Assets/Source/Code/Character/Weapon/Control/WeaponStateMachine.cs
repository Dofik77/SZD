using Stateless;

namespace RedRockStudio.SZD.Character
{
	public class WeaponStateMachine : IWeaponControl
	{
		private enum Trigger
		{
			Finished,
			Use,
			Block,
			Unblock,
			Reload
		}

		private readonly IWeaponUsingState _useWeaponState;
		private readonly IReloadingState _reloadingState;
		private readonly Magazine _magazine;
		private readonly EmptyState _idleState = new("Idle");
		private readonly EmptyState _blockedState = new("Blocked");
		
		private StateMachine<IState, Trigger> _fsm;

		public bool Blocked { get; private set; }

		public WeaponStateMachine(
			IWeaponUsingState useWeaponState,
			IReloadingState reloadingState,
			Magazine magazine)
		{
			_magazine = magazine;
			_useWeaponState = useWeaponState;
			_reloadingState = reloadingState;
		}

		public void Initialize()
		{
			_fsm = new StateMachine<IState, Trigger>(_idleState);
			ConfigureIdleState();
			ConfigureBlockedState();
			ConfigureWeaponUsingState();
			ConfigureReloadingState();
		}

		public void Update(float deltaTime) => 
			_fsm.State.Process(deltaTime);

		public void Dispose() => 
			_fsm = null;

		public void Block()
		{
			Blocked = true;
			_fsm.Fire(Trigger.Block);
		}

		public void Unblock()
		{
			Blocked = false;
			_fsm.Fire(Trigger.Unblock);
		}

		public void Reload() => 
			_fsm.Fire(Trigger.Reload);

		public void StartUsing() => 
			_fsm.Fire(Trigger.Use);

		public void StopUsing()
		{
			if (_fsm.IsInState(_useWeaponState))
				_fsm.Fire(Trigger.Finished);
		}

		private void ConfigureIdleState() =>
			_fsm.Configure(_idleState)
			    .PermitDynamic(
				    Trigger.Use,
				    () => _magazine.Count > 0 ? _useWeaponState : _reloadingState)
			    .Permit(Trigger.Block, _blockedState)
			    .Permit(Trigger.Reload, _reloadingState)
			    .Ignore(Trigger.Finished, Trigger.Unblock);

		private void ConfigureBlockedState() =>
			_fsm.Configure(_blockedState)
			    .Permit(Trigger.Unblock, _idleState)
			    .Permit(Trigger.Reload, _reloadingState)
			    .Ignore(Trigger.Block, Trigger.Use, Trigger.Finished);

		private void ConfigureWeaponUsingState() =>
			_fsm.Configure(_useWeaponState)
			    .ConnectEntryExit()
			    .OnFinished(Trigger.Finished)
			    .Permit(Trigger.Finished, _idleState)
			    .Permit(Trigger.Block, _blockedState)
			    .PermitReentryIf(Trigger.Use, () => _magazine.Count > 0)
			    .Ignore(Trigger.Reload, Trigger.Unblock);

		private void ConfigureReloadingState() =>
			_fsm.Configure(_reloadingState)
			    .ConnectEntryExit()
			    .OnFinished(Trigger.Finished)
			    .PermitDynamic(Trigger.Finished, () => Blocked ? _blockedState : _idleState)
			    .Ignore(Trigger.Block, Trigger.Unblock, Trigger.Use, Trigger.Reload);
	}
}