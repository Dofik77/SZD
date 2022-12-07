using System;
using System.Collections.Generic;
using System.Linq;
using Stateless;
using UnityEngine;

namespace RedRockStudio.SZD.Enemy
{
	public class EnemyStateMachine : IEnemyComponent, IEnemyUpdater, IEnemyControl
	{
		private const int DamageForFall = 3;

		private enum Trigger
		{
			Finished,
			Damaged,
			Attack,
			Kicked,
			Dead
		}

		private readonly Dictionary<Type, IEnemyState> _states;
		private readonly IBody _body;
		private readonly IRagdoll _ragdoll;
		private readonly EnemyConfig _config;
		private readonly IdleState _idleState;

		private StateMachine<IEnemyState, Trigger> _fsm;
		private bool _isKneeled;
		private bool _attackAvaliable;

		public EnemyStateMachine(IEnumerable<IEnemyState> states, IBody body, IRagdoll ragdoll, EnemyConfig config)
		{
			_states = states.ToDictionary(s => s.GetType(), s => s);
			_states.Add(typeof(EmptyState), new EmptyState("InitialState"));
			_body = body;
			_ragdoll = ragdoll;
			_config = config;
			_idleState = _states[typeof(IdleState)] as IdleState;
		}

		public void Initialize()
		{
			_fsm = new StateMachine<IEnemyState, Trigger>(_states[typeof(EmptyState)]);
			ConfigureInitialState();
			ConfigureMovingState();
			ConfigureFallingState();
			ConfigureRisingState();
			ConfigureAttackingState();
			ConfigureKickedState();
			ConfigureCrawlingBackState();
			ConfigureIdleState();
			ConfigureDeathState();
			_fsm.Activate();
		}

		public void Update(float deltaTime)
		{
			_fsm.State.Process(deltaTime);
#if DEBUG
			Debug.Log(_fsm.State);
#endif
		}

		public void Dispose() { }

		public void Kick() =>
			_fsm.Fire(Trigger.Kicked);

		public void Die() =>
			_fsm.Fire(Trigger.Dead);

		public void Damage() =>
			_fsm.Fire(Trigger.Damaged);

		public void AttackAvaliable()
		{
			_fsm.Fire(Trigger.Attack);
			_attackAvaliable = true;
		}

		public void AttackNotAvaliable() =>
			_attackAvaliable = false;

		private void ConfigureInitialState() =>
			_fsm.Configure(_states[typeof(EmptyState)])
			    .OnActivate(() => _fsm.Fire(Trigger.Finished))
			    .Permit(Trigger.Finished, _states[typeof(MovingState)]);

		private void ConfigureMovingState() =>
			_fsm.Configure(_states[typeof(MovingState)])
			    .OnEntry(Enter<MovingState>)
			    .OnExit(Exit<MovingState>)
			    .OnFinished(Trigger.Finished)
			    .Permit(Trigger.Finished, _states[typeof(IdleState)])
			    .Permit(Trigger.Attack, _states[typeof(AttackingState)])
			    .Permit(Trigger.Kicked, _states[typeof(KickedState)])
			    .Permit(Trigger.Dead, _states[typeof(DeathState)])
				.PermitIf(Trigger.Damaged, _states[typeof(FallingState)], CanFall)
				.IgnoreIf(Trigger.Damaged, CanNotFall);

		private void ConfigureFallingState() =>
			_fsm.Configure(_states[typeof(FallingState)])
			    .ConnectEntryExit()
			    .OnEntry(() => _isKneeled = false)
			    .OnFinished(Trigger.Finished)
			    .PermitDynamic(Trigger.Finished, RiseOrCrawl)
			    .Permit(Trigger.Dead, _states[typeof(DeathState)])
			    .Ignore(Trigger.Damaged, Trigger.Attack);

		private void ConfigureRisingState() => 
			_fsm.Configure(_states[typeof(RisingState)])
			    .OnEntry(Enter<RisingState>)
			    .OnExit(Exit<RisingState>)
			    .OnFinished(Trigger.Finished)
			    .PermitDynamic(Trigger.Finished, OnKneeledChangedState)
			    .Permit(Trigger.Dead, _states[typeof(DeathState)])
			    .PermitIf(Trigger.Damaged, _states[typeof(FallingState)], CanFall)
			    .IgnoreIf(Trigger.Damaged, CanNotFall)
				.Ignore(Trigger.Attack);

