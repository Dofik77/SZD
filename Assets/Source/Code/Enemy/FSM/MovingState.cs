using System;
using System.Collections.Generic;
using MyBox;
using RedRockStudio.SZD.Enemy.Graphics;
using RedRockStudio.SZD.Enemy.HandsLogic;
using Stateless;
using UnityEngine;

namespace RedRockStudio.SZD.Enemy
{
	public class MovingState : IEnemyState, IEnemyComponent
	{
		private enum State
		{
			Running,
			Walking,
			Limping,
			Crawling
		}

		private enum Trigger
		{
			Damaged,
			Reset
		}

		public event Action Finished;

		private const int DamageForDefenseTorso = 3;
		private const int DamageForDefenseHead = 1;

		private readonly IEnemyGraphics _graphics;
		private readonly Transform _transform;
		private readonly EnemyConfig _config;
		private readonly IBody _body;
		private readonly IEnumerable<ILockable> _lockables;

		private StateMachine<State, Trigger> _fsm;
		private bool _isActive;

		public MovingState(
			IEnemyGraphics graphics, Transform transform, EnemyConfig config, IBody body, IEnumerable<ILockable> lockables)
		{
			_graphics = graphics;
			_transform = transform;
			_config = config;
			_body = body;
			_lockables = lockables;
		}

		public void Initialize()
		{
			_fsm = new StateMachine<State, Trigger>(_config.IsRunning ? State.Running : State.Walking);
			if (_config.IsRunning)
				ConfigureRunningState();
			ConfigureWalkingState();
			ConfigureLimpingState();
			_body.Damaged += OnDamaged;
		}

		public void Dispose() =>
			_body.Damaged -= OnDamaged;

		public void Enter()
		{
			_isActive = true;
			_graphics.SetMovingSpeed(1);
			_graphics.ChangeMoving(true);
			_fsm.Fire(Trigger.Reset);
			if(!_fsm.IsInState(State.Crawling))
				_lockables.ForEach(l => l.Unlock());
		}

		public void EnterFromKneels() =>
			Enter();

		public void Process(float deltaTime)
		{
			if (NeedDefense())
			{
				Finished?.Invoke();
				return;
			}
			float speed = _fsm.State switch
			{
				State.Walking => _config.WalkingSpeed,
				State.Running => _config.RunningSpeed,
				State.Limping => _config.LimpingSpeed,
				State.Crawling => _config.CrawlingSpeed,
				_ => throw new InvalidOperationException()
			};
			_transform.Translate(Vector3.left * speed * deltaTime);
		}

		public void Exit()
		{
			_isActive = false;
			_graphics.ChangeMoving(false);
			_lockables.ForEach(l => l.Lock());
		}

		private void OnDamaged()
		{
			if (_isActive)
				_fsm.Fire(Trigger.Damaged);
		}

		private void ConfigureRunningState() =>
			_fsm.Configure(State.Running)
			    .OnEntry(() => _graphics.ChangeRunning(true))
			    .OnExit(
				    () =>
				    {
					    _config.IsRunning = false;
					    _graphics.ChangeRunning(false);
				    })
			    .PermitDynamic(Trigger.Reset, ChooseState)
			    .Ignore(Trigger.Damaged);

		private void ConfigureWalkingState() =>
			_fsm.Configure(State.Walking)
			    .PermitIf(Trigger.Damaged, State.Limping, () => _body.Legs.Injuries == Injuries.Low)
			    .IgnoreIf(Trigger.Damaged, () => _body.Legs.Injuries != Injuries.Low)
			    .PermitDynamic(Trigger.Reset, ChooseState);

		private void ConfigureLimpingState() =>
			_fsm.Configure(State.Limping)
			    .PermitDynamic(Trigger.Reset, ChooseState)
			    .Ignore(Trigger.Damaged);

		private State ChooseState() =>
			_body.Legs.Injuries switch
			{
				Injuries.None => State.Walking,
				Injuries.Low => State.Limping,
				Injuries.Medium => State.Limping,
				Injuries.Full => State.Limping,//TODO Crawling
				_ => throw new InvalidOperationException()
			};

		private bool NeedDefense() =>
			!_fsm.IsInState(State.Running) && 
			(_body.Head.HaveAccumulated(DamageForDefenseHead, true) || 
				_body.Torso.HaveAccumulated(DamageForDefenseTorso, true)); //TODO arms and legs
	}
}