		private void ConfigureAttackingState() => 
			_fsm.Configure(_states[typeof(AttackingState)])
			    .OnEntry(Enter<AttackingState>)
			    .OnExit(Exit<AttackingState>)
			    .OnFinished(Trigger.Finished)
			    .PermitIf(Trigger.Finished, _states[typeof(MovingState)], () => !_attackAvaliable, $"!{nameof(_attackAvaliable)}")
			    .PermitReentryIf(Trigger.Finished, () => _attackAvaliable, $"{nameof(_attackAvaliable)}")
			    .Permit(Trigger.Kicked, _states[typeof(KickedState)])
			    .Permit(Trigger.Dead, _states[typeof(DeathState)])
			    .PermitIf(Trigger.Damaged, _states[typeof(FallingState)], CanFall)
			    .IgnoreIf(Trigger.Damaged, CanNotFall)
				.Ignore(Trigger.Attack);

		private void ConfigureKickedState() => 
			_fsm.Configure(_states[typeof(KickedState)])
			    .OnEntry(Enter<KickedState>)
			    .OnExit(Exit<KickedState>)
			    .OnFinished(Trigger.Finished)
			    .Permit(Trigger.Finished, _states[typeof(IdleState)])
			    .Permit(Trigger.Dead, _states[typeof(DeathState)])
			    .PermitReentryIf(Trigger.Kicked, () => !_ragdoll.IsActive, "RagdollNotActive")
			    .PermitIf(Trigger.Damaged, _states[typeof(FallingState)], CanFall)
			    .IgnoreIf(Trigger.Damaged, CanNotFall)
				.Ignore(Trigger.Attack);

		private void ConfigureCrawlingBackState() => 
			_fsm.Configure(_states[typeof(CrawlingBackState)])
			    .ConnectEntryExit()
			    .Permit(Trigger.Dead, _states[typeof(DeathState)])
			    .PermitIf(Trigger.Damaged, _states[typeof(FallingState)], CanFall)
			    .IgnoreIf(Trigger.Damaged, CanNotFall)
				.Ignore(Trigger.Attack);

		private void ConfigureIdleState() => 
			_fsm.Configure(_states[typeof(IdleState)])
			    .OnEntry(Enter<IdleState>)
			    .OnExit(Exit<IdleState>)
			    .OnFinished(Trigger.Finished)
			    .PermitDynamicIf(Trigger.Finished, OnKneeledChangedState, _idleState.IsKneeled)
			    .PermitIf(Trigger.Finished, _states[typeof(MovingState)], () => !_idleState.IsKneeled(), "IsNotKneeled")
			    .Permit(Trigger.Attack, _states[typeof(AttackingState)])
			    .Permit(Trigger.Kicked, _states[typeof(KickedState)])
			    .Permit(Trigger.Dead, _states[typeof(DeathState)])
			    .PermitIf(Trigger.Damaged, _states[typeof(FallingState)], CanFall)
			    .IgnoreIf(Trigger.Damaged, CanNotFall);

		private void ConfigureDeathState() =>
			_fsm.Configure(_states[typeof(DeathState)])
			    .ConnectEntryExit()
			    .Ignore(Trigger.Damaged, Trigger.Attack, Trigger.Finished, Trigger.Kicked, Trigger.Dead);

		private bool CanFall() =>
			CheckFallCondition(false);

		private bool CanNotFall() =>
			!CheckFallCondition(true);

		private bool CheckFallCondition(bool reset) =>
			!_ragdoll.IsActive && 
			(_body.Head.HasBeenHalved(reset) || _body.Torso.HasBeenHalved(reset) || _body.Legs.HaveBeenKilled(reset) ||
			_config.IsRunning && _body.Head.HaveAccumulated(DamageForFall, reset));

		private IEnemyState RiseOrCrawl() =>
         _body.Legs.Injuries == Injuries.Full && _ragdoll.CalculateOrientation() == Orientation.Supine?
             _states[typeof(CrawlingBackState)] : _states[typeof(RisingState)];

		private IEnemyState OnKneeledChangedState()
		{
			if (_attackAvaliable)
			{
				_isKneeled = !_isKneeled;
				return _states[typeof(AttackingState)];
			}
			if (_isKneeled)
			{
				_isKneeled = false;
				return _states[typeof(MovingState)];
			}
			_isKneeled = true;
			return _body.Legs.Injuries == Injuries.Full ? 
				_states[typeof(IdleState)] : _states[typeof(RisingState)];
		}

		private void Enter<TState>()
		{
			if(_isKneeled)
				_states[typeof(TState)].EnterFromKneels();
			else
				_states[typeof(TState)].Enter();
		}

		private void Exit<TState>() =>
			_states[typeof(TState)].Exit();
	}
}